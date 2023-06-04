using UnityEngine;
using Kubewatch.Enums;

namespace Kubewatch
{
    public class Face : MonoBehaviour
    {
        [Header("Properties")]
        public EStickerColor DefaultColor = EStickerColor.White;

        [NonReorderable]
        public EdgeReference[] Edges = new EdgeReference[4];

        private Sticker[,] _stickers = new Sticker[3, 3];

        void Awake()
        {
            Reset(DefaultColor);

            //transform.rotation *= Quaternion.AngleAxis(-90, Vector3.up);
            //transform.rotation *= Quaternion.AngleAxis(180, Vector3.forward);
        }

        public void Clear()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var sticker = _stickers[i, j];
                    if (sticker != null) Destroy(sticker.gameObject);
                    _stickers[i, j] = null;
                }
            }
        }

        public void Reset()
        {
            Reset(DefaultColor);
        }

        public void Reset(EStickerColor color)
        {
            Clear();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _stickers[i, j] = Sticker.CreateSticker(transform, new Vector2Int(i - 1, j - 1), color);
                }
            }
        }

        public EStickerColor[] GetColors(Vector2Int from, Vector2Int to)
        {
            Vector2Int middle = (from + to) / 2;

            return new EStickerColor[]{
                _stickers[from.x, from.y].StickerColor,
                _stickers[middle.x, middle.y].StickerColor,
                _stickers[to.x, to.y].StickerColor
            };
        }

        public void SetColors(Vector2Int from, Vector2Int to, EStickerColor[] colors)
        {
            Vector2Int middle = (from + to) / 2;

            _stickers[from.x, from.y].SetColor(colors[0]);
            _stickers[middle.x, middle.y].SetColor(colors[1]);
            _stickers[to.x, to.y].SetColor(colors[2]);
        }

        public void FlipColors(EStickerColor facing)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _stickers[i, j].SetColor(GetFlippedColor(_stickers[i, j].StickerColor, facing));
                }
            }
        }

        public static EStickerColor GetFlippedColor(EStickerColor color)
        {
            switch (color)
            {
                case EStickerColor.White: return EStickerColor.Yellow;
                case EStickerColor.Yellow: return EStickerColor.White;
                case EStickerColor.Green: return EStickerColor.Blue;
                case EStickerColor.Blue: return EStickerColor.Green;
                case EStickerColor.Orange: return EStickerColor.Red;
                case EStickerColor.Red: return EStickerColor.Orange;
                default: return color;
            }
        }

        public static EStickerColor GetFlippedColor(EStickerColor color, EStickerColor facing)
        {
            if (color == facing) return color;

            var result = GetFlippedColor(color);
            if (result == facing) return color;

            return result;
        }

        public void RotateFace(bool clockwise = true)
        {
            EStickerColor color;
            int dir0 = clockwise ? 2 : 0;
            int dir2 = 2 - dir0;

            // Face Corners
            color = _stickers[0, 0].StickerColor;
            _stickers[0, 0].SetColor(_stickers[dir2, dir0].StickerColor);
            _stickers[dir2, dir0].SetColor(_stickers[2, 2].StickerColor);
            _stickers[2, 2].SetColor(_stickers[dir0, dir2].StickerColor);
            _stickers[dir0, dir2].SetColor(color);

            // Face Edges
            color = _stickers[1, 0].StickerColor;
            _stickers[1, 0].SetColor(_stickers[dir2, 1].StickerColor);
            _stickers[dir2, 1].SetColor(_stickers[1, 2].StickerColor);
            _stickers[1, 2].SetColor(_stickers[dir0, 1].StickerColor);
            _stickers[dir0, 1].SetColor(color);
        }

        public void RotateEdge(bool clockwise = true)
        {
            // Side Edges
            EStickerColor[] colors = Edges[0].Colors;
            for (int i = 0; i < 4; i++)
            {
                int j = (clockwise ? (3 - i) : (i + 1)) % 4;
                int k = (j + (clockwise ? 1 : 3)) % 4;
                Edges[k].Colors = j == 0 ? colors : Edges[j].Colors;
            }   
        }

        public void Turn(bool clockwise = true)
        {
            RotateEdge(clockwise);
            RotateFace(clockwise);
        }

        public static void ResetAll()
        {
            foreach (var face in GameObject.FindObjectsOfType<Face>())
            {
                face.Reset();
            }
        }
    }
}