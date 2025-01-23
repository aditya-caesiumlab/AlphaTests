using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GyroscopePrototype
{
    public class GyroscopeSettingsHandler : MonoBehaviour
    {
        [Header("Settings Attributes")]
        [SerializeField] private Button _settingsButton;

        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private Button _closeButton;

        [Header("Settings Buttons Attributes")]
        [SerializeField] private Button _alwaysOnButton;
        [SerializeField] private Button _scopeOnButton;
        [SerializeField] private Button _offButton;

        private void OnEnable()
        {
            _settingsButton.onClick.AddListener(OnSettingsButton);
            _closeButton.onClick.AddListener(OnCloseButton);

            _alwaysOnButton.onClick.AddListener(OnAlwaysOnButton);
            _scopeOnButton.onClick.AddListener(OnScopeOnButton);
            _offButton.onClick.AddListener(OnOffButton);
        }

        private void OnDisable()
        {
            _settingsButton.onClick.RemoveListener(OnSettingsButton);
            _closeButton.onClick.RemoveListener(OnCloseButton);

            _alwaysOnButton.onClick.RemoveListener(OnAlwaysOnButton);
            _scopeOnButton.onClick.RemoveListener(OnScopeOnButton);
            _offButton.onClick.RemoveListener(OnOffButton);
        }

        #region Settings Button Method

        private void OnSettingsButton()
        {
            _settingsPanel.SetActive(true);
        }

        private void OnCloseButton()
        {
            _settingsPanel.SetActive(false);
        }
        #endregion

        #region Gyro Setting Button Methods

        private void OnAlwaysOnButton()
        {
            Debug.Log("Gyroscope AlwaysOn Button Clicked");
        }

        private void OnScopeOnButton()
        {
            Debug.Log("Gyroscope ScopeOn Button Clicked");
        }

        private void OnOffButton()
        {
            Debug.Log("Gyroscope OFF Button Clicked");
        }

        #endregion
    }
}