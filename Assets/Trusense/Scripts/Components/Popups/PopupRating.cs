using Trusense.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Trusense.Components.Popups
{
    /// <summary>
    /// PopupRating is a class that inherits from Popup.
    /// It is used to define a popup for rating in the Trusense application.
    /// 
    /// Author: Nguyễn Duy Khánh
    /// Created: August 6, 2025
    /// Last Modified: August 6, 2025
    /// Version: 1.0.1
    /// </summary>
    public class PopupRating : Popup
    {
        // === Rating Settings ===
        [Header("Rating Settings")]
        [Tooltip("Duration of the handle movement animation in seconds.")]
        [SerializeField] private Button rateButton;

        /// <summary>
        /// Initializes the popup, setting up event listeners for the Rate button.
        /// </summary>
        public override void Initialized()
        {
            if (_isInitialized) return;
            base.Initialized();

            if (rateButton != null)
            {
                rateButton.onClick.RemoveAllListeners();
                rateButton.onClick.AddListener(HandleRate);
            }
        }


        /// <summary>
        /// Cleans up resources when the popup is closed or destroyed.
        /// Removes listeners from the Rate button to prevent memory leaks.
        /// </summary>
        public override void Clean()
        {
            if (rateButton != null)
            {
                rateButton.onClick.RemoveListener(HandleRate);
            }
        }


        // == Private Methods ===
        /// <summary>
        /// Handles the rating action when the Rate button is clicked.
        /// This method is called when the user clicks the Rate button in the popup.
        /// </summary>
        private void HandleRate()
        {
            if (rateButton != null)
            {
                Application.OpenURL("https://play.google.com/store/apps/details?id=com.trusense.app");
                this.Hide();
            }

        }
    }

}