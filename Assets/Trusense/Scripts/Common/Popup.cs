using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Trusense.Managers;

namespace Trusense.Common
{
    // === Class Header ===
    /// <summary>
    /// Popup class inherits from View, representing a UI popup with animated show/hide behavior.
    /// Manages a popup with a close button and scale animations using DOTween.
    /// Integrates with PopupManager for visibility control.
    /// 
    /// Author: Nguyễn Duy Khánh
    /// Created: August 6, 2025
    /// Last Modified: August 6, 2025
    /// Version: 1.0.0
    /// </summary>
    public class Popup : View
    {
        // === Animation Settings ===
        [Header("Animation Settings")]
        [Tooltip("Duration (in seconds) for the popup show animation. Must be positive.")]
        [SerializeField, Min(0.01f)] private float showDuration = 0.5f;

        [Tooltip("Duration (in seconds) for the popup hide animation. Must be positive.")]
        [SerializeField, Min(0.01f)] private float hideDuration = 0.5f;

        [Tooltip("Easing type for the show animation.")]
        [SerializeField] private Ease showEase = Ease.OutBack;

        [Tooltip("Easing type for the hide animation.")]
        [SerializeField] private Ease hideEase = Ease.InBack;

        // === UI Components ===
        [Header("UI Components")]
        [Tooltip("Reference to the Button that closes the popup.")]
        [SerializeField] private Button closeButton;

        [Tooltip("Reference to the RectTransform of the popup for scale animations.")]
        [SerializeField] private RectTransform popupTransform;

        // === Internal State ===
        private new bool _isInitialized = false;

        // === Initialization Logic ===
        /// <summary>
        /// Initializes the popup by setting up the close button's click event.
        /// Overrides the abstract Initialize method from the View base class.
        /// </summary>
        public override void Initialized()
        {
            if (_isInitialized)
            {
                return;
            }

            if (closeButton != null)
            {
                if (PopupManager.Current != null)
                {
                    closeButton.onClick.RemoveAllListeners();
                    closeButton.onClick.AddListener(() => PopupManager.Current.HidePopup());
                }
            }

            _isInitialized = true;
        }

        // === Cleanup Logic ===
        /// <summary>
        /// Cleans up the popup by removing the close button's listeners and stopping animations.
        /// Overrides the abstract Clean method from the View base class.
        /// </summary>
        public override void Clean()
        {
            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners(); // Removes listeners to prevent memory leaks.
            }

            if (popupTransform != null)
            {
                DOTween.Kill(this); // Stops only animations tagged with this instance.
            }
        }

        // === Content Update ===
        /// <summary>
        /// Refreshes the popup's content without reinitializing.
        /// Override in derived classes to update UI elements (e.g., text, images).
        /// </summary>
        public override void Refresh()
        {
            // Default implementation empty; override in derived classes.
        }

        // === Show Logic ===
        /// <summary>
        /// Shows the popup with a scale animation using DOTween.
        /// Overrides the virtual Show method from the View base class.
        /// </summary>
        public override void Show()
        {
            if (!_isInitialized)
            {
                Initialized();
            }

            if (_isVisible) return;

            if (popupTransform == null)
            {
                gameObject.SetActive(true);
                _isVisible = true;
                // OnShown?.Invoke(); // Notify listeners after state change (valid in Popup).
                return;
            }

            DOTween.Kill(this); // Stops any existing animations to avoid conflicts.
            gameObject.SetActive(true); // Activates GameObject for visibility.
            popupTransform.localScale = Vector3.zero; // Sets initial scale for animation.
            popupTransform.DOScale(Vector3.one, showDuration) // Animates to full scale.
                .SetEase(showEase) // Applies custom easing.
                .SetId(this) // Tags animation for cleanup.
                .OnComplete(() =>
                {
                    _isVisible = true; // Update visibility state after animation.
                    // OnShown?.Invoke(); // Notify after animation completes (valid in Popup).
                });
        }

        // === Hide Logic ===
        /// <summary>
        /// Hides the popup with a scale animation using DOTween.
        /// Overrides the virtual Hide method from the View base class.
        /// </summary>
        public override void Hide()
        {
            if (!_isVisible) return;
            if (popupTransform == null)
            {
                gameObject.SetActive(false);
                _isVisible = false;
                // OnHidden?.Invoke(); // Notify listeners after state change (valid in Popup).
                return;
            }

            DOTween.Kill(this);
            popupTransform.DOScale(Vector3.zero, hideDuration)
                .SetEase(hideEase)
                .SetId(this)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false); // Deactivates after animation.
                    _isVisible = false; // Update visibility state.
                    // OnHidden?.Invoke(); // Notify after animation completes (valid in Popup).
                });
        }

        // === Async Hide ===
        /// <summary>
        /// Hides the popup asynchronously, waiting for the animation to complete.
        /// Used by PopupManager for synchronized hiding.
        /// </summary>
        public IEnumerator HideAsync()
        {
            if (!_isVisible) yield break; // Skip if already hidden.

            if (popupTransform == null)
            {
                gameObject.SetActive(false); // Deactivate GameObject.
                _isVisible = false; // Update visibility state.
                // OnHidden?.Invoke(); // Notify listeners (valid in Popup).
                yield break;
            }

            DOTween.Kill(this); // Stops any existing animations.
            yield return popupTransform.DOScale(Vector3.zero, hideDuration) // Animates to zero scale.
                .SetEase(hideEase) // Applies custom easing.
                .SetId(this) // Tags animation for cleanup.
                .WaitForCompletion();
            gameObject.SetActive(false); // Deactivates after animation.
            _isVisible = false; // Update visibility state.
            // OnHidden?.Invoke(); // Notify after animation completes (valid in Popup).
        }



        // === Validation ===
        /// <summary>
        /// Validates animation durations in the Inspector to ensure they are positive.
        /// </summary>
        private void OnValidate()
        {
            if (showDuration < 0.01f)
            {
                Debug.LogWarning($"{GetType().Name}: showDuration must be positive. Setting to 0.01f.", this);
                showDuration = 0.01f;
            }
            if (hideDuration < 0.01f)
            {
                Debug.LogWarning($"{GetType().Name}: hideDuration must be positive. Setting to 0.01f.", this);
                hideDuration = 0.01f;
            }
        }
    }
}