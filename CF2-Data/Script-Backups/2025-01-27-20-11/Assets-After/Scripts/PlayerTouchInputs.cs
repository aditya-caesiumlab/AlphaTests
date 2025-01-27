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

        private void Update()
        {
            HandleTouchInput();
        }

        private void HandleTouchInput()
        {
            if (ControlFreak2.CF2Input.touchCount == 1) 
            {
                ControlFreak2.InputRig.Touch touch = ControlFreak2.CF2Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    
                    _previousTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {                   
                    _touchDelta = touch.deltaPosition;
                   
                    _horizontalAngle += _touchDelta.x * _lookSensitivity;
                    _verticalAngle -= _touchDelta.y * _lookSensitivity;
                    
                    _verticalAngle = Mathf.Clamp(_verticalAngle, -_maxVerticalAngle, _maxVerticalAngle);
                    
                    Quaternion targetRotation = Quaternion.Euler(_verticalAngle, _horizontalAngle, 0);

                    _cameraFollow.UpdateRotation(targetRotation);
                }
            }
        }
    }
}