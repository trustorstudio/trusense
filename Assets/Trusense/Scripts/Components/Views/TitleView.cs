using UnityEngine;
using Trusense.Common;
using UnityEngine.UI;
using Trusense.Managers;
using Trusense.Components.Popups;
using Trusense.Constants;

namespace Trusense.Components.Views
{
    // === Class Header ===
    /// <summary>
    /// TitleView represents the title screen UI, providing buttons to start the game, access account settings, 
    /// and integrate with Facebook. It handles user authentication and privacy policy checks before proceeding.
    /// Inherits from the View class to leverage common view functionality.
    /// 
    /// Author: Nguyễn Duy Khánh
    /// Created: August 6, 2025
    /// Last Modified: August 6, 2025
    /// Version: 1.0
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
            if (_isInitialized) return;
            if (startButton != null)
                startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnStartButton);
            if (accountButton != null)
                accountButton.onClick.RemoveAllListeners();
            accountButton.onClick.AddListener(HandleAccount);
            if (facebookButton != null)
                facebookButton.onClick.RemoveAllListeners();
            facebookButton.onClick.AddListener(HandleFacebook);
            bool isLoggedIn = AuthManager.Current.IsLoggedIn;
            if (accountButton != null)
                accountButton.gameObject.SetActive(!isLoggedIn);
            if (facebookButton != null)
                facebookButton.gameObject.SetActive(!isLoggedIn);

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

        /// <summary>
        /// Handles the Start button click event.
        /// Checks authentication and privacy status before proceeding to the loading screen or showing popups.
        /// </summary>
        private void OnStartButton()
        {
            if (AuthManager.Current.IsLoggedIn)
            {
                ViewManager.Current.Show<LoadingView>();
            }
            else
            {
                if (CheckPrivacy())
                {
                    System.Action onAuthAction = null;
                    onAuthAction = async () =>
                    {
                        AuthManager.Current.OnAuthAction -= onAuthAction;
                        await AuthManager.Current.SignIn();
                        ViewManager.Current.Show<LoadingView>();
                    };

                    AuthManager.Current.OnAuthAction += onAuthAction;
                    AuthManager.Current.OnAuthAction?.Invoke();
                }
                else
                {
                    PopupManager.Current.Show<PopupPrivacy>();
                }
            }
        }

        /// <summary>
        /// Handles the Account button click event.
        /// Opens the sign-in popup for account management.
        /// </summary>
        private void HandleAccount()
        {
            PopupManager.Current.Show<SignInPopup>();
        }

        /// <summary>
        /// Handles the Facebook button click event.
        /// Initiates Facebook integration or login (to be implemented).
        /// </summary>
        private void HandleFacebook()
        {

        }

        /// <summary>
        /// Checks if the privacy policy has been accepted.
        /// </summary>
        /// <returns>True if the privacy policy is accepted, false otherwise.</returns>
        private bool CheckPrivacy()
        {
            return PlayerPrefs.GetInt(Keys.PRIVACY, 0) == 1;
        }

    }
}