using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace GyroscopePrototype
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _playerTarget; 
        [SerializeField] private Vector3 offset = new Vector3(0,5,-10);   
        [SerializeField] private float _followSpeed = 0.125f; 

        private void LateUpdate()
        {
            if (_playerTarget == null) return;

            Vector3 targetPosition = _playerTarget.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);

            // Optional: Adjust camera tilt using gyroscope
            if (Input.gyro.enabled)
            {
                Quaternion gyroRotation = Input.gyro.attitude;
                transform.rotation = Quaternion.Euler(gyroRotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            }
        }
    }
}