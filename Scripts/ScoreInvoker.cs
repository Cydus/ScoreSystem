using System;
using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Responsible for invoking the score and resetMultiplier events.
/// Also calculates and animates score text.
/// </summary>

namespace DBScoreSystem
{
    public class ScoreInvoker : MonoBehaviour
    {
        [SerializeField]
        private Transform playerTransform;

        [SerializeField]
        private AnimationCurve scaleCurve;

        [SerializeField]
        private AnimationCurve positionCurve;

        [SerializeField]
        private int scoreToLog;

        public static event Action<int> ScoreEvent = delegate { };
        public static event Action ResetMultiplierEvent = delegate { };

        private TextMeshPro text;

        [SerializeField]
        public Vector3 moveVector = new Vector3(0f, 1f, 0f);

        [SerializeField]
        private float scaleAmount = 2f;

        [SerializeField]
        private float scaleModifier = .5f;

        [SerializeField]
        private float scaleStep = .1f;

        private IEnumerator animateCoroutine;
        private Vector3 scaleVector;
        private Vector3 scaleStepVector;

        private Color initialColor;
        private Color targetColor;
        private Vector3 initalPosition;

        private void Awake()
        {
            playerTransform = Camera.main.transform;
            scaleVector = new Vector3(scaleAmount, scaleAmount, scaleAmount);
            scaleStepVector = new Vector3(scaleStep, scaleStep, scaleStep);

            text = GetComponent<TextMeshPro>();


            initialColor = new Color(text.color.r, text.color.g, text.color.b, text.color.a);
            targetColor = new Color(text.color.r, text.color.g, text.color.b, 0f);
            initalPosition = transform.position;
        }

        void OnEnable()
        {
            // make score text face the player
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }

        public void Begin()
        {
            ScoreEvent.Invoke(scoreToLog);
            text.text = (scoreToLog * ScoreLogger.Instance.ScoreMultiplier).ToString(); // TODO remove toString
            text.enabled = true;
            StartCoroutine(MoveAndScaleText());
        }

        public void RegisterMiss()
        {
            ResetMultiplierEvent.Invoke();
        }

        IEnumerator MoveAndScaleText()
        {
            float progress = 0;
            Vector3 initialPos = transform.position;

            // scales up score text based on distance so it's always appears the same size
            float distance = Vector3.Distance(transform.position, playerTransform.position) * .01f * scaleModifier;
            Vector3 initialScale = new Vector3(distance, distance, distance);

            while (progress < 1f)
            {
                progress += 2f * Time.deltaTime;

                float positionEv = positionCurve.Evaluate(progress) * distance;

                Vector3 mv = Vector3.up * positionEv * 5;
                transform.position = mv + initialPos;

                float scaleCurveEv = scaleCurve.Evaluate(progress) * distance;

                Vector3 s = new Vector3(scaleCurveEv, scaleCurveEv, scaleCurveEv);
                transform.localScale = s + initialScale;

                yield return null;
            }

            StartCoroutine(FadeOutText());
        }

        IEnumerator FadeOutText()
        {
            float progress = 0f;
            Vector3 initialScale = transform.localScale;

            while (progress < 1f)
            {
                progress += 4f * Time.deltaTime;
                text.color = Color.Lerp(initialColor, targetColor, progress);

                yield return null;
            }


            ResetGameObject();
        }

        private void ResetGameObject()
        {
            transform.localScale = Vector3.one;
            transform.position = initalPosition;
            text.color = initialColor;
            text.enabled = false;
            StopCoroutine(MoveAndScaleText());
            StopCoroutine(FadeOutText());
        }

        private void OnDisable()
        {
            StopCoroutine(MoveAndScaleText());
        }
    }
}