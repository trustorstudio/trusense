using TMPro;
using Trusense.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Trusense.Scripts.Components.Popups
{
    /// <summary>
    /// PopupChecking displays a popup to inform players about the game's maintenance status.
    /// When the Accept button is clicked, the game exits if it is under maintenance.
    /// Inherits from the Popup class to leverage common popup functionality.
    /// 
    /// Author: Nguyễn Duy Khánh
    /// Created: August 6, 2025
    /// Last Modified: August 6, 2025
    /// Version: 1.0
    /// </summary>
    public class PopupChecking : Popup
    {
        [Header("UI Components")]
        [Tooltip("The button that players click to confirm and exit the game during maintenance.")]
        [SerializeField] private Button acceptButton;
        [Tooltip("TextMeshPro component to display the popup's title.")]
        [SerializeField] private TMP_Text titleText;

        [Tooltip("TextMeshPro component to display the maintenance message.")]
        [SerializeField] private TMP_Text messageText;
        [Header("Popup Settings")]

        [Tooltip("The default title displayed in the popup (e.g., 'Under Maintenance').")]
        [SerializeField] private string title = "Under Maintenance";

        [Tooltip("The default message displayed in the popup to inform about maintenance status.")]
        [SerializeField] private string message = "Under maintenance.\nPlease check our announcement.";
        private bool _isUnderMaintenance;

        /// <summary>
        /// Initializes the popup when it is first displayed.
        /// Sets up the Accept button event, updates the UI, and checks the maintenance status.
        /// </summary>
        public override void Initialized()
        {
            if (_isInitialized) return;
            base.Initialized();

            if (acceptButton != null)
            {
                acceptButton.onClick.RemoveAllListeners();
                acceptButton.onClick.AddListener(OnAcceptButton);
            }

            if (titleText != null)
            {
                titleText.text = title;
            }

            if (messageText != null)
            {
                messageText.text = message;
            }

            CheckMaintenance();
        }

        /// <summary>
        /// Cleans up resources when the popup is closed or destroyed.
        /// Removes listeners from the Accept button to prevent memory leaks.
        /// </summary>
        public override void Clean()
        {
            if (acceptButton != null)
            {
                acceptButton.onClick.RemoveListener(OnAcceptButton);
            }

            base.Clean();
        }


        /// <summary>
        /// Checks the maintenance status of the game.
        /// Updates the message based on whether maintenance is active.
        /// </summary>
        private void CheckMaintenance()
        {
            _isUnderMaintenance = IsMaintenanceTime();

            if (_isUnderMaintenance && messageText != null)
            {
                messageText.text = "The game is under maintenance.\nPlease check our announcement.";
            }

        }

        /// <summary>
        /// Simulates checking if the game is under maintenance based on the current time.
        /// </summary>
        /// <returns>True if the game is under maintenance, false otherwise.</returns>
        private bool IsMaintenanceTime()
        {
            int currentHour = System.DateTime.Now.Hour;
            return currentHour >= 11 && currentHour < 12;
        }

        /// <summary>
        /// Handles the Accept button click event.
        /// Exits the game if under maintenance; otherwise, closes the popup.
        /// </summary>
        private void OnAcceptButton()
        {
            if (_isUnderMaintenance)
            {

                PlayerPrefs.Save();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit(); 
#endif
            }
            else
            {

                this.Hide();
            }
        }

    }
}
