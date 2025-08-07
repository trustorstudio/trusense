using TMPro;
using Trusense.Common;
using Trusense.Components.Popups;
using Trusense.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Trusense.Components.Views
{
    /// <summary>
    /// LobbyView represents the lobby screen UI, providing a placeholder for future lobby functionalities.
    /// It inherits from the View class to leverage common view functionality.
    /// 
    /// Author: Nguyễn Duy Khánh
    /// Created: August 6, 2025
    /// Last Modified: August 6, 2025
    /// Version: 1.0
    /// </summary>
    public class LobbyView : View
    {
        // === UI Components Energy ===
        [Header("UI Components Energy")]
        [Tooltip("Button to open the energy UI.")]
        [SerializeField] private Button energyButton;
        [Tooltip("Text to display the current energy.")]
        [SerializeField] private TMP_Text energyText;
        // === UI Components Settings ===
        [Header("UI Components Settings")]
        [Tooltip("Button to open the settings UI.")]
        [SerializeField] private Button settingButton;

        // === UI Components Gold ===
        [Header("UI Components Gold")]
        [Tooltip("Button open to show the shop UI.")]
        [SerializeField] private Button shopButton;
        [Tooltip("Button to open the gold shop UI.")]
        [SerializeField] private Button goldButton;
        [Tooltip("Button to open the diamond shop UI.")]
        [SerializeField] private Button diamondButton;
        [Tooltip("Button to open the energy shop UI.")]



        /// <summary>
        /// Initializes the LobbyView.
        /// Sets up the button listeners and any necessary initial state.
        /// </summary>
        public override void Initialized()
        {
            if (_isInitialized)
            {
                return;
            }

            if (shopButton != null)
            {
                shopButton.onClick.RemoveAllListeners();
                shopButton.onClick.AddListener(HandleShop);
            }
            if (settingButton != null)
            {
                settingButton.onClick.RemoveAllListeners();
                settingButton.onClick.AddListener(HandleSetting);
            }
            
            if (EnergyManager.Current != null)
            {
                EnergyManager.Current.OnEnergyChanged += UpdateEnergy;
            }
        }

        /// <summary>
        /// Cleans up the LobbyView.
        /// Releases resources or resets state as needed.
        /// </summary>
        public override void Clean()
        {
            if (shopButton != null)
            {
                shopButton.onClick.RemoveListener(HandleShop);
            }
            if (settingButton != null)
            {
                settingButton.onClick.RemoveListener(HandleSetting);
            }

            if (EnergyManager.Current != null)
            {
                EnergyManager.Current.OnEnergyChanged -= UpdateEnergy;
            }
        }

        // === Handle Update Energy ===
        /// <summary>
        /// Updates the energy display in the UI.
        /// This method is called when the energy changes, updating the energy text accordingly.
        /// </summary>
        /// <param name="energy"></param>
        private void UpdateEnergy(int energy)
        {
            if (energyText != null)
            {
                energyText.text = energy.ToString() + " / " + EnergyManager.Current.MaxEnergy.ToString();
            }

        }

        /// <summary>
        /// Subscribes to the EnergyManager events when the LobbyView is enabled.
        /// This ensures that the energy display is updated whenever the energy changes.
        /// </summary>
        private void OnEnable()
        {
            // Subscribe to EnergyManager events
            if (EnergyManager.Current != null)
            {
                EnergyManager.Current.OnEnergyChanged += UpdateEnergy;
                UpdateEnergy(EnergyManager.Current.CurrentEnergy);
            }

        }

        /// <summary>
        /// Unsubscribes from the EnergyManager events when the LobbyView is disabled.
        /// This prevents memory leaks and ensures that the view does not receive updates when it is not active.
        /// </summary>
        private void OnDisable()
        {
            if (EnergyManager.Current != null)
            {
                EnergyManager.Current.OnEnergyChanged -= UpdateEnergy;
            }
        }

        // === Handle Button Click Events ===
        /// <summary>
        /// Handles the shop button click event.
        /// This method should contain the logic to open the shop UI.
        /// </summary>
        private void HandleShop()
        {

        }

        private void HandleSetting()
        {
            PopupManager.Current.Show<PopupSetting>();
        }
    }
}