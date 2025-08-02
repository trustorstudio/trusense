using UnityEngine;
using Trusense.Common;
using UnityEngine.UI;
using Trusense.Managers;
using Trusense.Components.Popups;

namespace Trusense.Components.Views
{
    // === Class Header ===
    /// <summary>
    /// TitleView class inherits from View, representing the title screen UI with buttons for starting the game, accessing account, and Facebook integration.
    /// </summary>
    public class TitleView : View
    {
        // === UI Components ===
        [Header("UI Components")]
        [Tooltip("Button to start the game.")]
        [SerializeField] private Button startButton;

        [Tooltip("Button to access account settings.")]
        [SerializeField] private Button accountButton;

        [Tooltip("Button for Facebook integration.")]
        [SerializeField] private Button facebookButton;


        /// <summary>
        /// Initializes the TitleView.
        /// This method should set up any necessary references or initial state for the TitleView.
        /// </summary>
        public override void Initialized()
        {
            if (_isInitialized)
            {
                return;
            }

            if (startButton != null)
            {
                startButton.onClick.RemoveAllListeners();
                startButton.onClick.AddListener(OnStartButton);
            }

            if (accountButton != null)
            {
                accountButton.onClick.RemoveAllListeners();
                accountButton.onClick.AddListener(OnAccountButton);
            }

            if (facebookButton != null)
            {
                facebookButton.onClick.RemoveAllListeners();
                facebookButton.onClick.AddListener(OnFacebookButton);
            }

            if (AuthManager.Instance.IsLoggedIn)
            {
                accountButton.gameObject.SetActive(!AuthManager.Instance.IsLoggedIn);
                facebookButton.gameObject.SetActive(!AuthManager.Instance.IsLoggedIn);
            }

            _isInitialized = true;
        }

        /// <summary>
        /// Cleans up the TitleView.
        /// This method should release resources or reset state as needed.
        /// </summary>
        public override void Clean()
        {
            if (startButton != null)
                startButton.onClick.RemoveAllListeners();
            if (accountButton != null)
                accountButton.onClick.RemoveAllListeners();
            if (facebookButton != null)
                facebookButton.onClick.RemoveAllListeners();
        }


        private void OnStartButton()
        {
            ViewManager.Instance.ShowView<LobbyView>();
        }

        [System.Obsolete]
        private void OnAccountButton()
        {
            PopupManager.Instance.ShowPopup<SignInPopup>();
        }

        private void OnFacebookButton()
        {

        }
    }
}