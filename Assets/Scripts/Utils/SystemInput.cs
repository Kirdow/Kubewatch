using UnityEngine;
using UnityEngine.UI;

namespace Kubewatch.Utils
{
    public class SystemInput : MonoBehaviour
    {
        public Image ExitImage;
        public float EscapeTimeCooldown = 1.0f;

        private float _escapeTime = 0.0f;
        
        public bool ExitRequested
        {
            get
            {
                #if UNITY_STANDALONE_OSX
                if (Input.GetKey(KeyCode.LeftCommand) && Input.GetKeyDown(KeyCode.W)) return true;
                #endif

                if (!Input.GetKey(KeyCode.Escape))
                {
                    ExitImage.enabled = false;
                    _escapeTime = Time.time;
                }
                else if ((Time.time - _escapeTime) >= EscapeTimeCooldown) return true;
                else
                {
                    ExitImage.transform.localScale = new Vector3((Time.time - _escapeTime) / EscapeTimeCooldown, 1, 1);
                    ExitImage.enabled = true;
                }

                return false;
            }
        }
        
        #if !UNITY_EDITOR
        void Update()
        {
            if (ExitRequested)
            {
                Application.Quit();
            }
        }
        #endif
    }
}