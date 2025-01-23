using System.Collections;
using UnityEngine;

namespace Ramesh.gyroscopeScripts_1
{
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float movementSpeed = 5f;
        public float rotationSpeed = 10f;

        private Vector3 moveDirection;
        private Rigidbody rb;

        [Header("Camera Settings")]
        [SerializeField] private Transform _camera;
        [SerializeField] private Transform playerBody; // Reference to the player body

        void Start()
        {
            rb = GetComponent<Rigidbody>(); // Initialize Rigidbody
        }

        void Update()
        {
            // Character movement input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

            if (moveDirection.magnitude >= 0.1f)
            {
                // Move the player relative to the camera direction (forward/backward/left/right)
                Vector3 moveDir = Quaternion.Euler(0, _camera.eulerAngles.y, 0) * moveDirection;

                // Apply movement
                rb.MovePosition(rb.position + moveDir * movementSpeed * Time.deltaTime);

                // Rotate player to face movement direction
                float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                playerBody.rotation = Quaternion.Slerp(playerBody.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
