using UnityEngine;
using UnityEditor;

namespace Kubewatch
{
    [ExecuteInEditMode]
    public class WatchPoint : MonoBehaviour
    {
        public Vector3 LookAtPosition = new Vector3(0,0,0);

        void Awake()
        {
            LookAt();
        }

        void Update()
        {
            if (transform.hasChanged)
                LookAt();
        }

        private void LookAt()
        {
            transform.LookAt(LookAtPosition, Vector3.up);
        }
    }
}