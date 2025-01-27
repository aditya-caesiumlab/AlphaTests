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
            float xValue = ControlFreak2.CF2Input.GetAxis("Mouse X"); // Horizontal movement
            float yValue = ControlFreak2.CF2Input.GetAxis("Mouse Y"); // Vertical movement

            // Debugging the input values
            Debug.Log($"Mouse X: {xValue}, Mouse Y: {yValue}");

            // Apply sensitivity and calculate new angles
            _horizontalAngle += xValue * _lookSensitivity;
            _verticalAngle -= yValue * _lookSensitivity;

            // Clamp the vertical angle to prevent flipping
            _verticalAngle = Mathf.Clamp(_verticalAngle, -_maxVerticalAngle, _maxVerticalAngle);

            // Create a target rotation based on the updated angles
            Quaternion targetRotation = Quaternion.Euler(_verticalAngle, _horizontalAngle, 0);

            // Update the camera's rotation
            _cameraFollow.UpdateRotation(targetRotation);
        }
    }
}