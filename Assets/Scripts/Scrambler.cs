using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kubewatch
{
    public class Scrambler
    {
        public int Turns { get; private set; }
        public string[] Sequence { get; private set; }
        public string SequenceString => string.Join(" ", Sequence);

        public Scrambler(int turns = 25)
        {
            Turns = Mathf.Max(10, turns);
            Sequence = new string[turns];
        
            Scramble();
        }

        void Scramble()
        {
            HashSet<ScrambleSide> sides = new HashSet<ScrambleSide>();

            var sideArray = (ScrambleSide[])System.Enum.GetValues(typeof(ScrambleSide));

            foreach (var side in sideArray)
            {
                sides.Add(side);
            }

            string[] extra = new string[]{
                "",
                "'",
                "2"
            };

            for (int j = 0; j < Turns; )
            {
                int k = Random.Range(0, sides.Count);
                ScrambleSide next = sideArray[k];
                if (!sides.Contains(next)) continue;

                sides.Remove(next);
                string nextStr = next.ToString();
                nextStr += extra[Random.Range(0, 3)];

                foreach (var s in next.GetEnableSides())
                {
                    sides.Add(s);
                }

                Sequence[j] = nextStr;
                ++j;            
            }
        }

        public static string GetSequence(int turns = 25) => (new Scrambler(turns)).SequenceString;
    }

    public enum ScrambleSide
    {
        R,
        L,
        B,
        F,
        D,
        U
    }

    public static class ScrambleSideExt
    {
        public static IEnumerable<ScrambleSide> GetEnableSides(this ScrambleSide side)
        {
            switch (side)
            {
                case ScrambleSide.L: goto case ScrambleSide.R;
                case ScrambleSide.R: return new ScrambleSide[] { ScrambleSide.U, ScrambleSide.D, ScrambleSide.F, ScrambleSide.B };
            
                case ScrambleSide.D: goto case ScrambleSide.U;
                case ScrambleSide.U: return new ScrambleSide[] { ScrambleSide.L, ScrambleSide.R, ScrambleSide.F, ScrambleSide.B };

                case ScrambleSide.B: goto case ScrambleSide.F;
                case ScrambleSide.F: return new ScrambleSide[] { ScrambleSide.L, ScrambleSide.R, ScrambleSide.U, ScrambleSide.D };
            }

            return new ScrambleSide[0];
        }
    }
}