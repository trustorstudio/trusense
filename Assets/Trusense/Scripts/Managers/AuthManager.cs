using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using Trusense.Tools;
using Trusense.Types;
using Unity.Collections;

namespace Trusense.Managers
{
    // === Class Header ===
    /// <summary>
    /// AuthManager handles user authentication using Unity Gaming Services (UGS) Authentication.
    /// Implemented as a Singleton for global access to authentication state and operations.
    /// Supports anonymous, Unity Player Accounts, and Facebook authentication.
    /// </summary>
    public class AuthManager : Singleton<AuthManager>
    {
        // === Properties ===
        [Header("Authentication Status")]
        [Tooltip("Indicates if the player is currently signed in.")]
        [SerializeField, ReadOnly] private bool isLoggedIn;


        // === Events ===
        // [Header("Authentication Events")]
        [Tooltip("Event triggered when the player successfully signs in.")]
        public event Action<string> OnSignedIn; // Passes Player ID

        [Tooltip("Event triggered when sign-in fails.")]
        public event Action<string> OnSignInFailed; // Passes error message

        [Tooltip("Event triggered when the player signs out.")]
        public event Action OnSignedOut;

        // === Internal State ===
        private bool isInitialized;

        // === Properties ===
        public bool IsLoggedIn => AuthenticationService.Instance.IsSignedIn;

        // === Lifecycle Methods ===
        protected override void Awake()
        {
            base.Awake();
            _ = Initialized();
        }

        // === Initialization Logic ===
        /// <summary>
        /// Initializes Unity Services and sets up authentication event listeners.
        /// </summary>
        private async Task Initialized()
        {
            if (isInitialized)
            {
                return;
            }

            try
            {
                var options = new InitializationOptions();
                await UnityServices.InitializeAsync(options);
                AuthenticationService.Instance.SignedIn += () =>
                {
                    isLoggedIn = true;
                    OnSignedIn?.Invoke(AuthenticationService.Instance.PlayerId);
                };
                AuthenticationService.Instance.SignInFailed += (ex) =>
                {
                    isLoggedIn = false;
                    OnSignInFailed?.Invoke(ex.Message);
                };
                AuthenticationService.Instance.SignedOut += () =>
                {
                    isLoggedIn = false;
                    OnSignedOut?.Invoke();
                };

                isInitialized = true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{GetType().Name}: Failed to initialize Unity Services: {ex.Message}");
                isInitialized = false;
            }
        }

        // === Authentication Methods ===
        /// <summary>
        /// Signs in the player using the specified authentication method.
        /// </summary>
        /// <param name="auth">Authentication method (Anonymously, Unity, or Facebook).</param>
        /// <param name="username">Username for Unity Player Accounts (optional).</param>
        /// <param name="password">Password for Unity Player Accounts (optional).</param>
        /// <returns>Task representing the sign-in operation.</returns>
        public async Task SignIn(Auth auth = Auth.Anonymously, string username = "", string password = "")
        {
            if (!isInitialized)
            {
                await Initialized();
                if (!isInitialized)
                {
                    throw new InvalidOperationException("Unity Services initialization failed.");
                }
            }

            if (IsLoggedIn)
            {
                return;
            }

            try
            {
                switch (auth)
                {
                    case Auth.Anonymously:
                        await AuthenticationService.Instance.SignInAnonymouslyAsync();
                        break;

                    case Auth.UsernamePassword:
                        break;

                    case Auth.Facebook:
                        break;

                    default:
                        throw new ArgumentException($"Unsupported authentication method: {auth}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"{GetType().Name}: Sign-in failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Signs up a new player using the specified authentication method.
        /// </summary>
        /// <param name="auth">Authentication method (Unity or Facebook; anonymous not supported for sign-up).</param>
        /// <param name="username">Username or access token (for Facebook).</param>
        /// <param name="password">Password for Unity Player Accounts.</param>
        /// <returns>Task representing the sign-up operation.</returns>
        public async Task SignUp(Auth auth = Auth.Anonymously, string username = "", string password = "")
        {
            if (!isInitialized)
            {
                await Initialized();
                if (!isInitialized)
                {
                    throw new InvalidOperationException("Unity Services initialization failed.");
                }
            }

            if (IsLoggedIn)
            {
                return;
            }

            try
            {
                switch (auth)
                {
                    case Auth.UsernamePassword:
                        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                        {
                            throw new ArgumentException("Username and password are required for Unity Player Accounts sign-up.");
                        }

                        break;

                    case Auth.Facebook:
                        break;

                    case Auth.Anonymously:
                        await AuthenticationService.Instance.SignInAnonymouslyAsync();
                        break;

                    default:
                        throw new ArgumentException($"Unsupported authentication method: {auth}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"{GetType().Name}: Sign-up failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Signs out the current player.
        /// </summary>
        public void SignOut()
        {
            if (!IsLoggedIn)
            {
                return;
            }

            AuthenticationService.Instance.SignOut();
        }

        // === Validation ===

        private void OnValidate()
        {
            isLoggedIn = IsLoggedIn;
        }
    }
}