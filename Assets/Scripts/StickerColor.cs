using UnityEngine;

namespace Kubewatch
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
                case EStickerColor.White: return Color.white;
                case EStickerColor.Red: return Color.red;
                case EStickerColor.Blue: return Color.blue;
                case EStickerColor.Yellow: return Color.yellow;
                case EStickerColor.Orange: return new Color(1.0f, 0.65f, 0.0f, 1.0f);
                case EStickerColor.Green: return Color.green;
                default:
                    return Color.black;
            }
        }
    }
}