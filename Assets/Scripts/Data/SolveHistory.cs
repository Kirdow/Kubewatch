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
        
        private static SolveHistory GetSolveHistory()
        {
            return LoadSolveHistory(GetFilePath());
        }

        private static void SetSolveHistory(SolveHistory history)
        {
            SaveSolveHistory(GetFilePath(), history);
        }

        public static void AddSolve(Solve solve)
        {
            var history = GetSolveHistory();
            var list = history.Solves.ToList();
            list.Add(solve);
            history.Solves = list.ToArray();
            SetSolveHistory(history);
        }

        public static void RemoveSolve(int index)
        {
            var history = GetSolveHistory();
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
            SetSolveHistory(history);
        }

        public static bool ResoreSolve()
        {
            if (_removedSolve?.Solve == null) return false;

            var history = GetSolveHistory();
            var list = history.Solves.ToList();
            _removedSolve = _removedSolve?.Restore(list);
            history.Solves = list.ToArray();
            SetSolveHistory(history);

            return true;
        }

        public static bool HasRemovedSolves() => _removedSolve?.Solve != null;

        public static Solve[] GetSolves(int count = -1)
        {
            var history = GetSolveHistory();
            count = Math.Min(count, history.Solves.Length);
            if (count < 0) count = history.Solves.Length;
            if (count == 0) return new Solve[0];

            var result = new Solve[count];
            Array.Copy(history.Solves, history.Solves.Length - count, result, 0, count);
            return result;
        }

        public static SolveHistory Empty => new SolveHistory();

        private static RemovedSolve _removedSolve = null;
    }
}