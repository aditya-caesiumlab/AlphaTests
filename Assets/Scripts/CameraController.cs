using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GyroscopePrototype
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _playerTarget; 
        [SerializeField] private Vector3 offset;   
        [SerializeField] private float _smoothSpeed = 0.125f; 

        private void LateUpdate()
        {
            Vector3 desiredPosition = _playerTarget.position + offset;

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(_playerTarget);
        }
    }
}