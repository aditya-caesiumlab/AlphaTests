using UnityEngine;

namespace Ramesh.gyroscopeScripts_1
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target; // The player or object the camera follows
        public Vector3 offset = new Vector3(0, 2f, -5f); // Offset from the target
        public float followSpeed = 5f; // How quickly the camera follows

        void LateUpdate()
        {
            // Calculate target position with offset
            Vector3 targetPosition = target.position + offset;

            // Smoothly move the camera to the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);

            // Always look at the target
            // transform.LookAt(target.position + Vector3.up * offset.y);
        }
    }
}