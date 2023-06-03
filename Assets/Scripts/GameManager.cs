using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Kubewatch.Data;
using Kubewatch.UI;
using Kubewatch.Enums;
using Kubewatch.Options;

namespace Kubewatch
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        public GameObject StickerPrefab;
        [Space]
        public MeshRenderer CubeMesh;
        public Material CubeMaterialNormal;
        public Material CubeMaterialScramble;
        public SpriteRenderer LogoStickerSprite;
        [Space]
        public GameObject CubeObject;
        public GameObject[] TimerVisibleObjects;
        public GameObject[] TimerHiddenObjects;
        [Space]
        public Text BigTimerText;
        public Text SmallTimerText;
        [Space]
        public GameObject SolveHistoryEntryPrefab;
        [Space]
        public UITextFormat CubeFlipText;

        [Header("Faces")]
        public Face FrontFace;
        public Face BackFace;
        public Face LeftFace;
        public Face RightFace;
        public Face TopFace;
        public Face BottomFace;

        [Header("Properties")]
        public float ScrambleSpeed = 0.15f;
        public float ScrambleDelay = 0.8f;
        
        private bool _solveActive = false;
        private float _timerStart = -1.0f;
        private float _timeElapsed = -1.0f;

        private EFlipMode _flipMode = EFlipMode.None;

        private Scrambler _scrambler = null;

        private bool _scrambling = false;
        
        void Awake()
        {
            if (FindObjectsOfType<GameManager>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            I = this;

            _flipMode = Prefs.FlipMode;
        }

        public Face GetFace(string name)
        {
            switch (name)
            {
                case "R": return RightFace;
                case "L": return LeftFace;
                case "U": return TopFace;
                case "D": return BottomFace;
                case "B": return BackFace;
                default: return FrontFace;
            }
        }

        public void TurnFaceFromNotation(string notation)
        {
            Face face = GetFace(notation.Substring(0, 1));
            string suffix = notation.Remove(0, 1);
            switch (suffix)
            {
                case "2":
                    face.Turn(); face.Turn();
                    break;
                case "'":
                    face.Turn(false);
                    break;
                default:
                    face.Turn();
                    break;
            }
        }

        public void FlipCubeColors()
        {
            EStickerColor facing = EStickerColor.Green;

            TopFace.FlipColors(facing);
            LeftFace.FlipColors(facing);
            RightFace.FlipColors(facing);
            BottomFace.FlipColors(facing);
            BackFace.FlipColors(facing);
            FrontFace.FlipColors(facing);
        }

        void Start()
        {
            ScrambleCube();
            UpdateFlipMode(EFlipMode.None);
        }

        void ScrambleCube()
        {
            Random.InitState((int)(System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() & 0x7FFFFFFF));
            Random.InitState(Random.Range(0xFFFFFF, 0x7FFFFFFF));
            
            _scrambler = new Scrambler(25);

            ScrambleText.Text = _scrambler.Sequence;
            
            _scrambling = false;
            StartCoroutine(RunScrambleAnimation(_scrambler.Sequence));
        }

        private IEnumerator RunScrambleAnimation(string[] sequence)
        {
            Face.ResetAll();

            yield return new WaitForSeconds(ScrambleDelay);

            CubeMesh.material = CubeMaterialScramble;

            _scrambling = true;            
            for (int i = 0; i < sequence.Length && _scrambling; i++)
            {
                string notation = sequence[i];
                TurnFaceFromNotation(notation);

                ScrambleText.Highlight = i;

                yield return new WaitForSeconds(ScrambleSpeed);
            }

            ScrambleText.Highlight = -1;
            CubeMesh.material = CubeMaterialNormal;

            _scrambling = false;
        }

        private float _keyTime = -1.0f;

        void Update()
        {
            if (Time.time - _keyTime >= 2.0f || _solveActive)
            {
                SmallTimerText.color = Color.white;
                CheckInput();
            }

            if (_solveActive)
            {
                BigTimerText.text = GetElapsed();
            }
            else
            {
                LogoStickerSprite.enabled = _flipMode != EFlipMode.Color;
            }
        }

        private void UpdateFlipMode(EFlipMode prevMode)
        {
            CubeObject.transform.rotation = Quaternion.AngleAxis(45.0f, Vector3.up);
            if (_flipMode == EFlipMode.Orientation) {
                CubeObject.transform.rotation *= Quaternion.AngleAxis(180.0f, Vector3.forward);
            }

            if (_flipMode == EFlipMode.Color || prevMode == EFlipMode.Color)
            {
                FlipCubeColors();
            }

            CubeFlipText.Text = _flipMode.GetDisplayString();
        }

        private bool TimerKeyDown => _solveActive && Input.anyKeyDown || Input.GetKeyDown(KeyCode.Space);

        private void CheckInput()
        {
            if (TimerKeyDown)
            {
                EFlipMode flipMode = _flipMode;
                _flipMode = EFlipMode.None;
                UpdateFlipMode(EFlipMode.None);

                bool state = _solveActive;
                SetSolving(!_solveActive);
                
                if (state) StopTimer();
                else StartTimer();

                _flipMode = flipMode;
                UpdateFlipMode(EFlipMode.None);
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                EFlipMode flipMode = _flipMode;
                _flipMode = (EFlipMode)(((int)_flipMode + 1) % 3);
                Prefs.FlipMode = _flipMode;

                UpdateFlipMode(flipMode);
            }
        }

        private void StartTimer()
        {
            _timerStart = Time.time;
            BigTimerText.text = "0:00.000";
        }

        private void StopTimer()
        {
            string display = GetElapsed();
            var solve = new Solve(_scrambler.Sequence, _timeElapsed);
            
            SmallTimerText.text = display;
            _keyTime = Time.time;
            SmallTimerText.color = Color.green;
            ScrambleCube();

            SolveHistory.AddSolve(solve, () => UISolveHistory.Inst.Reload());
        }

        public void OnSkipScramble()
        {
            ScrambleCube();
        }

        public void Recall(Solve solve)
        {
            if (_solveActive) return;

            SetSolving(false);
            SmallTimerText.text = solve.ElapsedString;
        }

        private string GetElapsed()
        {
            float timeNow = Time.time;
            _timeElapsed = timeNow - _timerStart;

            return Solve.GetElapsedString(_timeElapsed);
        }

        public void SetSolving(bool state)
        {
            foreach (var obj in TimerVisibleObjects)
            {
                if (obj != null) obj.SetActive(state);
            }

            foreach (var obj in TimerHiddenObjects)
            {
                if (obj != null) obj.SetActive(!state);
            }

            _solveActive = state;
        }

        public static GameManager I { get; private set; }
    }
}