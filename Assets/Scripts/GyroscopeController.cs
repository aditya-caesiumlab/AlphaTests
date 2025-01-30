using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GyroscopePrototype
{
    public class GyroscopeController : MonoBehaviour
    {
        [Header("Camera Reference")]
        [SerializeField] private Transform cameraTransform;

        [Header("UI Controls")]
        [SerializeField] private Toggle gyroToggle;
        [SerializeField] private Slider sensitivitySlider;
        [SerializeField] private Slider rotationLimitSlider;

        [Header("Gyro Settings")]
        [SerializeField] private float defaultSensitivity = 2f;
        [SerializeField] private float defaultRotationLimit = 45f; // Max rotation angle

        private bool isGyroEnabled = false;
        private Quaternion gyroInitialRotation;

        private void Start()
        {
            if (!SystemInfo.supportsGyroscope)
            {
                Debug.LogWarning("Gyroscope not supported on this device.");
                return;
            }

            Input.gyro.enabled = true;
            gyroInitialRotation = Input.gyro.attitude;

            // Initialize UI values
            if (gyroToggle != null) gyroToggle.onValueChanged.AddListener(ToggleGyro);
            if (sensitivitySlider != null) sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
            if (rotationLimitSlider != null) rotationLimitSlider.onValueChanged.AddListener(SetRotationLimit);

            sensitivitySlider.value = defaultSensitivity;
            rotationLimitSlider.value = defaultRotationLimit;
        }

        private void Update()
        {
            if (isGyroEnabled)
            {
                ApplyGyroRotation();
            }
        }

        private void ApplyGyroRotation()
        {
            Quaternion deviceRotation = Input.gyro.attitude;
            deviceRotation = Quaternion.Euler(90f, 0f, 0f) * new Quaternion(-deviceRotation.x, -deviceRotation.y, deviceRotation.z, deviceRotation.w);

            Quaternion deltaRotation = deviceRotation * Quaternion.Inverse(gyroInitialRotation);
            Vector3 eulerRotation = deltaRotation.eulerAngles;

            // Convert to range (-180, 180)
            if (eulerRotation.x > 180) eulerRotation.x -= 360;
            if (eulerRotation.y > 180) eulerRotation.y -= 360;

            // Clamp within rotation limits
            eulerRotation.x = Mathf.Clamp(eulerRotation.x, -rotationLimitSlider.value, rotationLimitSlider.value);
            eulerRotation.y = Mathf.Clamp(eulerRotation.y, -rotationLimitSlider.value, rotationLimitSlider.value);

            // Apply smooth rotation to camera
            cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, Quaternion.Euler(eulerRotation), Time.deltaTime * sensitivitySlider.value);
        }

        public void ToggleGyro(bool value)
        {
            isGyroEnabled = value;
            if (isGyroEnabled)
            {
                gyroInitialRotation = Input.gyro.attitude; // Reset base rotation
            }
        }

        public void SetSensitivity(float value)
        {
            Debug.Log($"Gyro Sensitivity set to {value}");
        }

        public void SetRotationLimit(float value)
        {
            Debug.Log($"Gyro Rotation Limit set to {value}");
        }
    }
}
