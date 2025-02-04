using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GyroInputController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI normalizedDeltaValue;
    [SerializeField] TextMeshProUGUI GyroRawValues;

    Vector3 oldGyroValues;
    Vector3 rawGyroEulerAngles;
    public Vector3 GyRoDeltaValue;

    private void Start()
    {
        Input.gyro.enabled = true; 
    }

    void GetGyroDeltaValue()
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
    Vector3 NormalizeAngleDifference(Vector3 delta)
    {
        return new Vector3(
            NormalizeSingleAxis(delta.x),
            NormalizeSingleAxis(delta.y),
            NormalizeSingleAxis(delta.z)
        );
    }

    float NormalizeSingleAxis(float angle)
    {
        // Ensure the difference is between -180 and 180
        angle = (angle + 180) % 360 - 180;
        return angle;
    }

    // Update is called once per frame
    void Update()
    {
        GetGyroDeltaValue();
    }
}
