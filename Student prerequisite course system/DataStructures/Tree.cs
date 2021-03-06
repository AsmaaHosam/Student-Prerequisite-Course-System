﻿using System;

public class Tree<T> where T : ITreeElement
{
    //private
    ArrayList<T> Elements;
    ArrayList<ArrayList<int>> AdjacencyList;
    int count;
    int capacity;
    void CheckIndex(params int[] indecies)
    {
        foreach (int n in indecies)
        {
            if (n < 0 || n >= AdjacencyList.Count)
            {
                throw new IndexOutOfRangeException();
            }
        }
    }
    //public
    public Tree()
    {
        AdjacencyList = new ArrayList<ArrayList<int>>();
        Elements = new ArrayList<T>();
        count = 0;
        capacity = 0;
    }
    public int Count
    {
        get => count;
    }
    public void AddElement(T value)
    {
        value.TreeIndex = count;
        Elements.Append(value);
        AdjacencyList.Count++;
    }
    public void Connect(T dependant, T dependee)
    {
        CheckIndex(dependant.TreeIndex, dependee.TreeIndex);
        if (AdjacencyList[dependant.TreeIndex] == null)
        {
            AdjacencyList[dependant.TreeIndex] = new ArrayList<int>();
        }
        AdjacencyList[dependant.TreeIndex].Append(dependee.TreeIndex);
    }
    public T[] GetDependantElements(T Target, T[] AlreadyTaken)
    {
        ArrayList<T> res = new ArrayList<T>();
        Queue<T> q = new Queue<T>();
        q.Enqueue(Target);
        while (q.Count > 0)
        {
            T tmp = q.Top();
            q.Dequeue();
            if (AdjacencyList[tmp.TreeIndex] == null) continue;
            foreach (int n in AdjacencyList[tmp.TreeIndex])
            {
                T tmpTarget = Elements[n];
                if (!Array.Exists(AlreadyTaken, new Predicate<T>((T a) => { return a == tmpTarget; })))
                {
                    q.Enqueue(tmpTarget);
                    res.Append(tmpTarget);
                }
            }
        }
        return res.ToArray();
    }
    public string[] UnloadToFile()
    {
        if (typeof(T) == typeof(Course))
        {
            ArrayList<string> res = new ArrayList<string>();
            System.Text.StringBuilder tmp = new System.Text.StringBuilder();
            for (int i = 0; i < count; i++)
            {
                Course c = Elements[i] as Course;
                tmp.Append(c.Name + " ");
                if (AdjacencyList[i] != null)
                {
                    for (int j = 0; j < AdjacencyList[i].Count; j++)
                    {
                        tmp.Append(AdjacencyList[i][j] + " ");
                    }
                }
                tmp.Remove(tmp.Length - 1, 1);
                res.Append(tmp.ToString());
                tmp.Clear();
            }
            return res.ToArray();
        }
        else
        {
            throw new NotImplementedException("UnloadToFile only works with Course type");
        }
    }
    public void LoadFromFile(string[] filedata)
    {
        if (typeof(T) == typeof(Course))
        {
            AdjacencyList.Count = filedata.Length;
            for (int i = 0; i < filedata.Length; i++)
            {
                AdjacencyList[i] = new ArrayList<int>();
                string[] fields = filedata[i].Split(' ', '\0');
                Course c = FileOperations.CoursesFile.GetCourse(fields[0]);
                c.TreeIndex = i;
                Elements.Append(c as T);
                for (int j = 1; j < fields.Length; j++)
                {
                    AdjacencyList[i].Append(int.Parse(fields[j]));
                }
            }
        }
        else
        {
            throw new NotImplementedException("LoadFromFile only works with Course type");
        }
    }
}

