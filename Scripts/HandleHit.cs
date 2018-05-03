using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Exposes the ability for raycasts to do something when they hit an object
/// </summary>
namespace DBScoreSystem
{
    public class HandleHit : MonoBehaviour
    {
        public ScoreInvoker si;
        public float minRespawnTime = 1f;
        public float maxRespawnTime = 5f;

        public Material enabledMat;
        public Material disabledMat;

        private MeshRenderer meshRenderer;

        private bool eligibleForScore = true;

        private void Awake()
        {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }

        // public method to be called by raycasts
        public void DoHit()
        {
            if (!eligibleForScore)
            {
                si.RegisterMiss();
            }
            else
            {
                si.Begin();
                meshRenderer.material = disabledMat;
                eligibleForScore = false;
                StartCoroutine(Respawn());
            }
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(Random.Range(minRespawnTime, maxRespawnTime));
            meshRenderer.material = enabledMat;
            eligibleForScore = true;
        }
    }
}