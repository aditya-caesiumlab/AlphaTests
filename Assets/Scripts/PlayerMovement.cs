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
            PlayerJoystickMovement();
        }

        private void PlayerJoystickMovement()
        {
            Vector3 movement = Vector3.zero;

            /* float horizontal = _variableJoystick.Horizontal;
             float vertical = _variableJoystick.Vertical;*/

            float horizontal = ControlFreak2.CF2Input.GetAxis("Horizontal");   
            float vertical = ControlFreak2.CF2Input.GetAxis("Vertical");

            movement += new Vector3(horizontal, 0, vertical).normalized;

            Vector3 moveDirection = transform.TransformDirection(movement) * _moveSpeed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + moveDirection);

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