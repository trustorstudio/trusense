using TMPro;
using Trusense.Common;
using Trusense.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Trusense.Components.Views
{
    /// <summary>
    /// ShopView is a class that inherits from View.
    /// It is used to define the shop view in the Trusense application.
    /// 
    /// Author: Nguyễn Duy Khánh
    /// Created: August 6, 2025
    /// Last Modified: August 6, 2025
    /// Version: 1.0
    /// </summary>
    public class ShopView : View
    {
        // === UI Components ===
        [Header("UI Button Top Components")]
        [Tooltip("Button to back the shop.")]
        [SerializeField] private Button backButton;
        [Tooltip("Button to open the home.")]
        [SerializeField] private Button homeButton;

        // === UI Energy Components ===
        [Header("UI Energy Components")]
        [Tooltip("Button to direction the energy icon.")]
        [SerializeField] private Button energyButton;
        [Tooltip("Text to display the energy value.")]
        [SerializeField] private TMP_Text energyText;

        // === UI Coin Components ===
        [Header("UI Coin Components")]
        [Tooltip("Button to direction the coin value.")]
        [SerializeField] private Button coinButton;
        [Tooltip("Text to display the coin value.")]
        [SerializeField] private TMP_Text coinText;

        // === UI Diamond Components ===
        [Header("UI Diamond Components")]
        [Tooltip("Button to direction the diamond value.")]
        [SerializeField] private Button diamondButton;
        [Tooltip("Text to display the diamond value.")]
        [SerializeField] private TMP_Text diamondText;

        // === UI Button Tap Components ===
        [Header("UI Button Tap Components")]
        [Tooltip("Image to tap the focus.")]
        [SerializeField] private Image focusImage;
        [Tooltip("Button to tap the daily.")]
        [SerializeField] private Button dailyButton;
        [Tooltip("Button to tap the chest.")]
        [SerializeField] private Button chestButton;
        [Tooltip("Button to tap the gold.")]
        [SerializeField] private Button goldButton;
        [Tooltip("Button to tap the gem.")]
        [SerializeField] private Button gemButton;


        /// <summary>
        /// Initializes the ShopView.
        /// Sets up the button listeners and any necessary initial state.
        /// </summary>
        public override void Initialized()
        {
            if (_isInitialized) return;
            if (backButton != null)
            {
                backButton.onClick.RemoveAllListeners();
                backButton.onClick.AddListener(HandleBack);
            }

            if (homeButton != null)
            {
                homeButton.onClick.RemoveAllListeners();
                homeButton.onClick.AddListener(HandleHome);
            }
            _isInitialized = true;
        }

        /// <summary>
        /// Cleans up the ShopView.
        /// Releases resources or resets state as needed.
        /// </summary>
        public override void Clean()
        {
            if (backButton != null)
            {
                backButton.onClick.RemoveListener(HandleBack);
            }

            if (homeButton != null)
            {
                homeButton.onClick.RemoveListener(HandleHome);
            }
        }


        /// <summary>
        /// Handles the action when the back button is clicked.
        /// This method is called when the user clicks the back button in the shop view.
        /// </summary>
        private void HandleBack()
        {
            ViewManager.Current.ShowLast();
        }

        /// <summary>
        /// Handles the action when the home button is clicked.
        /// This method is called when the user clicks the home button in the shop view.
        /// </summary>
        private void HandleHome()
        {
            ViewManager.Current.Show<LobbyView>();
        }


    }
}