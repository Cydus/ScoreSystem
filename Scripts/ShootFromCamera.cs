using UnityEngine;

/// <summary>
/// Basic script to shoot hittable objects in the scene
/// </summary>
namespace DBScoreSystem
{
    public class ShootFromCamera : MonoBehaviour
    {
        [SerializeField]
        private float rayLength = 100f;

        private Camera cameraRef;
        private Vector3 mousePos;
        private Ray ray;

        private bool allreadyShot = false;

        private void Awake()
        {
            cameraRef = Camera.main;
        }

        private void Update()
        {
            mousePos = Input.mousePosition;
            ray = cameraRef.ScreenPointToRay(mousePos);

            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.yellow);

            HandleInput();
        }

        private void Shoot()
        {
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo))
            {

                if (hitInfo.collider.GetComponentInChildren<HandleHit>() == null)
                    return;

                HandleHit hitHandler = hitInfo.collider.GetComponentInChildren<HandleHit>();
                Debug.Log("shoot");

                hitHandler.DoHit();
            }
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0) && !allreadyShot)
            {
                Shoot();
                allreadyShot = true;
            }

            if (Input.GetMouseButtonUp(0))
                allreadyShot = false;
        }
    }
}