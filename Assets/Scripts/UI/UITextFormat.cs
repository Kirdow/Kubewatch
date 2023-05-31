using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Kubewatch.UI
{
    [ExecuteInEditMode]
    public class UITextFormat : MonoBehaviour
    {
        [Header("Properties")]
        public string Format = "{0}";

        private Text _text;
        private string _rawText = string.Empty;

        public string Text
        {
            get => _rawText;
            set
            {
                _rawText = value;
                UpdateText();
            }
        }

        void Awake()
        {
            _text = GetComponent<Text>();
        }

        void Start()
        {
            UpdateText();
        }

        public void UpdateText()
        {
            _text.text = string.Format(Format, _rawText);
        }

        public void Set<T>(T obj)
        {
            if (obj == null)
            {
                Text = string.Empty;
                return;
            }

            Text = obj.ToString();
        }
    }
}