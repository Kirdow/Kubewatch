using UnityEngine;
using UnityEngine.UI;
using System;
using Kubewatch.Data;


namespace Kubewatch.UI
{
    public class UISolveEntry : MonoBehaviour
    {
        [SerializeField] private UITextFormat _elapsedText;
        [SerializeField] private UITextFormat _dateText;
        [SerializeField] private GameObject _copyObj;
        [SerializeField] private UIIndicator _indicator;

        public int Index { get; private set; } = -1;

        private Solve _solve = null;
        public Solve SolveEntry
        {
            get => _solve;
            set
            {
                _solve = value;
                UpdateSolve();
            }
        }

        void Start()
        {
            UpdateSolve();
        }

        public void UpdateSolve()
        {
            if (_solve == null)
            {
                _elapsedText.Text = "";
                _dateText.Text = "";
                _copyObj.SetActive(false);
                return;
            }

            _elapsedText.Set(_solve.ElapsedString);
            _dateText.Set(_solve.Time);
            _copyObj.SetActive(true);
        }

        public void OnCopyScramble()
        {
            if (_solve == null) return;
            _solve.Sequence.CopyToClipboard<string>(" ");
        }

        public void OnDelete()
        {
            if (_solve == null || Index < 0) return;

            SolveHistory.RemoveSolve(Index);
            UISolveHistory.Inst.Reload();
        }

        public void OnRecall()
        {
            if (_solve == null) return;

            GameManager.I.Recall(_solve);
        }

        private void SetPosition(int index, int pos, EIndicatorState state)
        {
            Index = index;

            var rt = (RectTransform)transform;
            rt.localPosition = new Vector3(6, -6 - 50 * pos, 0);
            rt.localScale = Vector3.one;

            _indicator.State = state;
        }

        public static UISolveEntry CreateInstance(Solve solve, RectTransform parent, int index, int pos, EIndicatorState state)
        {
            var obj = Instantiate(GameManager.I.SolveHistoryEntryPrefab, Vector3.zero, Quaternion.identity, parent);
            UISolveEntry entry = obj.GetComponent<UISolveEntry>();
            entry.SolveEntry = solve;
            entry.SetPosition(index, pos, state);
            return entry;
        }

    }
}