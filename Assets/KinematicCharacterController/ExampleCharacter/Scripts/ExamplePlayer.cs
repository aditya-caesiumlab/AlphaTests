﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using GyroscopePrototype;
using TMPro;

namespace KinematicCharacterController.Examples
{
    public class ExamplePlayer : MonoBehaviour
    {
        public ExampleCharacterController Character;
        public ExampleCharacterCamera CharacterCamera;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        //[SerializeField] TextMeshProUGUI normalizedDeltaValue;
        //[SerializeField] TextMeshProUGUI GyroRawValues;

        [SerializeField]GyroInputController GyroController;

        private void Start()
        {
            ControlFreak2.CFCursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
        }


        //Vector3 oldGyroValues;
        //Vector3 rawGyroEulerAngles;
        //public Vector3 GyRoDeltaValue;

        //void GetGyroDeltaValue()
        //{
        //    oldGyroValues = rawGyroEulerAngles;
        //    rawGyroEulerAngles = Input.gyro.attitude.eulerAngles;

        //    GyroRawValues.text = $" X : {rawGyroEulerAngles.x} , Y : {rawGyroEulerAngles.y}, z :{rawGyroEulerAngles.z} ";
            
        //    GyRoDeltaValue = NormalizeAngleDifference(rawGyroEulerAngles - oldGyroValues);
        //    normalizedDeltaValue.text = $" X : {GyRoDeltaValue.x} , Y : {GyRoDeltaValue.y}, z :{GyRoDeltaValue.z} ";

        //    Debug.Log("Getting Gyro Input");
        //}

        //// Function to handle 360-degree wrapping issues
        //Vector3 NormalizeAngleDifference(Vector3 delta)
        //{
        //    return new Vector3(
        //        NormalizeSingleAxis(delta.x),
        //        NormalizeSingleAxis(delta.y),
        //        NormalizeSingleAxis(delta.z)
        //    );
        //}

        //float NormalizeSingleAxis(float angle)
        //{
        //    // Ensure the difference is between -180 and 180
        //    angle = (angle + 180) % 360 - 180;
        //    return angle;
        //}



        private void Update()
        {




            if (ControlFreak2.CF2Input.GetMouseButtonDown(0))
            {
                ControlFreak2.CFCursor.lockState = CursorLockMode.Locked;
            }

            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
            {
                CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
                CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
            }
            //GetGyroDeltaValue();
            HandleCameraInput();
        }
         
        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = ControlFreak2.CF2Input.GetAxisRaw(MouseYInput) +GyroController.GyRoDeltaValue.x;
            float mouseLookAxisRight = ControlFreak2.CF2Input.GetAxisRaw(MouseXInput) + GyroController.GyRoDeltaValue.y;
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (ControlFreak2.CFCursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = -ControlFreak2.CF2Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            CharacterCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

            // Handle toggling zoom level
            if (ControlFreak2.CF2Input.GetMouseButtonDown(1))
            {
                CharacterCamera.TargetDistance = (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
            }
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = ControlFreak2.CF2Input.GetAxisRaw(VerticalInput);
            characterInputs.MoveAxisRight = ControlFreak2.CF2Input.GetAxisRaw(HorizontalInput);
            characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
            characterInputs.JumpDown = ControlFreak2.CF2Input.GetKeyDown(KeyCode.Space);
            characterInputs.CrouchDown = ControlFreak2.CF2Input.GetKeyDown(KeyCode.C);
            characterInputs.CrouchUp = ControlFreak2.CF2Input.GetKeyUp(KeyCode.C);

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }
    }
}