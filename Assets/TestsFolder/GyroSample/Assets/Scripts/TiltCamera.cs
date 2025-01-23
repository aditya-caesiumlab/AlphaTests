using UnityEngine;

namespace Ramesh.gyroscopeScripts_1
{
    public class TiltCamera : MonoBehaviour
    {
        private bool gyroEnabled;

        [Header("Rotation Limits")]
        public float minPitch = -30f; // Minimum X rotation angle
        public float maxPitch = 30f;  // Maximum X rotation angle
        public float minYaw = -60f;   // Minimum Y rotation angle
        public float maxYaw = 60f;    // Maximum Y rotation angle

        [Header("Smoothing Settings")]
        public float smoothingSpeed = 5f; // Higher value = faster smoothing

        [Header("Dead Zone Settings")]
        public float deadZoneThreshold = 1.5f; // Ignore tilts smaller than this angle

        private Quaternion targetRotation;

        void Start()
        {
            // Attempt to enable gyroscope
            gyroEnabled = SystemInfo.supportsGyroscope;
            Input.compass.enabled = true;
            targetRotation = transform.localRotation;
        }

        void Update()
        {
            if (gyroEnabled && Input.gyro.enabled)
            {
                Quaternion gyroRotation = GyroToUnity(Input.gyro.attitude);
                Vector3 constrainedEulerAngles = ConstrainEulerAngles(gyroRotation.eulerAngles);

                if (IsSignificantTilt(constrainedEulerAngles))
                {
                    targetRotation = Quaternion.Euler(constrainedEulerAngles);
                }
            }
            else
            {
                Vector3 gravity = Input.acceleration.normalized;
                Quaternion fallbackRotation = Quaternion.LookRotation(
                    new Vector3(gravity.x, gravity.y, -gravity.z), Vector3.up
                );
                Vector3 constrainedEulerAngles = ConstrainEulerAngles(fallbackRotation.eulerAngles);

                if (IsSignificantTilt(constrainedEulerAngles))
                {
                    targetRotation = Quaternion.Euler(constrainedEulerAngles);
                }
            }

            // Smoothly interpolate the camera rotation
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smoothingSpeed);
        }

        private Quaternion GyroToUnity(Quaternion q)
        {
            return new Quaternion(q.x, q.y, -q.z, -q.w);
        }

        private Vector3 ConstrainEulerAngles(Vector3 eulerAngles)
        {
            eulerAngles.x = NormalizeAngle(eulerAngles.x);
            eulerAngles.y = NormalizeAngle(eulerAngles.y);

            eulerAngles.x = Mathf.Clamp(eulerAngles.x, minPitch, maxPitch);
            eulerAngles.y = Mathf.Clamp(eulerAngles.y, minYaw, maxYaw);

            return eulerAngles;
        }

        private float NormalizeAngle(float angle)
        {
            angle = angle % 360f;
            if (angle > 180f) angle -= 360f;
            else if (angle < -180f) angle += 360f;
            return angle;
        }

        // Checks if the tilt change is significant enough
        private bool IsSignificantTilt(Vector3 eulerAngles)
        {
            Vector3 currentEuler = transform.localRotation.eulerAngles;
            float deltaX = Mathf.Abs(NormalizeAngle(eulerAngles.x) - NormalizeAngle(currentEuler.x));
            float deltaY = Mathf.Abs(NormalizeAngle(eulerAngles.y) - NormalizeAngle(currentEuler.y));

            return deltaX > deadZoneThreshold || deltaY > deadZoneThreshold;
        }
    }
}