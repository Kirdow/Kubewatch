using System;
using YamlDotNet.Serialization;

namespace Kubewatch.Data
{
    [Serializable]
    public class Solve
    {
        public string[] Sequence;
        public DateTime Time;
        public float Elapsed;
    
        public Solve()
        {
            Sequence = new string[0];
            Elapsed = 0.0f;
            Time = DateTime.UtcNow;
        }

        public Solve(string[] _sequence, float _elapsed = 0.0f, DateTime? _time = null)
        {
            Sequence = _sequence;
            Elapsed = _elapsed;
            Time = _time ?? DateTime.UtcNow;
        }

        public Solve(float _elapsed, DateTime? _time = null, params string[] _sequence)
        {
            Sequence = _sequence;
            Elapsed = _elapsed;
            Time = _time ?? DateTime.UtcNow;
        }

        [YamlIgnoreAttribute]
        public string ElapsedString => GetElapsedString(Elapsed);

        public static string GetElapsedString(float elapsed, bool isMs = true)
        {
            int seconds = (int)elapsed;
            int decimals = ((int)(elapsed * (isMs ? 1000.0f : 100.0f)) % (isMs ? 1000 : 100));
            int minutes = seconds / 60;
            seconds %= 60;

            return $"{minutes}:{seconds.ToString().PadLeft(2, '0')}.{decimals.ToString().PadLeft(isMs ? 3 : 2, '0')}";
        }
    }
}