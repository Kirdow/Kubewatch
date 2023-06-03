using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;

namespace Kubewatch.Data
{
    [Serializable]
    public class SolveHistory
    {
        public Solve[] Solves;

        public SolveHistory()
        {
            Solves = new Solve[0];
        }

        public SolveHistory(params Solve[] _solves)
        {
            Solves = _solves;
        }

        public static SolveHistory LoadSolveHistory(string filePath)
        {
            if (!File.Exists(filePath)) return Empty;

            string fileData = string.Empty;
            SolveHistory result = Empty;
            if (Serial.LoadFileString(filePath, ref fileData, "Failed to load solve history")
                && Serial.DeserializeObject<SolveHistory>(fileData, ref result, "Failed to deserialize solve history"))
                return result;

            return Empty;
        }

        public static bool SaveSolveHistory(string filePath, SolveHistory history)
        {
            if (history == null) return false;

            string fileData = string.Empty;
            return Serial.SerializeObject(history, ref fileData, "Failed to serialize solve history")
                && Serial.SaveFileString(filePath, fileData, "Failed to save solve history");
        }

        private static string GetFilePath() => FileData.GetDataFile("history.yml");
        
        private static void GetSolveHistory(Action<SolveHistory> callback)
        {
            Dispatcher.RunAsync(() =>
            {
                try
                {
                    var result = LoadSolveHistory(GetFilePath());
                    Dispatcher.RunOnMainThread(() =>
                    {
                        callback(result);
                    });
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            });
        }

        private static void SetSolveHistory(SolveHistory history, Action callback)
        {
            Dispatcher.RunAsync(() =>
            {
                SaveSolveHistory(GetFilePath(), history);
                Dispatcher.RunOnMainThread(() =>
                {
                    callback();
                });
            });
        }

        public static void AddSolve(Solve solve, Action callback)
        {
            GetSolveHistory(history =>
            {
                var list = history.Solves.ToList();
                list.Add(solve);
                history.Solves = list.ToArray();
                SetSolveHistory(history, callback);
            });
        }

        public static void RemoveSolve(int index, Action callback)
        {
            GetSolveHistory(history =>
            {
                var list = history.Solves.ToList();
                int solveIndex = index >= 0 ? index : (list.Count + index);
                if (solveIndex < list.Count && solveIndex >= 0)
                {
                    Solve solve = list[solveIndex];
                    RemovedSolve removedSolve = new RemovedSolve(solve, _removedSolve);
                    _removedSolve = removedSolve;
                    list.RemoveAt(solveIndex);
                }

                history.Solves = list.ToArray();
                SetSolveHistory(history, callback);
            });
        }

        public static void ResoreSolve(Action<bool> callback)
        {
            if (_removedSolve?.Solve == null)
            {
                callback(false);
                return;
            }

            GetSolveHistory(history =>
            {
                var list = history.Solves.ToList();
                _removedSolve = _removedSolve?.Restore(list);
                history.Solves = list.ToArray();
                SetSolveHistory(history, () => callback(true));
            });
        }

        public static bool HasRemovedSolves() => _removedSolve?.Solve != null;

        public static void GetSolves(Action<Solve[]> callback, int _count = -1)
        {
            GetSolveHistory(history =>
            {
                int count = Math.Min(_count, history.Solves.Length);
                if (count < 0) count = history.Solves.Length;
                if (count == 0)
                {
                    callback(new Solve[0]);
                    return;
                }

                var result = new Solve[count];
                Array.Copy(history.Solves, history.Solves.Length - count, result, 0, count);
                callback(result);
            });
        }

        public static SolveHistory Empty => new SolveHistory();

        private static RemovedSolve _removedSolve = null;
    }
}