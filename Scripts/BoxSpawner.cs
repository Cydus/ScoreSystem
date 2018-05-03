using UnityEngine;

/// <summary>
/// Spawns a matrix of GameObjects on Start
/// </summary>
namespace DBScoreSystem
{
    public class BoxSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab;


        [SerializeField]
        private int numX;

        [SerializeField]
        private int numY;

        public float spacingX;
        public float spacingY;

        Vector3 startPos;

        void Awake()
        {
            // calculate spawning startpoint
            float width = numX * spacingX;
            float height = numY * spacingY;

            startPos.x = transform.position.x - width / 2;
            startPos.y = transform.position.y - height / 2;
        }

        private void Start()
        {
            SpawnObjects();
        }

        private void SpawnObjects()
        {
            for (float x = 0; x < numX; x++)
            {
                for (float y = 0; y < numY; y++)
                {
                    Vector3 prefabPos = new Vector3();

                    prefabPos.x = startPos.x + (x * spacingX);
                    prefabPos.y = startPos.y + (y * spacingX);
                    prefabPos.z = transform.position.z;

                    Instantiate(prefab, prefabPos, Quaternion.identity);
                }
            }
        }
    }
}