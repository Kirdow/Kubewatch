using UnityEngine.UI;

namespace Kubewatch.UI
{
    public class UIScrollRect : ScrollRect
    {
        public float scrollSpeed = 1f;

        protected override void Awake()
        {
            base.Awake();
            
            this.scrollSensitivity = scrollSpeed * ScrollSpeedPlatformMultiplier;
        }

        public static float ScrollSpeedPlatformMultiplier
        {
            get
            {
                #if UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
                    return 4;
                #elif UNITY_STANDALONE_WIN
                    return 50;
                #else
                    return 1;
                #endif
            }
        }
    }
}