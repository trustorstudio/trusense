using Trusense.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Trusense.Components.Views
{
    public class ShopView : View
    {
        // === UI Components ===
        [Header("UI Components")]
        [Tooltip("Button to open the shop.")]
        [SerializeField] private Button shopButton;

        /// <summary>
        /// Initializes the ShopView.
        /// Sets up the button listeners and any necessary initial state.
        /// </summary>
        public override void Initialized()
        {
            if (_isInitialized) return;
            if (shopButton != null)
                shopButton.onClick.RemoveAllListeners();
            shopButton.onClick.AddListener(OnShopButton);
            _isInitialized = true;
        }

        /// <summary>
        /// Cleans up the ShopView.
        /// Releases resources or resets state as needed.
        /// </summary>
        public override void Clean()
        {
            if (shopButton != null)
                shopButton.onClick.RemoveAllListeners();
        }

        private void OnShopButton()
        {
            // Logic to handle shop button click
            Debug.Log("Shop button clicked.");
        }
    }
}