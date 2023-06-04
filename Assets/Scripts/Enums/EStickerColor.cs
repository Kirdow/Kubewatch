using UnityEngine;

namespace Kubewatch.Enums
{
    public enum EStickerColor
    {
        White,
        Red,
        Blue,
        Yellow,
        Orange,
        Green
    }

    public static class EStickerColorExt
    {
        public static EStickerColor Opposite(this EStickerColor sc)
        {
            return (EStickerColor)(((int)sc + 3) % 3);
        }

        public static Color GetUnityColor(this EStickerColor sc)
        {
            switch (sc)
            {
                case EStickerColor.White: return White;
                case EStickerColor.Red: return Red;
                case EStickerColor.Blue: return Blue;
                case EStickerColor.Yellow: return Yellow;
                case EStickerColor.Orange: return Orange;
                case EStickerColor.Green: return Green;
                default:
                    return Color.black;
            }
        }

        public static readonly Color White = Color.white;
        public static readonly Color Red = new Color(183f / 255f, 18f / 255f, 52f / 255f, 1.0f);
        public static readonly Color Blue = new Color(0f, 70f / 255f, 172f / 255f, 1.0f);
        public static readonly Color Yellow = new Color(1.0f, 213f / 255f, 0f, 1.0f);
        public static readonly Color Orange = new Color(1.0f, 88f / 255f, 0f, 1.0f);
        public static readonly Color Green = new Color(0f, 155f / 255f, 72f / 255f, 1.0f);
    }
}