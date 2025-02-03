using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        
        [Header("Gyroscope Values Display")]
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private TextMeshProUGUI rawGyroDataText;
        [SerializeField] private TextMeshProUGUI appliedRotationText;
        [SerializeField] private TextMeshProUGUI cameraRotationText;

        [Header("Gyro Settings")]
        [SerializeField] private float defaultSensitivity = 2f;
        [SerializeField] private float defaultRotationLimit = 45f; 

        internal bool isGyroEnabled = false;
        private Quaternion initialGyroRotation;

        public Vector2 GyroRotation { get; private set; } 

        private void Start()
        {
            InitialGyroCheck();

            InitializeGyroValues();

            UpdateGyroStatus();
        }

        private void Update()
        {
            if (isGyroEnabled)
            {
                Vector2 rotation = GetGyroInput();
                GyroRotation = rotation;

                if (cameraTransform != null)
                {
                    ApplyGyroRotation(rotation);
                }
            }

            UpdateGyroDebugUI();
        }

        private void InitializeGyroValues()
        {
            // Initialize UI values
            if (gyroToggle != null)
            {
                gyroToggle.onValueChanged.AddListener(ToggleGyro);
                gyroToggle.isOn = false; // Default state
            }
            if (sensitivitySlider != null)
            {
                sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
                sensitivitySlider.value = defaultSensitivity;
            }
            if (rotationLimitSlider != null)
            {
                rotationLimitSlider.onValueChanged.AddListener(SetRotationLimit);
                rotationLimitSlider.value = defaultRotationLimit;
            }
        }

        private void InitialGyroCheck()
        {
            if (!SystemInfo.supportsGyroscope)
            {
                Debug.LogWarning("Gyroscope not supported on this device.");
                if (statusText) statusText.text = "Gyroscope: Not Supported";
                return;
            }

            Input.gyro.enabled = true;
            initialGyroRotation = Input.gyro.attitude;
        }

        private Vector2 GetGyroInput()
        {
            Quaternion deviceRotation = Input.gyro.attitude;
            deviceRotation = Quaternion.Euler(90f, 0f, 0f) * new Quaternion(-deviceRotation.x, -deviceRotation.y, deviceRotation.z, deviceRotation.w);

            Quaternion deltaRotation = deviceRotation * Quaternion.Inverse(initialGyroRotation);
            Vector3 eulerRotation = deltaRotation.eulerAngles;

            // Convert to range (-180, 180)
            if (eulerRotation.x > 180) eulerRotation.x -= 360;
            if (eulerRotation.y > 180) eulerRotation.y -= 360;

            // Clamp within rotation limits
            eulerRotation.x = Mathf.Clamp(eulerRotation.x, -rotationLimitSlider.value, rotationLimitSlider.value);
            eulerRotation.y = Mathf.Clamp(eulerRotation.y, -rotationLimitSlider.value, rotationLimitSlider.value);

            return new Vector2(eulerRotation.x, eulerRotation.y);
        }

        private void ApplyGyroRotation(Vector2 rotation)
        {
            Quaternion targetRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, targetRotation, Time.deltaTime * sensitivitySlider.value);
        }

        #region Gyro UI Methods
        public void ToggleGyro(bool value)
        {
            isGyroEnabled = value;
            if (isGyroEnabled)
            {
                initialGyroRotation = Input.gyro.attitude; 
            }
            UpdateGyroStatus();
        }

        public void SetSensitivity(float value)
        {
            Debug.Log($"Gyro Sensitivity set to {value}");
        }

        public void SetRotationLimit(float value)
        {
            Debug.Log($"Gyro Rotation Limit set to {value}");
        }

        private void UpdateGyroStatus()
        {
            if (statusText)
            {
                if (SystemInfo.supportsGyroscope)
                {
                    statusText.text = isGyroEnabled ? "Gyroscope: Enabled" : "Gyroscope: Disabled";
                }
                else
                {
                    statusText.text = "Gyroscope: Not Supported";
                }
            }
        }

        private void UpdateGyroDebugUI()
        {
            if (statusText != null)
            {
                Vector2 gyroRotation = GyroRotation;
                Vector3 cameraEuler = cameraTransform.localRotation.eulerAngles;
                Vector3 rawGyroEulerAngles = Input.gyro.attitude.eulerAngles;

                rawGyroDataText.text = $"Raw Gyro: X={rawGyroEulerAngles.x:F2}, Y={rawGyroEulerAngles.y:F2}";
                appliedRotationText.text = $"Applied Rotation: X={gyroRotation.x:F2}, Y={gyroRotation.y:F2}";
                cameraRotationText.text = $"Camera Rotation: X={cameraEuler.x:F2}, Y={cameraEuler.y:F2}, Z={cameraEuler.z:F2}";
            }
        }
        #endregion
    }
}
