using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
        [Space]
        public GameObject CubeObject;
        public GameObject TextObject;
        public GameObject TimerObject;
        public GameObject TimeObject;
        [Space]
        public Text BigTimerText;
        public Text SmallTimerText;

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
        
        void Awake()
        {
            if (FindObjectsOfType<GameManager>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            I = this;
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

        void Start()
        {
            ScrambleCube();
        }

        void ScrambleCube()
        {
            Random.InitState(Random.Range(0xFFFFFF, 0x7FFFFFFF));
            
            Scrambler scr = new Scrambler(25);

            ScrambleText.Text = scr.Sequence;
            
            StartCoroutine(RunScrambleAnimation(scr.Sequence));
        }

        private IEnumerator RunScrambleAnimation(string[] sequence)
        {
            Face.ResetAll();

            yield return new WaitForSeconds(ScrambleDelay);

            CubeMesh.material = CubeMaterialScramble;
            
            for (int i = 0; i < sequence.Length; i++)
            {
                string notation = sequence[i];
                TurnFaceFromNotation(notation);

                ScrambleText.Highlight = i;

                yield return new WaitForSeconds(ScrambleSpeed);
            }

            ScrambleText.Highlight = -1;
            CubeMesh.material = CubeMaterialNormal;
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
        }

        private void CheckInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bool state = _solveActive;
                SetSolving(!_solveActive);
                
                if (state) StopTimer();
                else StartTimer();
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
            SmallTimerText.text = display;
            _keyTime = Time.time;
            SmallTimerText.color = Color.green;
            ScrambleCube();
        }

        private string GetElapsed()
        {
            float timeNow = Time.time;
            _timeElapsed = timeNow - _timerStart;

            int seconds = (int)_timeElapsed;
            int ms = (int)(_timeElapsed * 1000.0f) % 1000;

            int minutes = seconds / 60;
            seconds %= 60;

            return $"{minutes}:{seconds.ToString().PadLeft(2, '0')}.{ms.ToString().PadLeft(3, '0')}";
        }

        public void SetSolving(bool state)
        {
            if (_solveActive != state)
            {
                TextObject.SetActive(!state);
                TimerObject.SetActive(state);
                TimeObject.SetActive(!state);
                CubeObject.SetActive(!state);
            }

            _solveActive = state;
        }

        public static GameManager I { get; private set; }
    }
}