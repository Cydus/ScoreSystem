using UnityEngine;

/// <summary>
/// Basic Audiomanager for the sample scene.
/// </summary>
namespace DBScoreSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioClip PositiveScoreSound;

        [SerializeField]
        AudioClip NegativeSound;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            ScoreInvoker.ResetMultiplierEvent += PlayNegative;
            ScoreInvoker.ScoreEvent += PlayPositive;
        }

        private void PlayPositive(int score)
        {
            audioSource.PlayOneShot(PositiveScoreSound);
        }

        private void PlayNegative()
        {
            audioSource.PlayOneShot(NegativeSound);
        }
    }
}