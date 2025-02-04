using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GyroscopePrototype
{
    public class GyroscopeSettingsHandler : MonoBehaviour
    {
        [Header("Application Close Button")]
        [SerializeField] private Button _applicationCloseButton;

        private void OnEnable()
        {
            _applicationCloseButton.onClick.AddListener(OnApplicationCloseButton);
        }

        private void OnDisable()
        {
            _applicationCloseButton.onClick.RemoveListener(OnApplicationCloseButton);
        }

        #region Application Close Button
        
        private void OnApplicationCloseButton()
        {
            Application.Quit();
            Debug.Log("Application is Closing");
        }
        #endregion
    }
}