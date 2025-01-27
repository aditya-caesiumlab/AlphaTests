using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace GyroscopePrototype
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _playerTarget;
        [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
        [SerializeField] private float _followSpeed = 0.125f;

        public Quaternion CurrentRotation { get; private set; } = Quaternion.identity;

        private void LateUpdate()
        {
            if (_playerTarget == null) return;

            FollowPlayer();
            transform.rotation = CurrentRotation;

           // GyroscopeFollow();
        }

        private void GyroscopeFollow()
        {
            Vector3 targetPosition = _playerTarget.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);

            // Optional: Adjust camera tilt using gyroscope
            if (Input.gyro.enabled)
            {
                Quaternion gyroRotation = Input.gyro.attitude;
                Vector3 gyroTilt = new Vector3(-gyroRotation.eulerAngles.x, 0, gyroRotation.eulerAngles.z);
                transform.rotation = Quaternion.Euler(gyroTilt + new Vector3(30, 0, 0));
            }
        }

        private void FollowPlayer()
        {
            Vector3 targetPosition = _playerTarget.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);
        }

        public void UpdateRotation(Quaternion rotation)
        {
            CurrentRotation = rotation;
        }
    }
}