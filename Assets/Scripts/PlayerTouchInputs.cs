using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GyroscopePrototype
{
    public class PlayerTouchInputs : MonoBehaviour
    {
        [SerializeField] private CameraController _cameraFollow; 
        [SerializeField] private float _lookSensitivity = 0.1f; 
        [SerializeField] private float _maxVerticalAngle = 80f; 

        private float _verticalAngle = 0f; 
        private float _horizontalAngle = 0f; 
        private Vector2 _previousTouchPosition; 
        private Vector2 _touchDelta;

        public float XValue;
        public float YValue;

        private void Update()
        {
            HandleTouchInput();

            /*XValue = ControlFreak2.CF2Input.GetAxis("Mouse X");
            YValue = ControlFreak2.CF2Input.GetAxis("Mouse Y");*/
        }

        private void HandleTouchInput()
        {
            float xValue = ControlFreak2.CF2Input.GetAxis("Mouse X"); 
            float yValue = ControlFreak2.CF2Input.GetAxis("Mouse Y"); 

            //Debug.Log($"Mouse X: {xValue}, Mouse Y: {yValue}");

            _horizontalAngle += xValue * _lookSensitivity;
            _verticalAngle -= yValue * _lookSensitivity;

            _verticalAngle = Mathf.Clamp(_verticalAngle, -_maxVerticalAngle, _maxVerticalAngle);

            Quaternion targetRotation = Quaternion.Euler(_verticalAngle, _horizontalAngle, 0);

            _cameraFollow.UpdateRotation(targetRotation);
        }
    }
}