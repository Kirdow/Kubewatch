using UnityEngine;
using Kubewatch.Enums;

namespace Kubewatch
{
    public class Sticker : MonoBehaviour
    {
        public const float DistanceRatio = 3.2f / 3.0f;
        
        [Header("Properties")]
        public Vector2Int Position = new Vector2Int(0, 0);
        public EStickerColor StickerColor = EStickerColor.White;
        
        [Header("References")]
        public SpriteRenderer Sprite;

        void Awake()
        {
            Poll();            
        }

        void Update()
        {
            Poll();
        }

        public void SetPosition(Vector2Int pos)
        {
            pos.Clamp(new Vector2Int(-1, -1), new Vector2Int(1, 1));
            Position = pos;
        }

        public void SetColor(EStickerColor color)
        {
            StickerColor = color;
        }

        private void Poll()
        {
            transform.localPosition = (Vector2)Position * DistanceRatio;
            transform.localRotation = Quaternion.identity;
            Sprite.color = StickerColor.GetUnityColor();
        }

        public static Sticker CreateSticker(Transform parent, Vector2Int pos, EStickerColor color)
        {
            GameObject obj = Instantiate(GameManager.I.StickerPrefab, Vector3.zero, Quaternion.identity, parent);
            Sticker sticker = obj.GetComponent<Sticker>();
            sticker.SetPosition(pos);
            sticker.SetColor(color);
            sticker.Poll();
            return sticker;
        }
    }
}