using UnityEngine;
using Trusense.Common;
using System.Collections.Generic;
using System;

namespace Trusense.Components.Popups
{
    /// <summary>
    /// PopupSettingLanguage is a class that inherits from Popup.
    /// It is used to define settings for language selection in the Trusense application.
    /// This class can be extended to include specific settings or behaviors for language popups.
    /// 
    /// Author: Nguyễn Duy Khánh
    /// Created: August 6, 2025
    /// Last Modified: August 6, 2025
    /// Version: 1.0.1
    /// </summary>
    public class PopupSettingLanguage : Popup
    {
        // === UI Components ===
        [Header("UI Components")]
        [Tooltip("Button to change the language settings.")]
        [SerializeField] private List<Flag> flags;

        /// <summary>
        /// Initializes the popup settings for language selection.
        /// This method can be overridden to set up specific settings for the language popup.
        /// </summary>
        public override void Initialized()
        {
            if (_isInitialized) return;
            base.Initialized();

            if (flags == null || flags.Count == 0)
            {
                return;
            }

            foreach (var flag in flags)
            {
                flag.OnFlagSelected.RemoveAllListeners();
                flag.OnFlagSelected.AddListener(SelectFlag);
                flag.Load();
            }
        }

        /// <summary>
        /// Cleans up resources when the popup is closed or destroyed.
        /// This method can be overridden to handle cleanup specific to the language popup.
        /// </summary>
        public override void Clean()
        {
            foreach (var flag in flags)
            {
                flag.Save();
                flag.OnFlagSelected.RemoveAllListeners();
            }
            base.Clean();
        }

        /// <summary>
        /// Selects a flag and updates the selection state.
        /// This method is called when a flag is selected, and it updates the selection state of the flags.
        /// It also saves the selected language to PlayerPrefs.
        /// </summary>
        /// <param name="selectedFlag"></param>
        public void SelectFlag(Flag selectedFlag)
        {
            foreach (var flag in flags)
            {
                flag.SetSelected(flag == selectedFlag);
            }
        }
    }
}