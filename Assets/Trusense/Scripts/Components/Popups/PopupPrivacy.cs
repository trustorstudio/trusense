using Trusense.Common;
using Trusense.Constants;
using Trusense.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Trusense.Components.Popups
{
    /// <summary>
    /// PopupPrivacy displays a popup for the game's privacy policy.
    /// It allows players to accept the privacy policy or view the terms of service via a URL.
    /// Inherits from the Popup class to leverage common popup functionality.
    /// 
    /// Author: [Your Name]
    /// Created: August 6, 2025
    /// Last Modified: August 6, 2025
    /// Version: 1.0
    /// </summary>
    public class PopupPrivacy : Popup
    {
        // === UI Components ===
        [Header("UI Components")]
        [Tooltip("The button that players click to accept the privacy policy.")]
        [SerializeField] private Button acceptButton;

        [Tooltip("The button that opens the privacy url in a web browser.")]
        [SerializeField] private Button privacyButton;

        /// <summary>
        /// Initializes the popup, setting up event listeners for the Accept and Terms of Service buttons.
        /// </summary>
        public override void Initialized()
        {
            if (_isInitialized) return;
            base.Initialized();

            if (acceptButton != null)
            {
                acceptButton.onClick.RemoveAllListeners();
                acceptButton.onClick.AddListener(HandleAccept);
            }

            if (privacyButton == null)
            {
                privacyButton.onClick.RemoveAllListeners();
                privacyButton.onClick.AddListener(HandlePrivacy);
            }


        }

        /// <summary>
        /// Cleans up resources when the popup is closed or destroyed.
        /// Removes listeners from the buttons to prevent memory leaks.
        /// </summary>
        public override void Clean()
        {
            if (acceptButton != null)
            {
                acceptButton.onClick.RemoveListener(HandleAccept);
            }
            if (privacyButton != null)
            {
                privacyButton.onClick.RemoveListener(HandlePrivacy);
            }

            base.Clean();
        }

        /// <summary>
        /// Handles the Accept button click event.
        /// Saves the privacy acceptance status, triggers the auth action, and closes the popup.
        /// </summary>
        private void HandleAccept()
        {

            PlayerPrefs.SetInt(Keys.PRIVACY, 1);
            PlayerPrefs.Save();
            AuthManager.Current.OnAuthAction?.Invoke();
        }

        /// <summary>
        /// Handles the Terms of Service button click event.
        /// Opens the terms of service URL in the default web browser.
        /// </summary>
        private void HandlePrivacy()
        {
            Application.OpenURL("https://dualtarget.xyz");
        }
    }
}