using Trusense.Common;
using Trusense.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Trusense.Components.Popups
{
    /// <summary>
    /// PopupSetting is a class that inherits from Popup.
    /// It is used to define settings for popups in the Trusense application.
    /// This class can be extended to include specific settings or behaviors for popups.
    /// 
    /// Author: Nguyễn Duy Khánh
    /// Created: August 6, 2025
    /// Last Modified: August 6, 2025
    /// Version: 1.0
    /// </summary>
    public class PopupSetting : Popup
    {
        // === UI Button Components ===
        [Header("UI Button Components")]
        [Tooltip("The button that opens the language settings.")]
        [SerializeField] private Button languageButton;

        /// <summary>
        /// Initializes the popup settings.
        /// This method can be overridden to set up specific settings for the popup.
        /// </summary>
        public override void Initialized()
        {
            if (_isInitialized) return;
            base.Initialized();
            if (languageButton != null)
            {
                languageButton.onClick.RemoveAllListeners();
                languageButton.onClick.AddListener(HandleLanguage);
            }

        }

        /// <summary>
        /// Cleans up resources when the popup is closed or destroyed.
        /// This method can be overridden to handle cleanup specific to the popup settings.
        /// </summary>
        public override void Clean()
        {
            if (languageButton != null)
            {
                languageButton.onClick.RemoveListener(HandleLanguage);
            }

            base.Clean();
        }

        /// <summary>
        /// Handles the click event for the language settings button.
        /// This method can be extended to implement specific logic for changing language settings.
        /// </summary>
        private void HandleLanguage()
        {
            PopupManager.Current.Show<PopupSettingLanguage>();
        }
    }
}