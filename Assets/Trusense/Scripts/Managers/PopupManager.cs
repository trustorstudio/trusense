using System;
using UnityEngine;
using Trusense.Common;
using Trusense.Tools;
using System.Collections.Generic;


namespace Trusense.Managers
{
    /// <summary>
    /// Singleton manager for handling UI popups.
    /// Manages initialization, visibility, and navigation history for popups with optimized lookup and error handling.
    /// </summary>
    public class PopupManager : Singleton<PopupManager>
    {
        [Header("Popup Configuration")]
        [Tooltip("Array of all popups managed by this PopupManager.")]
        [SerializeField] private Popup[] popups;


        [Header("Internal State")]
        [Tooltip("Stack of previously shown popups for navigation history.")]
        [SerializeField] private Stack<Popup> HistoryPopups = new Stack<Popup>();

        private Popup CurrentPopup;
        private Dictionary<Type, Popup> popupCache = new Dictionary<Type, Popup>();


        /// <summary>
        /// Event triggered when the current popup changes.
        /// </summary>
        public event Action<Popup> OnPopupChanged;

        // === Initialization Logic ===
        /// <summary>
        /// Initializes all popups and builds the popup cache on startup.
        /// </summary>
        protected void Start()
        {
            Initialized();
        }

        private void Initialized()
        {
            if (popups == null || popups.Length == 0)
            {
                return;
            }

            popupCache.Clear();
            foreach (Popup popup in popups)
            {
                if (popup == null)
                {
                    continue;
                }
                if (popupCache.ContainsKey(popup.GetType()))
                {
                    continue;
                }
                popupCache[popup.GetType()] = popup;
                popup.Initialized();
                popup.Hide();
            }
        }


        /// <summary>
        /// Retrieves a popup of the specified type from the cache.
        /// </summary>
        /// <typeparam name="Template">The type of popup to retrieve.</typeparam>
        /// <returns>The popup of type Template, or null if not found.</returns>
        public Template GetPopup<Template>() where Template : Popup
        {
            if (popupCache.TryGetValue(typeof(Template), out Popup popup))
            {
                return popup as Template;
            }
            return null;
        }


        /// <summary>
        /// Shows a popup of the specified type, optionally adding the current popup to history.
        /// </summary>
        /// <typeparam name="Template">The type of popup to show.</typeparam>
        /// <param name="remember">Whether to add the current popup to history.</param>
        /// <param name="hide">Whether to hide the current popup.</param>
        public void ShowPopup<Template>(bool remember = true, bool hide = false) where Template : Popup
        {
            Template popup = GetPopup<Template>();
            if (popup != null)
            {
                ShowPopup(popup, remember, hide);
            }
        }

        /// <summary>
        /// Shows the specified popup, optionally adding the current popup to history.
        /// </summary>
        /// <param name="popup">The popup to show.</param>
        /// <param name="remember">Whether to add the current popup to history.</param>
        /// <param name="hide">Whether to hide the current popup.</param>
        public void ShowPopup(Popup popup, bool remember = true, bool hide = false)
        {
            if (popup == null)
            {
                return;
            }

            if (CurrentPopup != null && CurrentPopup != popup)
            {
                if (remember)
                {
                    HistoryPopups.Push(CurrentPopup);
                }
                if (hide)
                {
                    CurrentPopup.Hide();
                }
            }

            popup.Show();
            CurrentPopup = popup;
            OnPopupChanged?.Invoke(popup);
        }

        /// <summary>
        /// Hides the current popup and shows the last popup in history, if available.
        /// </summary>
        public void HidePopup()
        {
            if (CurrentPopup == null)
            {
                Debug.LogWarning($"{GetType().Name}: No current popup to hide.");
                return;
            }

            CurrentPopup.Hide();
            Popup previousPopup = CurrentPopup;
            CurrentPopup = null;

            if (HistoryPopups.Count > 0)
            {
                Popup lastPopup = HistoryPopups.Pop();
                if (lastPopup != null)
                {
                    ShowPopup(lastPopup, false);
                }


            }

            OnPopupChanged?.Invoke(null);
        }

        // === Full Cleanup ===
        /// <summary>
        /// Hides all popups, clears history, and resets the manager.
        /// </summary>
        public void HideAllPopups()
        {
            if (CurrentPopup != null)
            {
                CurrentPopup.Hide();
                CurrentPopup = null;
            }

            foreach (Popup popup in HistoryPopups)
            {
                if (popup != null && popup.gameObject.activeSelf)
                {
                    popup.Hide();
                }
            }
            HistoryPopups.Clear();
            OnPopupChanged?.Invoke(null);
        }


        /// <summary>
        /// Validates the Popups array in the Inspector to ensure no duplicates or nulls.
        /// </summary>
        private void OnValidate()
        {
            if (popups == null) return;
            HashSet<Popup> uniquePopups = new HashSet<Popup>();
            foreach (Popup popup in popups)
            {
                if (popup == null)
                {
                    Debug.LogWarning($"{GetType().Name}: Null popup at index in Popups array.", this);
                }
                else if (!uniquePopups.Add(popup))
                {
                    Debug.LogWarning($"{GetType().Name}: Duplicate popup {popup.name}.", this);
                }
            }
        }
    }
}