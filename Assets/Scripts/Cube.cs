using UnityEngine;

namespace Kubewatch
{
    public class Cube : MonoBehaviour
    {
        [Header("References")]
        public Face Front;
        public Face Back;
        public Face Top;
        public Face Bottom;
        public Face Left;
        public Face Right;
    }
}