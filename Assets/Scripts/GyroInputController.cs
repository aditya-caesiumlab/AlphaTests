using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GyroscopePrototype
{
    public class GyroInputController : MonoBehaviour
    {
        [Header("Values Display")]
        [SerializeField] private TextMeshProUGUI normalizedDeltaValue;
        [SerializeField] private TextMeshProUGUI GyroRawValues;

        private Vector3 oldGyroValues;
        private Vector3 rawGyroEulerAngles;
        public Vector3 GyRoDeltaValue;

        #region Unity Methods
        private void Start()
        {
            Input.gyro.enabled = true;
        }

        private void Update()
        {
            GetGyroDeltaValue();
        }
        #endregion

        #region Gyro Input Methods
        private void GetGyroDeltaValue()
        {
            oldGyroValues = rawGyroEulerAngles;
            rawGyroEulerAngles = Input.gyro.attitude.eulerAngles;

            GyroRawValues.text = $" X : {rawGyroEulerAngles.x} , Y : {rawGyroEulerAngles.y.ToString()}, z :{rawGyroEulerAngles.z} ";

            GyRoDeltaValue = rawGyroEulerAngles - oldGyroValues;

            //GyRoDeltaValue = NormalizeAngleDifference(rawGyroEulerAngles - oldGyroValues);
            normalizedDeltaValue.text = $" X : {GyRoDeltaValue.x} , Y : {GyRoDeltaValue.y}, z :{GyRoDeltaValue.z} ";

            Debug.Log("Getting Gyro Input");
        }

        // Function to handle 360-degree wrapping issues 
        private Vector3 NormalizeAngleDifference(Vector3 delta)
        {
            return new Vector3(
                NormalizeSingleAxis(delta.x),
                NormalizeSingleAxis(delta.y),
                NormalizeSingleAxis(delta.z)
            );
        }

        private float NormalizeSingleAxis(float angle)
        {
            // Ensure the difference is between -180 and 180
            angle = (angle + 180) % 360 - 180;
            return angle;
        }

        #endregion
    }
}