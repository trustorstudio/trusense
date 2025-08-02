using TMPro;
using Trusense.Common;
using Trusense.Constants;
using Trusense.Managers;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Trusense.Components.Views;

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
        [SerializeField] private TMP_InputField usernameInput;
        [Tooltip("Input field for entering the password.")]
        [SerializeField] private TMP_InputField passwordInput;

        [Header("UI Icons")]
        [Tooltip("Icon for the username input field.")]
        [SerializeField] private Image usernameIcon;
        [Tooltip("Icon for the password input field.")]
        [SerializeField] private Image passwordIcon;
        private Color validColor = new Color(0f, 0.7176f, 1f, 1f);
        private Color invalidColor = Color.red;
        private Color defaultColor = new Color(0.7176f, 0.7686f, 0.7960f, 1f);

        [Header("UI Buttons")]
        [Tooltip("Button to trigger the sign-in action.")]
        [SerializeField] private Button signinButton;

        [Tooltip("Button to trigger the sign-up action, opening the SignUpPopup.")]
        [SerializeField] private Button signupButton;

        [Header("Remember Me")]
        [Tooltip("Toggle for enabling/disabling the 'Remember Me' feature to save username.")]
        [SerializeField] private Toggle rememberToggle;

        // Regex patterns for UGS validation
        private readonly string usernamePattern = @"^[a-zA-Z0-9][a-zA-Z0-9_\-\.]{1,28}[a-zA-Z0-9]$";
        private readonly string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^\s]{8,100}$";

        // === Initialization ===
        /// <summary>
        /// Initializes the popup, sets up button listeners, and plays the open sound.
        /// </summary>
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

            if (usernameInput != null)
            {
                usernameInput.onValueChanged.RemoveAllListeners();
                usernameInput.onValueChanged.AddListener((value) => OnValidated(value, passwordInput != null ? passwordInput.text : ""));
            }

            if (passwordInput != null)
            {
                passwordInput.onValueChanged.RemoveAllListeners();
                passwordInput.onValueChanged.AddListener((value) => OnValidated(usernameInput != null ? usernameInput.text : "", value));
            }

            OnValidated(usernameInput != null ? usernameInput.text : "", passwordInput != null ? passwordInput.text : "");
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
                rememberToggle.onValueChanged.RemoveListener(OnRememberToggle);
            if (usernameInput != null)
                usernameInput.onValueChanged.RemoveAllListeners();
            if (passwordInput != null)
                passwordInput.onValueChanged.RemoveAllListeners();

            // AudioManager.Instance.PlayPopupCloseSound();

            base.Clean();
        }

        // === Button Handlers ===
        /// <summary>
        /// Handles the sign-in button click, playing a click sound and triggering sign-in logic.
        /// </summary>
        private async void OnSigninButton()
        {
            // AudioManager.Instance.PlayButtonClickSound();
            if (usernameInput == null || passwordInput == null)
            {
                return;
            }

            string username = usernameInput.text;
            string password = passwordInput.text;

            if (!ValidateUsername(username) || !ValidatePassword(password))
            {
                OnValidated(username, password);
                Debug.LogError("Invalid username or password.");
                return;
            }
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


            try
            {
                if (ViewManager.Instance != null)
                {
                    ViewManager.Instance.ShowView<LoadingView>();
                    Hide();
                }
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Sign-in failed: {exception.Message}");
            }
        }

        /// <summary>
        /// Handles the sign-up button click, playing a click sound and opening the SignUpPopup.
        /// </summary>
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

        /// <summary>
        /// Validates the username input.
        /// </summary>
        /// <param name="username">The username to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        private bool ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username) || username.Length < 3 || username.Length > 30)
            {
                return false;
            }
            return Regex.IsMatch(username, usernamePattern);
        }

        /// <summary>
        /// Validates the password input.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        private bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8 || password.Length > 100)
            {
                return false;
            }
            return Regex.IsMatch(password, passwordPattern);
        }

        /// <summary>
        /// Updates the validation state and icon colors for input fields.
        /// </summary>
        /// <param name="username">Current username input.</param>
        /// <param name="password">Current password input.</param>
        private void OnValidated(string username, string password)
        {
            if (usernameInput != null && usernameIcon != null)
            {
                usernameIcon.color = string.IsNullOrEmpty(username) ? defaultColor : ValidateUsername(username) ? validColor : invalidColor;
            }

            if (passwordInput != null && passwordIcon != null)
            {
                passwordIcon.color = string.IsNullOrEmpty(password) ? defaultColor : ValidatePassword(password) ? validColor : invalidColor;
            }
        }
    }
}
