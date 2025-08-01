using Trusense.Common;
using Trusense.Constants;
using Trusense.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Trusense.Components.Popups
{
    // === Class Header ===
    /// <summary>
    /// Represents the sign-in popup UI, inheriting from Popup. Handles sign-in and sign-up button interactions
    /// with audio feedback for opening, closing, and button clicks.
    /// </summary>
    public class SignInPopup : Popup
    {
        // === UI Components ===
        [Header("UI Input Fields")]
        [Tooltip("Input field for entering the username or email.")]
        [SerializeField] private InputField usernameInput;
        [Tooltip("Input field for entering the password.")]
        [SerializeField] private InputField passwordInput;

        [Header("UI Buttons")]
        [Tooltip("Button to trigger the sign-in action.")]
        [SerializeField] private Button signinButton;

        [Tooltip("Button to trigger the sign-up action, opening the SignUpPopup.")]
        [SerializeField] private Button signupButton;

        [Header("Remember Me")]
        [Tooltip("Toggle for enabling/disabling the 'Remember Me' feature to save username.")]
        [SerializeField] private Toggle rememberToggle;

        // === Initialization ===
        /// <summary>
        /// Initializes the popup, sets up button listeners, and plays the open sound.
        /// </summary>
        [System.Obsolete("Use OnEnable or a custom initialization method instead.")]
        public override void Initialized()
        {
            base.Initialized();
            // AudioManager.Instance.PlayPopupOpenSound();

            bool isRemember = PlayerPrefs.GetInt(Keys.REMEMBER_ME, 0) == 1;
            if (rememberToggle != null)
            {
                rememberToggle.isOn = isRemember;
                rememberToggle.onValueChanged.RemoveAllListeners();
                rememberToggle.onValueChanged.AddListener(OnRememberToggle);
            }

            if (isRemember && usernameInput != null)
            {
                usernameInput.text = PlayerPrefs.GetString(Keys.USERNAME, "");
            }
            if (signinButton != null)
            {
                signinButton.onClick.RemoveAllListeners();
                signinButton.onClick.AddListener(OnSigninButton);
            }

            if (signupButton != null)
            {
                signupButton.onClick.RemoveAllListeners();
                signupButton.onClick.AddListener(OnSignupButton);
            }

        }

        // === Cleanup ===
        /// <summary>
        /// Cleans up the popup, removes button listeners, and plays the close sound.
        /// </summary>
        public override void Clean()
        {
            // Remove button listeners to prevent memory leaks
            if (signinButton != null)
                signinButton.onClick.RemoveListener(OnSigninButton);
            if (signupButton != null)
                signupButton.onClick.RemoveListener(OnSignupButton);
            if (rememberToggle != null)
            {
                rememberToggle.onValueChanged.RemoveListener(OnRememberToggle);
            }

            // AudioManager.Instance.PlayPopupCloseSound();

            base.Clean();
        }

        // === Button Handlers ===
        /// <summary>
        /// Handles the sign-in button click, playing a click sound and triggering sign-in logic.
        /// </summary>
        private void OnSigninButton()
        {
            // AudioManager.Instance.PlayButtonClickSound();

            if (rememberToggle != null && rememberToggle.isOn && usernameInput != null)
            {
                PlayerPrefs.SetInt(Keys.REMEMBER_ME, 1);
                PlayerPrefs.SetString(Keys.USERNAME, usernameInput.text);
                PlayerPrefs.Save();
            }
            else
            {
                PlayerPrefs.SetInt(Keys.REMEMBER_ME, 0);
                PlayerPrefs.Save();
            }


            Debug.Log("Sign-in button clicked.");
        }

        /// <summary>
        /// Handles the sign-up button click, playing a click sound and opening the SignUpPopup.
        /// </summary>
        [System.Obsolete("Consider replacing with a non-obsolete method for popup navigation.")]
        private void OnSignupButton()
        {
            // AudioManager.Instance.PlayButtonClickSound();
            PopupManager.Instance.ShowPopup<SignUpPopup>(true, true);
        }

        /// <summary>
        /// Handles the Remember Me toggle change, plays a click sound, and updates the saved state.
        /// </summary>
        /// <param name="isOn">True if Remember Me is enabled, false otherwise.</param>
        private void OnRememberToggle(bool isOn)
        {
            PlayerPrefs.SetInt(Keys.REMEMBER_ME, isOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
