using Trusense.Constants;
using Trusense.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Trusense.Common
{
    /// <summary>
    /// Flag is a simple MonoBehaviour class that serves as a marker or tag.
    /// It can be used to identify GameObjects in the Trusense application that require special
    /// handling or categorization.
    /// 
    /// Author: Nguyễn Duy Khánh
    /// Created: August 6, 2025 
    /// Last Modified: August 6, 2025
    /// Version: 1.0.0
    /// </summary>
    public class Flag : MonoBehaviour
    {
        // === UI Components ===
        [Header("UI Components")]
        [Tooltip("Optional icon checked to represent the flag visually.")]
        [SerializeField] private Image checkIcon;
        [Tooltip("Optional image to represent the flag visually.")]
        [SerializeField] private Image flagImage;
        [Tooltip("Button to handle selection of the flag.")]
        [SerializeField] private Button buttonLanguage;
        [Tooltip("The language associated with the flag, used for localization.")]
        [SerializeField] private string language = "English";

        // === Events ===
        [Header("Events")]
        [Tooltip("Optional popup to display additional information or options related to the flag.")]
        public UnityEvent<Flag> OnFlagSelected = new UnityEvent<Flag>();


        // === Properties ===
        private Sprite flagSprite;
        private bool isSelected = false;


        /// <summary>
        /// Gets or sets the selection state of the flag.
        /// This property can be used to determine if the flag is currently selected.
        /// </summary>
        public string Language
        {
            get => language;
            set
            {
                language = value;
                if (checkIcon != null)
                {
                    checkIcon.sprite = Resources.Load<Sprite>($"Flags/{language}");
                }
            }
        }

        /// <summary>
        /// Gets or sets the sprite for the flag.
        /// This property can be used to change the visual representation of the flag.
        /// </summary>
        public Sprite FlagSprite
        {
            get => flagSprite;
            set
            {
                flagSprite = value;
                if (flagImage != null)
                {
                    flagImage.sprite = flagSprite;
                }
            }
        }

        /// <summary>
        /// Initializes the Flag component.
        /// This method can be overridden to set up specific settings for the flag.
        /// </summary>
        public void Start()
        {
            if (flagImage != null)
            {
                flagSprite = flagImage.sprite;
            }
            if (checkIcon != null)
            {
                checkIcon.gameObject.SetActive(false);
            }
            if (buttonLanguage != null)
            {
                buttonLanguage.onClick.RemoveAllListeners();
                buttonLanguage.onClick.AddListener(HandleSelect);
            }
            Load();
        }

        /// <summary>
        /// Cleans up resources when the Flag component is destroyed.
        /// This method is called when the GameObject is destroyed or when the component is removed.
        /// </summary>
        private void OnDestroy()
        {
            if (checkIcon != null)
            {
                checkIcon.gameObject.SetActive(false);
            }
            if (buttonLanguage != null)
            {
                buttonLanguage.onClick.RemoveListener(HandleSelect);
            }
        }

        /// <summary>
        /// Handles the selection of the flag.
        /// This method is called when the button associated with the flag is clicked.  
        /// </summary>
        private void HandleSelect()
        {
            OnFlagSelected.Invoke(this);
        }

        /// <summary>
        /// Saves the current selection state of the flag to PlayerPrefs.
        /// This method is called to persist the selected language in PlayerPrefs.
        /// </summary>
        public void Save()
        {
            if (isSelected)
            {
                LanguageManager.Current.SetLanguage(language);
            }
            else if (LanguageManager.Current.GetLanguage() == language)
            {
                LanguageManager.Current.ClearLanguage();
            }
        }

        /// <summary>
        /// Loads the selection state of the flag from PlayerPrefs. 
        /// This method is called to initialize the flag's selection state based on previously saved preferences.
        /// </summary>
        public void Load()
        {
            isSelected = LanguageManager.Current.GetLanguage() == language;
            if (checkIcon != null)
            {
                checkIcon.gameObject.SetActive(isSelected);
            }
        }

        /// <summary>
        /// Sets the selection state of the flag.
        /// This method updates the selection state of the flag and saves it to PlayerPrefs.    
        /// </summary>
        /// <param name="selected"></param>
        public void SetSelected(bool selected)
        {
            isSelected = selected;
            if (checkIcon != null)
            {
                checkIcon.gameObject.SetActive(isSelected);
            }
            Save();
        }
    }
}