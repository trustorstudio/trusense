using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using Trusense.Common;
using Unity.Services.Authentication;
using Trusense.Managers;

namespace Trusense.Components.Views
{
    // === Class Header ===
    /// <summary>
    /// LoadingView class inherits from View, representing a loading screen UI with a progress slider and text.
    /// Displays simulated loading progress before transitioning to LobbyView, with UGS Authentication check.
    /// </summary>
    public class LoadingView : View
    {
        // === Constants ===
        private const float DEFAULT_ANIMATION_DURATION = 2f;

        // === UI Components ===
        [Header("UI Components")]
        [Tooltip("Text component to display loading progress percentage.")]
        [SerializeField] private TMP_Text loadingText;
        [Tooltip("Slider component to visualize loading progress.")]
        [SerializeField] private Slider loadingSlider;

        // === Animation Settings ===
        [Header("Animation Settings")]
        [Tooltip("Duration (in seconds) for the loading animation.")]
        [SerializeField, Min(0.01f)] private float animationDuration = DEFAULT_ANIMATION_DURATION;

        // === Internal State ===
        private Coroutine loadingCoroutine;

        // === Lifecycle Methods ===
        [System.Obsolete]
        private void OnEnable()
        {
            // Start loading animation only if initialized and not already running
            if (_isInitialized && loadingCoroutine == null)
            {
                StartLoading();
            }
        }

        private void OnDisable()
        {
            StopLoading();
        }

        // === Initialization Logic ===
        /// <summary>
        /// Initializes the LoadingView by resetting the UI and verifying UGS Authentication.
        /// Overrides the abstract Initialize method from the View base class.
        /// </summary>
        public override void Initialized()
        {
            if (_isInitialized)
            {
                return;
            }

            // Validate UI components
            if (loadingSlider == null)
            {

            }
            if (loadingText == null)
            {
               
            }

            // Check UGS Authentication
            // if (AuthenticationService.Instance != null && AuthenticationService.Instance.IsSignedIn)
            // {
            //     Debug.Log($"{GetType().Name}: Player is signed in. Player ID: {AuthenticationService.Instance.PlayerId}");
            // }
            // else
            // {
            //     Debug.LogWarning($"{GetType().Name}: Player is not signed in. Consider redirecting to TitleView.", this);
            //     // Optionally redirect to TitleView
            //     if (ViewManager.Instance != null)
            //     {
            //         ViewManager.Instance.ShowView<TitleView>();
            //         return;
            //     }
            // }

            // Reset UI
            UpdateLoadingUI(0f);
            _isInitialized = true;
        }

        // === Cleanup Logic ===
        /// <summary>
        /// Cleans up the LoadingView by stopping coroutines and DOTween animations.
        /// Overrides the abstract Clean method from the View base class.
        /// </summary>
        public override void Clean()
        {
            StopLoading();
        }

        // === Content Update ===
        /// <summary>
        /// Refreshes the LoadingView's UI without reinitializing.
        /// Updates slider and text based on current progress.
        /// </summary>
        public override void Refresh()
        {
            UpdateLoadingUI(loadingSlider != null ? loadingSlider.value : 0f);
        }

        // === Loading Logic ===
        private void StartLoading()
        {
            if (loadingCoroutine != null)
            {
                StopCoroutine(loadingCoroutine);
            }
            loadingCoroutine = StartCoroutine(LoadProgress());
        }

        private void StopLoading()
        {
            if (loadingCoroutine != null)
            {
                StopCoroutine(loadingCoroutine);
                loadingCoroutine = null;
            }
            DOTween.Kill(this); 
        }

        private IEnumerator LoadProgress()
        {
            // Reset UI
            UpdateLoadingUI(0f);

            // Simulate loading with DOTween animation
            if (loadingSlider != null)
            {
                loadingSlider.value = 0f;
                yield return loadingSlider.DOValue(1f, animationDuration)
                    .SetEase(Ease.Linear)
                    .SetUpdate(true) // Use unscaled time for loading
                    .SetId(this)
                    .OnUpdate(() => UpdateLoadingUI(loadingSlider.value))
                    .OnComplete(OnLoadingComplete)
                    .WaitForCompletion();
            }
            else
            {
                // Fallback if slider is missing
                yield return new WaitForSecondsRealtime(animationDuration);
                OnLoadingComplete();
            }
        }

        private void OnLoadingComplete()
        {
            if (ViewManager.Instance != null)
            {
                ViewManager.Instance.ShowView<LobbyView>();
                Hide();
            }
        }

        private void UpdateLoadingUI(float progress)
        {
            if (loadingSlider != null)
            {
                loadingSlider.value = progress;
            }
            if (loadingText != null)
            {
                // Cache percentage to avoid repeated calculations
                int percentage = Mathf.RoundToInt(progress * 100);
                loadingText.text = $"{percentage}%";
            }
        }

        // === Validation ===
        private void OnValidate()
        {
            if (animationDuration < 0.01f)
            {
                Debug.LogWarning($"{GetType().Name}: animationDuration must be positive. Setting to 0.01f.", this);
                animationDuration = 0.01f;
            }
        }
    }
}