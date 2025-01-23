using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ramesh.gyroscopeScripts_2
{
    public class GyroscopeControl : MonoBehaviour
    {
        private Quaternion initialRotation;
        private Quaternion gyroNeutralRotation;
        private Quaternion gyroRotation = Quaternion.identity;
        public Quaternion inputRotation = Quaternion.identity;
        private bool gyroInitialized = false;

        public bool GyroEnabled;

        public bool Usegyro;


        [SerializeField] private float smoothing = 0.2f;
        public float inputSensitivity = 2.0f;
        [SerializeField] private float gyroInfluence = 0.8f;

        public Vector2 lastTouchPosition;
        public bool isTouching = false;


        public FixedJoystick joystick;


        public Vector2 MouseDelta;
        [SerializeField] private bool istouchpadactive;

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
            yield return new WaitForSeconds(1f);

            gyroNeutralRotation = GyroToUnity(Input.gyro.attitude);
        }

        private void Update()
        {

            if (Usegyro)
                ApplyGyroRotation();


            Quaternion combinedRotation = inputRotation * gyroRotation;
            float clampedX = Mathf.Clamp(combinedRotation.eulerAngles.x, -25, 25);
            combinedRotation = Quaternion.Euler(combinedRotation.eulerAngles.x, combinedRotation.eulerAngles.y, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, combinedRotation, smoothing);



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

                if (tiltX >= 0.03f || tiltY > -0.03f)
                {
                    if (Mathf.Abs(tiltX) > Mathf.Abs(tiltY)) // More tilt on X-axis
                    {
                        gyroRotation = Quaternion.Euler(gyroRotation.eulerAngles.x, gyroRotation.eulerAngles.y, 0); // Apply rotation only on Y-axis
                    }
                    else // More tilt on Y-axis
                    {
                        gyroRotation = Quaternion.Euler(gyroRotation.eulerAngles.x, gyroRotation.eulerAngles.y, 0); // Apply rotation only on X-axis
                    }

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
