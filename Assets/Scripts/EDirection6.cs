using UnityEngine;

namespace Kubewatch
{
    public enum EDirection6
    {
        Up,
        Front,
        Right,
        Down,
        Back,
        Left
    }

    public static class EDirection6Ext
    {
        public static EDirection6 GetOpposite(this EDirection6 dir)
        {
            return (EDirection6)(((int)dir + 3) % 6);
        }
    }
}