using UnityEngine;
using UnityEngine.EventSystems;

namespace Ramesh.gyroscopeScripts_2
{
    public class TPSController : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private FixedJoystick _fixedJoystick;
        [SerializeField] private Transform _camera;
        [SerializeField] private float _mouseSensitivity = 100f;

        private Vector3 _moveDirection;

        bool touching;
        private Vector2 lastTouchPosition;
        Quaternion inputRot;
        private float rotationY;
        [SerializeField] private float playerRotMulitplayer;

        [SerializeField] private Transform FpsPoint;
        [SerializeField] private Transform TpsPoint;
        [SerializeField] private GameObject PlayerBody;

        public bool Switched;
        private void Update()
        {
            /*Quaternion CamRot = Quaternion.Euler(0, _camera.rotation.y * playerRotMulitplayer, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, CamRot, 1);*/
            UpdatePlayerRotation();
            HandleMovement();
        }

        // To stop rotation when the mouse is released
        private void OnMouseUp()
        {
            touching = false;
        }
        private void HandleMovement()
        {
            float horizontal = _fixedJoystick.Horizontal;
            float vertical = _fixedJoystick.Vertical;

            // Calculate the movement direction relative to the camera
            _moveDirection = new Vector3(horizontal, 0, vertical).normalized;
            if (_moveDirection.magnitude >= 0.1f)
            {
                // Move the player in the direction the camera is facing
                Vector3 movement = _camera.TransformDirection(_moveDirection);
                movement.y = 0f; // Prevent vertical movement
                transform.position += movement * _speed * Time.deltaTime;
            }
        }

        private void UpdatePlayerRotation()
        {

            Vector3 cameraForward = _camera.forward;
            cameraForward.y = 0; // Keep player upright
            transform.rotation = Quaternion.LookRotation(cameraForward);

        }

        public void OnFpsSwitch()
        {
            if (!Switched)
            {
                _camera.transform.position = Vector3.Lerp(_camera.transform.position, FpsPoint.position, 1);
                PlayerBody.SetActive(false);
                Switched = true;
            }
            else
            {
                _camera.transform.position = Vector3.Lerp(_camera.transform.position, TpsPoint.position, 1);
                PlayerBody.SetActive(true);
                Switched = false;
            }

        }
    }
}