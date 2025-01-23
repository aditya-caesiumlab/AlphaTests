using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ramesh.gyroscopeScripts_2
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Transform _player;
        [SerializeField] private float SmoothSpeed;
        private void LateUpdate()
        {
            Vector3 desiredPos = transform.position + _offset;
            Vector3 smoothPos = Vector3.Lerp(desiredPos, _player.transform.position, SmoothSpeed);

            transform.position = smoothPos;

        }
    }
}