using DG.Tweening; // Imports DG.Tweening for smooth animation utilities.
using UnityEngine; // Imports core Unity classes like MonoBehaviour and GameObject.
using UnityEngine.UI; // Imports Unity UI classes like Button and RectTransform.
using Trusense.Common; // Imports the View base class.
using TankOfValor.Managers.UI; // Imports PopupManager for integration.
using System.Collections; // Imports for IEnumerator support in HideAsync.

namespace Trusense.Common // Namespace for shared, reusable UI components.
{
    // === Class Header ===
    /// <summary>
    /// Popup class inherits from View, representing a UI popup with animated show/hide behavior.
    /// Manages a popup with a close button and scale animations using DOTween.
    /// Integrates with PopupManager for visibility control.
    /// </summary>
    public class Popup : View
    {
        // === Animation Settings ===
        [Header("Animation Settings")] // Groups animation-related fields in the Unity Inspector.
        [Tooltip("Duration (in seconds) for the popup show animation. Must be positive.")] // Tooltip for showDuration.
        [SerializeField, Min(0.01f)] private float showDuration = 0.5f; // Duration for scaling from 0 to 1.

        [Tooltip("Duration (in seconds) for the popup hide animation. Must be positive.")] // Tooltip for hideDuration.
        [SerializeField, Min(0.01f)] private float hideDuration = 0.5f; // Duration for scaling from 1 to 0.

        [Tooltip("Easing type for the show animation.")] // Tooltip for showEase.
        [SerializeField] private Ease showEase = Ease.OutBack; // Easing for show animation.

        [Tooltip("Easing type for the hide animation.")] // Tooltip for hideEase.
        [SerializeField] private Ease hideEase = Ease.InBack; // Easing for hide animation.

        // === UI Components ===
        [Header("UI Components")] // Groups UI component fields in the Unity Inspector.
        [Tooltip("Reference to the Button that closes the popup.")] // Tooltip for CloseButton.
        [SerializeField] private Button CloseButton; // Reference to the close button.

        [Tooltip("Reference to the RectTransform of the popup for scale animations.")] // Tooltip for PopupTransform.
        [SerializeField] private RectTransform PopupTransform; // RectTransform for animations.

        // === Internal State ===
        private new bool _isInitialized = false; // Tracks initialization state.

        // === Initialization Logic ===
        /// <summary>
        /// Initializes the popup by setting up the close button's click event.
        /// Overrides the abstract Initialize method from the View base class.
        /// </summary>
        [System.Obsolete]
        public override void Initialized()
        {
            if (_isInitialized)
            {
                Debug.LogWarning($"{GetType().Name}: Already initialized, skipping.", this);
                return;
            }

            if (CloseButton != null)
            {
                if (PopupManager.Instance != null)
                {
                    CloseButton.onClick.RemoveAllListeners();
                    CloseButton.onClick.AddListener(() => PopupManager.Instance.HidePopup()); // Calls PopupManager to hide this popup.
                }
                else
                {
                    Debug.LogWarning($"{GetType().Name}: PopupManager instance is null, cannot set up CloseButton.", this);
                }
            }
            else
            {
                Debug.LogWarning($"{GetType().Name}: CloseButton is not assigned in the Inspector.", this);
            }

            _isInitialized = true; // Marks the popup as initialized.
        }

        // === Cleanup Logic ===
        /// <summary>
        /// Cleans up the popup by removing the close button's listeners and stopping animations.
        /// Overrides the abstract Clean method from the View base class.
        /// </summary>
        public override void Clean()
        {
            if (CloseButton != null)
            {
                CloseButton.onClick.RemoveAllListeners(); // Removes listeners to prevent memory leaks.
            }

            if (PopupTransform != null)
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

            if (_isVisible) return; // Skip if already visible (inherited from View).

            if (PopupTransform == null)
            {
                Debug.LogError($"{GetType().Name}: PopupTransform is not assigned in the Inspector.", this);
                gameObject.SetActive(true); // Activate GameObject for visibility.
                _isVisible = true; // Update visibility state.
                // OnShown?.Invoke(); // Notify listeners after state change (valid in Popup).
                return;
            }

            DOTween.Kill(this); // Stops any existing animations to avoid conflicts.
            gameObject.SetActive(true); // Activates GameObject for visibility.
            PopupTransform.localScale = Vector3.zero; // Sets initial scale for animation.
            PopupTransform.DOScale(Vector3.one, showDuration) // Animates to full scale.
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
            if (PopupTransform == null)
            {
                gameObject.SetActive(false); 
                _isVisible = false;
                // OnHidden?.Invoke(); // Notify listeners after state change (valid in Popup).
                return;
            }

            DOTween.Kill(this); 
            PopupTransform.DOScale(Vector3.zero, hideDuration) 
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

            if (PopupTransform == null)
            {
                Debug.LogError($"{GetType().Name}: PopupTransform is not assigned in the Inspector.", this);
                gameObject.SetActive(false); // Deactivate GameObject.
                _isVisible = false; // Update visibility state.
                // OnHidden?.Invoke(); // Notify listeners (valid in Popup).
                yield break;
            }

            DOTween.Kill(this); // Stops any existing animations.
            yield return PopupTransform.DOScale(Vector3.zero, hideDuration) // Animates to zero scale.
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