using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GyroscopePrototype
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 5f;

        [SerializeField] private VariableJoystick _variableJoystick;

        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            if(SystemInfo.supportsGyroscope)
            {
                Input.gyro.enabled = true;
                Debug.Log("Gyroscope enabled in PlayerMovement");
            }
        }

        private void FixedUpdate()
        {
            PlayerMove();
        }

        private void PlayerMove()
        {
            Vector3 movement = Vector3.zero;

            // Joystick movement
            float horizontal = _variableJoystick.Horizontal;
            float vertical = _variableJoystick.Vertical;
            movement += new Vector3(horizontal, 0, vertical).normalized;

            // Apply movement
            Vector3 moveDirection = transform.TransformDirection(movement) * _moveSpeed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + moveDirection);

            // Smooth rotation when moving
            if (movement.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                Quaternion smoothedRotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.fixedDeltaTime
                );
                _rigidbody.MoveRotation(smoothedRotation);
            }
        }
    }
}