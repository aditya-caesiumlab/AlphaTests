using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace GyroscopePrototype
{
    public class CameraHandler : MonoBehaviour
    {
        [Header("Target and Offset")]
        [SerializeField] private Transform _playerTarget;
        [SerializeField] private Vector3 _offset;

        [Header("Follow Settings")]
        [SerializeField] private float _followSpeed = 0.125f;

        [Header("Rotation Settings")]
        [SerializeField] private Vector3 _initialRotation;

        private Quaternion _currentRotation;

        private void Start()
        {
            // Initialize rotation with the Inspector-defined value
            _currentRotation = Quaternion.Euler(_initialRotation);
            transform.rotation = _currentRotation;
            if (_playerTarget != null)
            {
                transform.position = _playerTarget.position + _offset;
            }
        }

        private void LateUpdate()
        {
            if (_playerTarget == null) return;

            FollowPlayer();
            transform.rotation = _currentRotation;

            // GyroscopeFollow();
        }

        private void GyroscopeFollow()
        {
            Vector3 targetPosition = _playerTarget.position + _offset;
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
            Vector3 targetPosition = _playerTarget.position + _offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);
            //targetPosition = Vector3.MoveTowards(transform.position, targetPosition,_followSpeed * Time.deltaTime);
        }

        public void UpdateRotation(Quaternion rotation)
        {
            _currentRotation = rotation;
        }
    }
}