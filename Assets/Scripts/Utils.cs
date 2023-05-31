using UnityEngine;
using System;
using System.Linq;

public static class Extensions
{
    public static T[] SubArray<T>(this T[] array, int startIndex, int endIndex)
    {
        int length = endIndex - startIndex;
        T[] result = new T[length];
        Array.Copy(array, startIndex, result, 0, length);
        return result;
    }

    public static void CopyToClipboard(this string str)
    {
        GUIUtility.systemCopyBuffer = str;
    }

    public static void CopyToClipboard<T>(this T[] arr, string sep)
    {
        var str = string.Join(sep, arr.Select(p => p.ToString()));
        str.CopyToClipboard();
    }
}