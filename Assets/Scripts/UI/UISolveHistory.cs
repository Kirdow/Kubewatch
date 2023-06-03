using UnityEngine;
using UnityEngine.UI;
using Kubewatch.Data;
using Kubewatch.Enums;
using Kubewatch.Options;
using System.Collections.Generic;
using System.Linq;

namespace Kubewatch.UI
{
    public class UISolveHistory : MonoBehaviour
    {
        [SerializeField] private RectTransform _contentTransform;

        [Space]
        [SerializeField] private UITextFormat _singleBestText;
        [SerializeField] private UITextFormat _averageText;
        [SerializeField] private UITextFormat _average5Text;
        [SerializeField] private UITextFormat _average10Text;
        [InspectorName("Average 3 of 5 Text")]
        [SerializeField] private UITextFormat _average3of5Text;
        [InspectorName("Average 10 of 12 Text")]
        [SerializeField] private UITextFormat _average10of12Text;
        [SerializeField] private UITextFormat _btnSortDateText;
        [SerializeField] private UITextFormat _btnSortTimeText;
        [Space]
        [SerializeField] private Button _btnRestore;

        private SortedSolve[] _solves = new SortedSolve[0];

        private ESortType _sortType = ESortType.Date;
        private ESortDirection _sortDir = ESortDirection.Descending;

        public const string SortAscendingIcon = "\u25B2";
        public const string SortDescendingIcon = "\u25BC";

        private Solve[] Solves
        {
            get => _solves.Select(p => p.Solve).ToArray();
            set
            {
                var solves = value ?? new Solve[0];
                _solves = new SortedSolve[solves.Length];
                for (int i = 0; i < _solves.Length; i++) _solves[i] = new SortedSolve(i, solves[i]);
                
                UpdateSort();
                UpdateRestore();
            }
        }

        void Awake()
        {
            Inst = this;

            _sortType = Prefs.SortType;
            _sortDir = Prefs.SortDirection;
        }

        void Start()
        {
            Reload();
        }

        public void Reload()
        {
            Solves = SolveHistory.GetSolves();
            Debug.Log($"Loaded {_solves.Length} solves!");
        }

        public void OnSortPress(int sortTypeInt)
        {
            ESortType sortType = (ESortType)sortTypeInt;

            if (_sortType != sortType)
                _sortDir = sortType == ESortType.Date ? ESortDirection.Descending : ESortDirection.Ascending;
            else
                _sortDir = _sortDir == ESortDirection.Ascending ? ESortDirection.Descending : ESortDirection.Ascending;
            
            _sortType = sortType;

            Prefs.SavePrefs(p => {
                p._SortType = _sortType;
                p._SortDirection = _sortDir;
            });
            
            UpdateSort();
        }

        public void OnRestorePress()
        {
            if (SolveHistory.ResoreSolve())
                Reload();
        }

        public void UpdateRestore()
        {
            _btnRestore.interactable = SolveHistory.HasRemovedSolves();
        }

        public void UpdateSort()
        {
            _btnSortTimeText.Text = "";
            _btnSortDateText.Text = "";
            UITextFormat btn = (_sortType == ESortType.Date) ? _btnSortDateText : _btnSortTimeText;
            btn.Text = (_sortDir == ESortDirection.Ascending) ? SortAscendingIcon : SortDescendingIcon;

            IEnumerable<SortedSolve> data = _solves;
            if (_sortDir == ESortDirection.Ascending)
                data = (_sortType == ESortType.Date) ? data.OrderBy(p => p.Solve.Time) : data.OrderBy(p => p.Solve.Elapsed);
            else
                data = (_sortType == ESortType.Date) ? data.OrderByDescending(p => p.Solve.Time) : data.OrderByDescending(p => p.Solve.Elapsed);
            _solves = data.ToArray();

            UpdateContent();
        }

        public void UpdateContent()
        {
            var children = _contentTransform.GetComponentsInChildren<UISolveEntry>();
            foreach (var child in children)
            {
                Destroy(child.gameObject);
            }

            _singleBestText.Text = "--";
            _averageText.Text = "--";
            _average5Text.Text = "--";
            _average10Text.Text = "--";
            _average3of5Text.Text = "--";
            _average10of12Text.Text = "--";

            var solves = _solves.OrderByDescending(p => p.Solve.Time).ToArray();
            float averageValue = _solves.Length == 0 ? 0.0f : (_solves.Select(p => p.Solve.Elapsed).Sum() / _solves.Length);
            var sorted = _solves.OrderBy(p => p.Solve.Elapsed).ToArray();
            SortedSolve worst = sorted.Length > 0 ? sorted[sorted.Length - 1] : null;
            SortedSolve best = sorted.Length > 0 ? sorted[0] : null;
            
            List<float> floats = new List<float>();
            for (int i = 0; i < solves.Length; i++)
            {
                floats.Add(solves[i].Solve.Elapsed);
                
                if (floats.Count == 5)
                {
                    _average5Text.Text = Solve.GetElapsedString(floats.Sum() / floats.Count);
                    _average3of5Text.Text = Solve.GetElapsedString(floats.OrderBy(p => p).Skip(1).Take(3).Sum() / 3.0f);
                }
                else if (floats.Count == 10)
                {
                    _average10Text.Text = Solve.GetElapsedString(floats.Sum() / floats.Count);
                }
                else if (floats.Count == 12)
                {
                    _average10of12Text.Text = Solve.GetElapsedString(floats.OrderBy(p => p).Skip(1).Take(10).Sum() / 10.0f);
                }
            }

            for (int i = 0; i < _solves.Length; i++)
            {
                SortedSolve solve = _solves[i];
                EIndicatorState state = EIndicatorState.None;
                if (solve == best) state = EIndicatorState.Best;
                else if (solve == worst) state = EIndicatorState.Worst;
                else if (solve.Solve.Elapsed < best.Solve.Elapsed + best.Solve.Elapsed * 0.1f) state = EIndicatorState.Good;
                else if (solve.Solve.Elapsed > worst.Solve.Elapsed - worst.Solve.Elapsed * 0.1f) state = EIndicatorState.Bad;
                
                UISolveEntry.CreateInstance(solve.Solve, _contentTransform, solve.Index, i, state);
            }

            if (floats.Count > 0)
            {
                _averageText.Text = Solve.GetElapsedString(floats.Sum() / floats.Count);
                _singleBestText.Text = Solve.GetElapsedString(floats.OrderBy(p => p).First());
            }

            _contentTransform.sizeDelta = new Vector2(_contentTransform.sizeDelta.x, 2 + 50 * _solves.Length);
        }

        public static UISolveHistory Inst { get; private set; }
    }
}