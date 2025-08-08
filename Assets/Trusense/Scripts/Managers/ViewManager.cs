using System;
using UnityEngine;
using Trusense.Common;
using Trusense.Tools;
using System.Collections.Generic;

namespace Trusense.Managers
{

    /// <summary>
    /// Singleton manager for handling UI views.
    /// Manages initialization, visibility, and navigation history with optimized view lookup.
    /// </summary>
    public class ViewManager : Singleton<ViewManager>
    {

        [Header("View Configuration")]
        [Tooltip("Array of all views managed by this ViewManager.")]
        [SerializeField] private View[] views;

        [Tooltip("The initial view to show when the ViewManager starts.")]
        [SerializeField] private View startView;
        [Header("Internal State")]
        [Tooltip("Stack of previously shown views for navigation history.")]
        [SerializeField] private Stack<View> historiesView = new Stack<View>();

        private View currentView;
        private readonly Dictionary<Type, View> viewCache = new Dictionary<Type, View>();

        /// <summary>
        /// Event triggered when the current view changes.
        /// </summary>
        public event Action<View> OnViewChanged;

        /// <summary>
        /// Initializes all views, builds the view cache, and shows the start view.
        /// </summary>
        private void Start()
        {
            if (views == null || views.Length == 0)
            {
                return;
            }

            viewCache.Clear();
            foreach (var view in views)
            {
                if (view == null)
                {
                    continue;
                }
                if (viewCache.ContainsKey(view.GetType()))
                {
                    continue;
                }
                viewCache[view.GetType()] = view;
                view.Initialized();
                view.Hide();
            }

            if (startView != null)
            {
                Show(startView);
            }
        }

        /// <summary>
        /// Retrieves a view of the specified type from the cache.
        /// </summary>
        /// <typeparam name="T">The type of view to retrieve.</typeparam>
        /// <returns>The view of type T, or null if not found.</returns>
        public T GetView<T>() where T : View
        {
            if (this == null) return null;
            if (viewCache.TryGetValue(typeof(T), out View view))
            {
                return view as T;
            }
            return null;
        }

        /// <summary>
        /// Shows a view of the specified type, optionally adding the current view to history.
        /// </summary>
        /// <typeparam name="T">The type of view to show.</typeparam>
        /// <param name="remember">Whether to add the current view to history.</param>
        public void Show<T>(bool remember = true) where T : View
        {
            T view = GetView<T>();
            if (view != null)
            {
                Show(view, remember);

            }
        }

        /// <summary>
        /// Shows the specified view, optionally adding the current view to history.
        /// </summary>
        /// <param name="view">The view to show.</param>
        /// <param name="remember">Whether to add the current view to history.</param>
        public void Show(View view, bool remember = true)
        {
            if (view == null)
            {
                Debug.LogWarning("Attempted to show a null view.", this);
                return;
            }

            PopupManager.Instance.HideAllPopups();

            if (currentView != null && currentView != view)
            {
                if (remember)
                {
                    historiesView.Push(currentView);
                }
                currentView.Hide();
            }

            view.Show();
            currentView = view;
            OnViewChanged?.Invoke(view);
        }

        /// <summary>
        /// Shows the last view in the history stack, if available.
        /// </summary>
        public void ShowLast()
        {
            if (historiesView.Count == 0)
            {
                return;
            }

            View lastView = historiesView.Pop();
            if (lastView != null)
            {
                Show(lastView, false);
            }
        }

        /// <summary>
        /// Cleans up all views and clears history.
        /// </summary>
        public void Reset()
        {
            foreach (View view in viewCache.Values)
            {
                view?.Clean();
                view?.Hide();
            }
            historiesView.Clear();
            currentView = null;
            viewCache.Clear();
        }

        /// <summary>
        /// Validates the views array in the Inspector to ensure no duplicates or nulls.
        /// </summary>
        private void OnValidate()
        {
            if (views == null) return;
            HashSet<View> uniqueViews = new HashSet<View>();
            for (int i = 0; i < views.Length; i++)
            {
                if (views[i] == null)
                {
                    Debug.LogWarning($"{GetType().Name}: Null view at index {i} in views array.", this);
                }
                else if (!uniqueViews.Add(views[i]))
                {
                    Debug.LogWarning($"{GetType().Name}: Duplicate view {views[i].name} at index {i}.", this);
                }
            }
        }
    }
}