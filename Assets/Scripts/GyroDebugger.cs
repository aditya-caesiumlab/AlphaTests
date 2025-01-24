using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GyroDebugger : MonoBehaviour
{
    [SerializeField] private Transform player; 

    private Vector3 gyroGravity; 
    private Quaternion gyroAttitude;

    [SerializeField] private TextMeshProUGUI _statusText;
    private void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true; 
            Debug.Log("Gyroscope Enabled");
            Debug.Log("Gyroscope is supported on this device.");
            _statusText.text = Input.gyro.enabled ? "Gyroscope: Enabled" : "Gyroscope: Supported but not enabled.";


            if(Input.gyro.enabled)
            {
                Debug.Log("Gyroscope is enabled and active.");
            }
            else
            {
                Debug.LogWarning("Gyroscope is supported but could not be enabled.");
            }
        }
        else
        {
            _statusText.text = "Gyroscope: Not Supported";
            Debug.LogWarning("Gyroscope not supported on this device");
        }
    }

    private void Update()
    {
        if (!Input.gyro.enabled) return;

        // Log gyroscopic gravity and attitude
        gyroGravity = Input.gyro.gravity;
        gyroAttitude = Input.gyro.attitude;

        Debug.Log($"Gyro Gravity: {gyroGravity}");
        Debug.Log($"Gyro Attitude: {gyroAttitude.eulerAngles}");
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || !Input.gyro.enabled) return;

        // Draw gravity direction vector
        Gizmos.color = Color.red;
        Gizmos.DrawLine(player.position, player.position + gyroGravity * 2);

        // Draw an arrow to represent the player's orientation
        Gizmos.color = Color.green;
        Gizmos.DrawRay(player.position, player.forward * 2);

        // Draw a representation of gyroscopic rotation
        Vector3 gyroRotation = gyroAttitude.eulerAngles;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(player.position, Quaternion.Euler(gyroRotation) * Vector3.forward * 2);
    }
}
