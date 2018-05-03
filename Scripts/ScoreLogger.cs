using UnityEngine;
using TMPro;

/// <summary>
/// Provides the basic operation of a visual score logger with hashmarks
/// </summary>

namespace DBScoreSystem
{
    public class ScoreLogger : MonoBehaviour
    {
        [SerializeField]
        GameObject[] HashMarks;

        [SerializeField]
        TextMeshPro ScoreText;

        [SerializeField]
        TextMeshPro MultiplierText;

        public static ScoreLogger Instance;

        private int _score = 0;
        private int _scoreMultiplier = 1;

        private int HashMarkIndex = 0;
        private int HashMarkCount;

        private int length;

        private char[] myCharArray = new char[30]; // should be enough capacity for anyone

        // cache references to chars to minimise GCAlloc
        private char[] charReference = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            HashMarkCount = HashMarks.Length;

            ScoreInvoker.ResetMultiplierEvent += ResetMultiplier;
            ScoreInvoker.ScoreEvent += AddScore;
        }

        private void Start()
        {
            ResetHashMarks();

            IntToCharArray(_score, myCharArray, out length);
            ScoreText.SetCharArray(myCharArray, 0, length);

            IntToCharArray(_scoreMultiplier, myCharArray, out length);
            MultiplierText.SetCharArray(myCharArray, 0, length);
        }

        public int Score
        {
            get
            {
                return _score;
            }

            set
            {
                if (value >= 0)
                {

                    if ((HashMarkIndex + 1) > HashMarkCount)
                    {
                        ResetHashMarks();
                        ScoreMultiplier++;
                    }
                    else
                    {
                        HashMarks[HashMarkIndex].SetActive(true);
                        HashMarkIndex++;
                    }

                    _score = value;
                    IntToCharArray(_score, myCharArray, out length);
                    ScoreText.SetCharArray(myCharArray, 0, length);
                }
            }
        }

        public int ScoreMultiplier
        {
            get
            {
                return _scoreMultiplier;
            }

            set
            {
                if (value > 0)
                {
                    _scoreMultiplier = value;
                    IntToCharArray(_scoreMultiplier, myCharArray, out length);
                    MultiplierText.SetCharArray(myCharArray, 0, length);
                }
            }
        }

        void AddScore(int scoreToAdd)
        {
            Score += scoreToAdd * _scoreMultiplier;
        }

        // converts integer into a char array.
        private void IntToCharArray(int p_int, in char[] charArray, out int length)
        {
            int index = Mathf.FloorToInt(Mathf.Log10(p_int));
            length = index + 1;

            // if zero set to char zero
            if (p_int <= 0)
            {
                charArray[0] = charReference[0];
                length = 1;
            }
            else
                for (; p_int != 0; p_int /= 10)
                {
                    charArray[index] = charReference[(p_int % 10)];
                    index--;
                }
        }

        private void ResetMultiplier()
        {
            ScoreMultiplier = 1;
            ResetHashMarks();
        }

        private void ResetHashMarks()
        {
            for (int i = 0; i < HashMarks.Length; i++)
                HashMarks[i].SetActive(false);
            HashMarkIndex = 0;
        }
    }
}