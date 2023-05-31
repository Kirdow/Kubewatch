using UnityEngine;
using UnityEngine.UI;
using System;

namespace Kubewatch.UI
{
    public class UIIndicator : MonoBehaviour
    {
        [SerializeField] private Color _bestColor;
        [SerializeField] private Color _worstColor;
        [SerializeField] private Color _goodColor;
        [SerializeField] private Color _badColor;

        private Image _indicatorImage;
        private EIndicatorState _state;

        public EIndicatorState State
        {
            get => _state;
            set
            {
                _state = value;
                UpdateState();
            }
        }

        void Awake()
        {
            _indicatorImage = GetComponent<Image>();
            State = EIndicatorState.None;
        }

        public void UpdateState()
        {
            if (_state == EIndicatorState.None)
            {
                _indicatorImage.enabled = false;
                return;
            }

            _indicatorImage.enabled = true;

            Color color = Color.white;
            switch (_state)
            {
                case EIndicatorState.Best: color = _bestColor; break;
                case EIndicatorState.Worst: color = _worstColor; break;
                case EIndicatorState.Good: color = _goodColor; break;
                case EIndicatorState.Bad: color = _badColor; break;
                default:
                    Debug.LogError(_state);
                    _indicatorImage.enabled = false;
                    return;
            }

            _indicatorImage.color = color;
        }
    }

    public enum EIndicatorState
    {
        None,
        Best,
        Worst,
        Good,
        Bad
    }
}