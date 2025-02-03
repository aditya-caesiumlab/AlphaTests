using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GyroscopePrototype
{
    public class GyroscopeHandler : MonoBehaviour
    {
        [Header("Gyroscope Sensitivity Settings")]
        [SerializeField, Range(0.1f, 2f)] private float gyroSensitivity = 1f; 
        [SerializeField, Range(0.01f, 0.5f)] private float filterStrength = 0.1f; 
        [SerializeField, Range(10f, 90f)] private float maxTiltAngle = 30f; 
        [SerializeField, Range(0f, 0.5f)] private float deadZone = 0.1f; 

        [Header("UI Controls")]
        [SerializeField] private Slider sensitivitySlider;
        [SerializeField] private Slider filterSlider;
        [SerializeField] private Slider maxTiltSlider;
        [SerializeField] private Slider deadZoneSlider;

        [Header("Text Displays")]
        [SerializeField] private TextMeshProUGUI sensitivityText; 
        [SerializeField] private TextMeshProUGUI filterText;      
        [SerializeField] private TextMeshProUGUI maxTiltText;     
        [SerializeField] private TextMeshProUGUI deadZoneText;    

        private Vector3 smoothedGyroTilt; 

        private void Start()
        {
            if (SystemInfo.supportsGyroscope)
            {
                Input.gyro.enabled = true;
                Debug.Log("Gyroscope Enabled");
            }
            else
            {
                Debug.LogWarning("Gyroscope not supported on this device.");
            }
        }

        private void Update()
        {
            GyroscopeBehaviour();
        }

        private void GyroscopeBehaviour()
        {
            if (!Input.gyro.enabled) return;

            // Gyroscope tilt processing
            Quaternion gyroRotation = Input.gyro.attitude;
            Vector3 gyroTilt = new Vector3(-gyroRotation.eulerAngles.x, 0, gyroRotation.eulerAngles.z);

            // Apply dead zone
            if (gyroTilt.magnitude < deadZone)
            {
                gyroTilt = Vector3.zero;
            }

            // Clamp tilt angle
            gyroTilt.x = Mathf.Clamp(gyroTilt.x, -maxTiltAngle, maxTiltAngle);
            gyroTilt.z = Mathf.Clamp(gyroTilt.z, -maxTiltAngle, maxTiltAngle);

            // Apply low-pass filter for smoothness
            smoothedGyroTilt = Vector3.Lerp(smoothedGyroTilt, gyroTilt, filterStrength);

            // Apply sensitivity
            Vector3 adjustedTilt = smoothedGyroTilt * gyroSensitivity;

            // Debugging - Display gyroscope tilt values
            Debug.Log($"Gyro Tilt: {adjustedTilt}");
        }
    }
}
