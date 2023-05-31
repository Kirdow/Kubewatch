using UnityEngine;
using System;
using System.IO;

namespace Kubewatch.Data
{
    public static class FileData
    {
        public static string GetDirectory()
        {
            string path = Application.persistentDataPath;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }

        public static string GetDataDirectory()
        {
            string path = Path.Combine(GetDirectory(), "Data");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }

        public static string GetDataFile(string fileName)
        {
            return Path.Combine(GetDataDirectory(), fileName);
        }
    }
}