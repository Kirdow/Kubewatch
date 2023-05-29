using UnityEngine;
using System;

namespace Kubewatch
{
    [Serializable]
    public class EdgeReference
    {
        [Header("References")]
        public Face Source;
        public Face Target;

        [Header("Properties")]
        public Vector2Int Left;
        public Vector2Int Right;

        public EStickerColor[] Colors
        {
            get => Target.GetColors(Left, Right);
            set => Target.SetColors(Left, Right, value);
        }
    }
}