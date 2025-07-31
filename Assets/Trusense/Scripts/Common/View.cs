using UnityEngine;

namespace Trusense.Common
{
    /// <summary>
    /// Abstract base class for UI or visual components, inheriting from MonoBehaviour.
    /// Enforces a consistent interface for initialization, cleanup, and visibility management.
    /// </summary>
    public abstract class View : MonoBehaviour
    {
        // === Configuration ===
        [Header("View Configuration")] // Groups configuration fields in the Unity Inspector.
        [Tooltip("If true, the view initializes automatically on Awake.")] // Tooltip for autoInitialize.
        [SerializeField] protected bool autoInitialize = true; // Controls automatic initialization.

        [Tooltip("If true, the view is hidden by default on Start.")] // Tooltip for startHidden.
        [SerializeField] protected bool startHidden = true; // Controls default visibility.

        // === Internal State ===
        protected bool _isInitialized = false; // Tracks initialization state.
        protected bool _isVisible = false; // Tracks visibility state.

        // === Events ===
        /// <summary>
        /// Event triggered when the view is shown.
        /// </summary>
        public event System.Action OnShown;

        /// <summary>
        /// Event triggered when the view is hidden.
        /// </summary>
        public event System.Action OnHidden;

        // === Lifecycle Methods ===
        /// <summary>
        /// Abstract method to clean up the view.
        /// Derived classes must implement this to release resources, unregister events, or reset state.
        /// </summary>
        public abstract void Clean();

        /// <summary>
        /// Abstract method to initialize the view.
        /// Derived classes must implement this to set up references, load initial data, or perform setup tasks.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Virtual method to refresh the view's content without reinitializing.
        /// Derived classes can override this to update UI or data.
        /// </summary>
        public virtual void Refresh() { }

        // === Visibility Methods ===
        /// <summary>
        /// Hides the view by deactivating its GameObject.
        /// Can be overridden for custom hiding logic (e.g., animations).
        /// </summary>
        public virtual void Hide()
        {
            if (!_isVisible) return; // Skip if already hidden.
            this.gameObject.SetActive(false);
            _isVisible = false;
            OnHidden?.Invoke(); // Notify listeners.
        }

        /// <summary>
        /// Shows the view by activating its GameObject.
        /// Ensures the view is initialized before showing.
        /// Can be overridden for custom showing logic (e.g., animations).
        /// </summary>
        public virtual void Show()
        {
            if (!_isInitialized)
            {
                Initialize();
                _isInitialized = true;
            }
            if (_isVisible) return; // Skip if already visible.
            this.gameObject.SetActive(true);
            _isVisible = true;
            OnShown?.Invoke(); // Notify listeners.
        }

        // === Unity Lifecycle ===
        /// <summary>
        /// Automatically initializes the view if autoInitialize is enabled.
        /// </summary>
        protected virtual void Awake()
        {
            if (autoInitialize)
            {
                Initialize();
                _isInitialized = true;
            }
        }

        /// <summary>
        /// Sets the default visibility state if startHidden is enabled.
        /// </summary>
        protected virtual void Start()
        {
            if (startHidden)
            {
                Hide();
            }
        }

        /// <summary>
        /// Cleans up the view when the GameObject is destroyed.
        /// Handles errors in derived Clean implementations.
        /// </summary>
        protected virtual void OnDestroy()
        {
            try
            {
                Clean();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Cleanup failed in {GetType().Name}: {ex.Message}", this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual bool IsVisible()
        {
            return this.gameObject.activeSelf;
        }
    }
}