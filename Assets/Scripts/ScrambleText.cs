using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Kubewatch
{
    public class ScrambleText : MonoBehaviour
    {
        public static ScrambleText I { get; private set; }

        [Header("References")]
        [InspectorName("Text")]
        public Text TextRef;
        private string[] _sequence = new string[0];
        private int _highlight = -1;

        void Awake()
        {
            I = this;
        }

        public void UpdateText()
        {
            if (_highlight < 0)
            {
                TextRef.text = string.Join(" ", _sequence);
                return;
            }

            var sb = new StringBuilder();
            if (_highlight > 0) sb.Append(string.Join(" ", _sequence.SubArray(0, Mathf.Min(_highlight, _sequence.Length))));
            if (_highlight < _sequence.Length) sb.Append(" ").Append("<color=#00ff00ff>").Append(_sequence[_highlight]).Append("</color>");
            if (_highlight + 1 < _sequence.Length) sb.Append(" ").Append(string.Join(" ", _sequence.SubArray(_highlight + 1, _sequence.Length)));

            TextRef.text = sb.ToString();
        }

        public static string[] Text
        {
            get => I._sequence;
            set
            {
                I._sequence = value;
                I._highlight = -1;
                I.UpdateText();
            }
        }

        public static int Highlight
        {
            get => I._highlight;
            set
            {
                I._highlight = value;
                I.UpdateText();
            }
        }
    }
}