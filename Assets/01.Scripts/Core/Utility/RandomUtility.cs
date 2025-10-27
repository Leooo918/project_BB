using System.Collections.Generic;
using UnityEngine;

public static class RandomUtility
{
    public static T GetSingleElementInList<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    //public static List<T> ToList<T>(this T value)
    //{
    //    List<T> list = new List<T>();
    //    list.Add(value); 
    //    return list;
    //}
}
