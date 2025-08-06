using UnityEngine;
using Trusense.Managers;
using DG.Tweening;
using Trusense.Common;
using System.Collections;
using TMPro;

namespace Trusense.Components.Popups
{
    /// <summary>
    /// PopupErrorNetwork displays a popup when there is no internet connection.
    /// Inherits from Popup to leverage animated show/hide behavior using DOTween.
    /// Efficiently monitors network status and updates visibility accordingly.
    /// </summary>
    public class PopupErrorNetwork : Popup
    {
        // === UI Components ===
        [Header("Popup Error Network Settings")]
        [Tooltip("Text component to display the network error title.")]
        [SerializeField] private TMP_Text errorTitleText;
        [Tooltip("Default error title when no internet is detected.")]
        [SerializeField] private string defaultErrorTitle = "Network connection failed.";
        [Tooltip("Text component to display the network error message.")]
        [SerializeField] private TMP_Text errorMessageText;
        [Tooltip("Default error message when no internet is detected.")]
        [SerializeField] private string defaultErrorMessage = "Please check your cellular or Wi-Fi connection and retry.";

        // === Internal State ===
        private bool isNetwork;
        private Coroutine networkCoroutine;

        // === Initialization Logic ===
        /// <summary>
        /// Initializes the popup, sets the error message, and checks initial network status.
        /// Overrides the Initialized method from the Popup base class.
        /// </summary>
        public override void Initialized()
        {
            if (_isInitialized) return;
            base.Initialized();

            if (errorTitleText != null)
            {
                errorTitleText.text = defaultErrorTitle;
            }

            if (errorMessageText != null)
            {
                errorMessageText.text = defaultErrorMessage;
            }

            CheckNetwork();
        }

        // === Content Update ===
        /// <summary>
        /// Refreshes the popup content by re-checking network status.
        /// Overrides the Refresh method from the Popup base class.
        /// </summary>
        public override void Refresh()
        {
            CheckNetwork();
        }

        // === Network Status Check ===
        /// <summary>
        /// Checks network status and updates popup visibility if needed.
        /// Only updates UI when network status changes to optimize performance.
        /// </summary>
        private void CheckNetwork()
        {
            bool wasNetworkAvailable = isNetwork;
            isNetwork = Application.internetReachability != NetworkReachability.NotReachable;

            if (isNetwork != wasNetworkAvailable)
            {
                if (!isNetwork)
                {
                    if (!_isVisible)
                    {
                        PopupManager.Current.Show(this);
                        if (errorTitleText != null && errorTitleText.text != defaultErrorTitle)
                        {
                            errorTitleText.text = defaultErrorTitle;
                        }

                        if (errorMessageText != null && errorMessageText.text != defaultErrorTitle)
                        {
                            errorMessageText.text = defaultErrorMessage;
                        }
                    }
                }
                else if (_isVisible)
                {

                    PopupManager.Current.HidePopup();
                }
            }
        }

        // === Unity Lifecycle ===
        /// <summary>
        /// Called when the GameObject is enabled.
        /// Starts periodic network monitoring if not already running.
        /// </summary>
        private void OnEnable()
        {
            if (networkCoroutine == null)
            {
                networkCoroutine = StartCoroutine(NetworkCheckCoroutine());
            }
        }

        /// <summary>
        /// Called when the GameObject is disabled.
        /// Stops the network check coroutine to prevent memory leaks.
        /// </summary>
        private void OnDisable()
        {
            if (networkCoroutine != null)
            {
                StopCoroutine(networkCoroutine); // Stop coroutine
                networkCoroutine = null; // Clear reference
            }
        }

        // === Periodic Network Check ===
        /// <summary>
        /// Coroutine that periodically checks network status every 2 seconds.
        /// Uses WaitForSecondsRealtime to ensure checks are unaffected by Time.timeScale.
        /// </summary>
        private IEnumerator NetworkCheckCoroutine()
        {
            while (true)
            {
                CheckNetwork();
                yield return new WaitForSecondsRealtime(2f);
            }
        }
    }
}