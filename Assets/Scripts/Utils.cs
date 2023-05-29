using UnityEngine;
using System;

public static class Extensions
{
    public static T[] SubArray<T>(this T[] array, int startIndex, int endIndex)
    {
        int length = endIndex - startIndex;
        T[] result = new T[length];
        Array.Copy(array, startIndex, result, 0, length);
        return result;
    }
}