using UnityEngine;
using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using Kubewatch.Enums;
using Kubewatch.Data;

namespace Kubewatch.Options
{
    [Serializable]
    public class Prefs
    {
        public EFlipMode _FlipMode;
        public ESortType _SortType;
        public ESortDirection _SortDirection;

        private static Prefs LoadPrefs(string filePath)
        {
            if (!File.Exists(filePath)) return Empty;

            string fileData = string.Empty;
            Prefs prefs = Empty;
            if (Serial.LoadFileString(filePath, ref fileData, "Failed to load preferences")
                && Serial.DeserializeObject<Prefs>(fileData, ref prefs, "Failed to deserialize preferences"))
                return prefs;

            return Empty;
        }

        private static bool SavePrefs(string filePath, Prefs prefs)
        {
            if (prefs == null) return false;

            string fileData = string.Empty;
            return Serial.SerializeObject(prefs, ref fileData, "Failed to serialize preferences")
                && Serial.SaveFileString(filePath, fileData, "Failed to save preferences");
        }

        private static string GetFilePath() => FileData.GetDataFile("prefs.yml");

        private static Prefs GetPrefs()
        {
            return LoadPrefs(GetFilePath());
        }

        private static void SetPrefs(Prefs prefs)
        {
            SavePrefs(GetFilePath(), prefs);
        }

        public static void SavePrefs(Action<Prefs> callback)
        {
            if (callback == null) return;
            
            var prefs = GetPrefs();
            callback(prefs);
            SetPrefs(prefs);
        }

        public static T LoadPrefs<T>(T _default, Func<Prefs, T> callback)
        {
            if (callback == null) return _default;

            var prefs = GetPrefs();
            return callback(prefs);
        }

        public static EFlipMode FlipMode
        {
            get => LoadPrefs(EFlipMode.None, p => p._FlipMode);
            set => SavePrefs(p => p._FlipMode = value);
        }

        public static ESortType SortType
        {
            get => LoadPrefs(ESortType.Date, p => p._SortType);
            set => SavePrefs(p => p._SortType = value);
        }

        public static ESortDirection SortDirection
        {
            get => LoadPrefs(SortType == ESortType.Date ? ESortDirection.Descending : ESortDirection.Ascending, p => p._SortDirection);
            set => SavePrefs(p => p._SortDirection = value);
        }

        private static Prefs Empty => new Prefs();
    }
}