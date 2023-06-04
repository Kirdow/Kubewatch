using UnityEngine;

namespace Kubewatch.Enums
{
    public enum EDirection4
    {
        Up,
        Right,
        Down,
        Left
    }

    public static class EDirection4Ext
    {
        public static EDirection4 GetOpposite(this EDirection4 dir)
        {
            return (EDirection4)(((int)dir + 2) % 4);
        }

        public static EDirection4 NextClockwise(this EDirection4 dir, int repeat = 0)
        {
            EDirection4 result = dir;
            repeat = Mathf.Max(repeat, 0) % 4;
            
            while (repeat-- >= 0)
            {
                result = (EDirection4)(((int)result + 1) % 4);
            }

            return result;
        }

        public static EDirection4 NextCounterClockwise(this EDirection4 dir, int repeat = 0)
        {
            EDirection4 result = dir;
            repeat = Mathf.Max(repeat, 0) % 4;

            while (repeat-- >= 0)
            {
                result = (EDirection4)(((int)result + 3) % 4);
            }

            return result;
        }
    }
}