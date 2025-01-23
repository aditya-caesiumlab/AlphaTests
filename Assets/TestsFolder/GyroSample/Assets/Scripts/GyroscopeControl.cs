using System.Collections;
using UnityEngine;

namespace Ramesh.gyroscopeScripts_1
{
    public class GyroscopeControl : MonoBehaviour
    {
        private Quaternion initialRotation;
        private Quaternion gyroNeutralRotation;
        private Quaternion gyroRotation = Quaternion.identity;
        private Quaternion inputRotation = Quaternion.identity; // This will store either mouse or joystick input or touch input
        private bool gyroInitialized = false;

        public bool GyroEnabled { get; private set; }

        // SETTINGS
        [SerializeField] private float smoothing = 0.2f;
        [SerializeField] private float inputSensitivity = 2.0f; // Sensitivity for input axis (horizontal/vertical)
        [SerializeField] private float gyroInfluence = 0.8f;

        private Vector2 lastTouchPosition;
        private bool isTouching = false;

        private void InitGyro()
        {
            if (!gyroInitialized)
            {
                Input.gyro.enabled = true;
                Input.gyro.updateInterval = 0.0167f;
                gyroInitialized = true;
            }
        }

        private IEnumerator Start()
        {
            if (SystemInfo.supportsGyroscope)
            {
                InitGyro();
                GyroEnabled = true;
            }
            else
            {
                GyroEnabled = false;
            }

            initialRotation = transform.rotation;
            yield return new WaitForSeconds(1f); // Allow time for the gyroscope to stabilize

            gyroNeutralRotation = GyroToUnity(Input.gyro.attitude);
        }

        private void Update()
        {
            HandleAxisInput(); // Handle mouse or joystick axis input for rotation
            HandleTouchInput(); // Handle touch input for rotation
            ApplyGyroRotation();

            // Combine input and gyro rotations
            Quaternion combinedRotation = inputRotation * gyroRotation;

            // Use Lerp for smooth blending
            transform.rotation = Quaternion.Lerp(transform.rotation, combinedRotation, smoothing);
        }

        private void HandleAxisInput()
        {
            // Use GetAxis to rotate based on mouse or joystick input
            float rotationX = Input.GetAxis("Mouse Y") * inputSensitivity; // Vertical movement of mouse or joystick
            float rotationY = Input.GetAxis("Mouse X") * inputSensitivity; // Horizontal movement of mouse or joystick

            // Update rotation based on axis input (Mouse Y and Mouse X are mapped by default)
            inputRotation *= Quaternion.Euler(rotationY, -rotationX, 0);
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    isTouching = true;
                    lastTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 delta = touch.position - lastTouchPosition;
                    lastTouchPosition = touch.position;

                    // Reverse the direction to correct rotation
                    float rotationX = -delta.y * inputSensitivity; // Vertical movement for touch
                    float rotationY = delta.x * inputSensitivity; // Horizontal movement for touch

                    inputRotation *= Quaternion.Euler(rotationX, rotationY, 0); // Adjust rotation
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isTouching = false;
                }
            }
        }

        private void ApplyGyroRotation()
        {
            if (GyroEnabled)
            {
                Quaternion currentGyro = GyroToUnity(Input.gyro.attitude);

                // Calculate the relative gyro rotation to the neutral position (gyroNeutralRotation)
                Quaternion relativeGyro = Quaternion.Inverse(gyroNeutralRotation) * currentGyro;

                // Apply rotation only around the correct axis based on tilt direction
                gyroRotation = Quaternion.Lerp(gyroRotation, relativeGyro, gyroInfluence);

                // Adjust rotation based on the tilt for X and Y axis.
                // For vertical tilt (up/down), we only apply rotation around the Y-axis (horizontal).
                // For horizontal tilt (left/right), we only apply rotation around the X-axis (vertical).
                float tiltX = relativeGyro.eulerAngles.x;
                float tiltY = relativeGyro.eulerAngles.y;

                if (Mathf.Abs(tiltX) > Mathf.Abs(tiltY)) // More tilt on X-axis
                {
                    // Apply rotation only on Y-axis (horizontal rotation), keep Z-axis fixed at 0
                    gyroRotation = Quaternion.Euler(gyroRotation.eulerAngles.x, tiltY, 0);
                }
                else // More tilt on Y-axis
                {
                    // Apply rotation only on X-axis (vertical rotation), keep Z-axis fixed at 0
                    gyroRotation = Quaternion.Euler(tiltX, gyroRotation.eulerAngles.y, 0);
                }
            }
        }

        private Quaternion GyroToUnity(Quaternion q)
        {
            return new Quaternion(q.x, q.y, -q.z, -q.w);
        }

        public void Recalibrate()
        {
            gyroNeutralRotation = GyroToUnity(Input.gyro.attitude);
        }
    }
}