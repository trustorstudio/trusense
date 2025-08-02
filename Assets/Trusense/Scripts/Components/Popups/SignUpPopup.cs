using System.Text.RegularExpressions;
using TMPro;
using Trusense.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Trusense.Components.Popups
{
    // === Class Header ===
    /// <summary>
    /// Represents the sign-up popup UI, inheriting from Popup. Handles sign-up and sign-in button interactions
    /// with audio feedback for opening, closing, and button clicks.
    /// </summary>
    public class SignUpPopup : Popup
    {
        // === UI Components ===
        [Header("UI Input Feilds")]
        [Tooltip("Input field for entering the email")]
        [SerializeField] private TMP_InputField emailInput;
        [Tooltip("Input field for entering the username")]
        [SerializeField] private TMP_InputField usernameInput;
        [Tooltip("Input feild for entering the password")]
        [SerializeField] private TMP_InputField passwordInput;

        [Header("UI Buttons")]
        [Tooltip("Button to trigger the sign-up action")]
        [SerializeField] private Button signupButton;
        [Tooltip("Button  to trigger the sign-in action, opening the sign-in-popup")]
        [SerializeField] private Button signinButton;

        [Header("UI Icons")]
        [Tooltip("Icon for the username input field.")]
        [SerializeField] private Image usernameIcon;
        [Tooltip("Icon for the password input field.")]
        [SerializeField] private Image passwordIcon;
        [Tooltip("Icon for the email input field.")]
        [SerializeField] private Image emailIcon;

        // === validate color ===
        private Color validColor = new Color(0f, 0.7176f, 1f, 1f);
        private Color invalidColor = Color.red;
        private Color defaultColor = new Color(0.7176f, 0.7686f, 0.7960f, 1f);

        // === Regex patterns for UGS validation ===
        private readonly string usernamePattern = @"^[a-zA-Z0-9][a-zA-Z0-9_\-\.]{1,28}[a-zA-Z0-9]$";
        private readonly string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^\s]{8,100}$";
        private readonly string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        // === Initialization ===
        /// <summary>
        /// Initializes the popup, sets up button listeners, and plays the open sound.
        /// </summary>
        public override void Initialized()
        {
            base.Initialized();

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
            if (emailInput != null)
                emailInput.onValueChanged.RemoveAllListeners();
            if (usernameInput != null)
                usernameInput.onValueChanged.RemoveAllListeners();
            if (passwordInput != null)
                passwordInput.onValueChanged.RemoveAllListeners();
            base.Clean();
        }

        // === Button Handler ===
        /// <summary>
        /// Handles the sign-up button click, playing a click sound and opening the SignUpPopup.
        /// </summary>
        private void OnSignupButton()
        {

        }

        /// <summary>
        /// Handles the sign-up button click, playing a click sound and triggering sign-up logic.
        /// </summary>
        private void OnSigninButton()
        {

        }

        // === Handle Validate ===
        /// <summary>
        /// Validates the email input.
        /// </summary>
        /// <param name="email">The email to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || email.Length < 3 || email.Length > 30)
            {
                return false;
            }
            return Regex.IsMatch(email, emailPattern);
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
        /// <param name="email">Current password input.</param>
        private void OnValidated(string username, string password, string email)
        {
            if (usernameInput != null && usernameIcon != null)
            {
                usernameIcon.color = string.IsNullOrEmpty(username) ? defaultColor : ValidateUsername(username) ? validColor : invalidColor;
            }

            if (passwordInput != null && passwordIcon != null)
            {
                passwordIcon.color = string.IsNullOrEmpty(password) ? defaultColor : ValidatePassword(password) ? validColor : invalidColor;
            }

            if (emailInput != null && emailIcon != null)
            {
                emailIcon.color = string.IsNullOrEmpty(email) ? defaultColor : ValidateEmail(email) ? validColor : invalidColor;
            }
        }
    }
}