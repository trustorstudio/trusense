using DG.Tweening;
using TMPro;
using Trusense.Common;
using Trusense.Constants;
using Trusense.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Trusense.Components.Popups
{
    /// <summary>
    /// PopupSetting is a class that inherits from Popup.
    /// It is used to define settings for popups in the Trusense application.
    /// This class can be extended to include specific settings or behaviors for popups.
    /// 
    /// Author: Nguyễn Duy Khánh
    /// Created: August 6, 2025
    /// Last Modified: August 6, 2025
    /// Version: 1.0.1
    /// </summary>
    public class PopupSetting : Popup
    {
        // === Animation Settings ===
        [Header("Animation Settings")]
        [Tooltip("Duration of the handle movement animation in seconds.")]
        [SerializeField] private float moveDuration = 0.3f;
        [Tooltip("Easing type for the handle movement animation.")]
        [SerializeField] private Ease moveEase = Ease.InOutQuad;
        // === UI Sprite Components ===
        [Header("UI Sprite Components")]
        [Tooltip("Color for the active background of the popup.")]
        [SerializeField] private Sprite activeBackgroud;
        [Tooltip("Color for the inactive background of the popup.")]
        [SerializeField] private Sprite inactiveBackground;
        [Tooltip("Color for the active handle of the popup.")]
        [SerializeField] private Sprite activeHandle;
        [Tooltip("Color for the inactive handle of the popup.")]
        [SerializeField] private Sprite inactiveHandle;

        // === UI Button Components ===
        [Header("UI Language Components")]
        [Tooltip("Image component to visually represent the language setting.")]
        [SerializeField] private Image languageImage;
        [Tooltip("The button that opens the language settings.")]
        [SerializeField] private Button languageButton;
        [Tooltip("Text component to display the current language setting.")]
        [SerializeField] private TMP_Text languageText;

        // === UI Vibration Components ===
        [Header("UI Vibration Components")]
        [Tooltip("Button to toggle vibration settings.")]
        [SerializeField] private Toggle vibrationToggle;
        [Tooltip("Image background to display the vibration setting.")]
        [SerializeField] private Image vibrationBackground;
        [Tooltip("Image handle to display the vibration setting.")]
        [SerializeField] private Image vibrationHandle;

        // === UI Push Alam Components ===
        [Header("UI Push Alarm Components")]
        [Tooltip("Button to toggle push alarm settings.")]
        [SerializeField] private Toggle pushAlarmToggle;
        [Tooltip("Image background to display the pushAlarm setting.")]
        [SerializeField] private Image pushAlarmBackground;
        [Tooltip("Image handle to display the pushAlarm setting.")]
        [SerializeField] private Image pushAlarmHandle;

        // === UI Sound FX Components ===
        [Header("UI Sound FX Components")]
        [Tooltip("Slider to sound fx settings ")]
        [SerializeField] private Slider soundFxSlider;
        [Tooltip("Image handle display sound fx settings")]
        [SerializeField] private Image soundFxImage;
        [Tooltip("Sprite handle in active sound Fx settings")]
        [SerializeField] private Sprite soundFxActive;
        [Tooltip("Sprite handle active sound Fx settings")]
        [SerializeField] private Sprite soundFxInactive;

        // === UI Sound Music Components ===
        [Header("UI Sound Music Components")]
        [Tooltip("Slider to sound Music settings ")]
        [SerializeField] private Slider soundMusicSlider;
        [Tooltip("Image handle display sound Music settings")]
        [SerializeField] private Image soundMusicImage;
        [Tooltip("Sprite handle in active sound Music settings")]
        [SerializeField] private Sprite soundMusicActive;
        [Tooltip("Sprite handle active sound music settings")]
        [SerializeField] private Sprite soundMusicInactive;

        /// <summary>
        /// Initializes the popup settings.
        /// This method can be overridden to set up specific settings for the popup.
        /// </summary>
        public override void Initialized()
        {
            if (_isInitialized) return;
            base.Initialized();
            UpdateLanguage(LanguageManager.Current.GetLanguage());
            if (languageButton != null)
            {
                languageButton.onClick.RemoveAllListeners();
                languageButton.onClick.AddListener(HandleLanguage);
            }
            if (vibrationToggle != null)
            {
                bool vibrationEnabled = PlayerPrefs.GetInt(Keys.VIBRATION, 1) == 1;
                vibrationToggle.isOn = vibrationEnabled;
                OnVibration(vibrationEnabled);
                vibrationToggle.onValueChanged.AddListener(OnVibration);
            }
            if (pushAlarmToggle != null)
            {
                bool pushAlarmEnabled = PlayerPrefs.GetInt(Keys.PUSH_ALARM, 1) == 1;
                pushAlarmToggle.isOn = pushAlarmEnabled;
                OnPushAlarm(pushAlarmEnabled);
                pushAlarmToggle.onValueChanged.AddListener(OnPushAlarm);
            }
            EventManager.Current.OnLanguageChanged.RemoveListener(UpdateLanguage);
            EventManager.Current.OnLanguageChanged.AddListener(UpdateLanguage);

        }

        /// <summary>
        /// Cleans up resources when the popup is closed or destroyed.
        /// This method can be overridden to handle cleanup specific to the popup settings.
        /// </summary>
        public override void Clean()
        {
            if (languageButton != null)
            {
                languageButton.onClick.RemoveListener(HandleLanguage);
            }

            if (vibrationToggle != null)
            {
                vibrationToggle.onValueChanged.RemoveListener(OnVibration);
            }
            if (pushAlarmToggle != null)
            {
                pushAlarmToggle.onValueChanged.RemoveListener(OnPushAlarm);
            }
            if (EventManager.Current != null)
            {
                EventManager.Current.OnLanguageChanged.RemoveListener(UpdateLanguage);
            }

            DOTween.Kill(vibrationHandle);
            DOTween.Kill(pushAlarmHandle);
            base.Clean();
        }

        /// <summary>
        /// Handles the click event for the language settings button.
        /// This method can be extended to implement specific logic for changing language settings.
        /// </summary>
        private void HandleLanguage()
        {
            PopupManager.Current.Show<PopupSettingLanguage>();
        }


        /// <summary>
        /// Updates the language settings in the popup.
        /// This method can be called to refresh the language display based on the current settings.
        /// </summary>
        /// <param name="language"></param>
        private void UpdateLanguage(string language = "English")
        {

            if (string.IsNullOrEmpty(language))
            {
                language = "English";
            }

            if (languageImage != null)
            {
                Sprite sprite = Resources.Load<Sprite>($"Flags/{language}");
                languageImage.sprite = sprite != null ? sprite : Resources.Load<Sprite>("Flags/english");
            }

            if (languageText != null)
            {
                languageText.text = language.ToUpper();
            }
        }
        /// <summary>
        /// Handles the toggle change event for vibration settings.
        /// This method updates the vibration state in PlayerPrefs and applies the changes.
        /// </summary>
        /// <param name="isOn"></param>
        private void OnVibration(bool isOn)
        {
            if (vibrationBackground != null)
                vibrationBackground.sprite = isOn ? activeBackgroud : inactiveBackground;
            if (vibrationHandle != null)
            {
                vibrationHandle.sprite = isOn ? activeHandle : inactiveHandle;
                Vector3 currentPos = vibrationHandle.rectTransform.anchoredPosition;
                float newX = isOn ? -Mathf.Abs(currentPos.x) : Mathf.Abs(currentPos.x);
                vibrationHandle.rectTransform.DOAnchorPosX(newX, moveDuration).SetEase(moveEase);
            }

            PlayerPrefs.SetInt(Keys.VIBRATION, isOn ? 1 : 0);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Handles the toggle change event for push alarm settings.
        /// This method updates the push alarm state in PlayerPrefs and applies the changes.
        /// </summary>
        /// <param name="isOn"></param>
        private void OnPushAlarm(bool isOn)
        {
            if (pushAlarmBackground != null)
                pushAlarmBackground.sprite = isOn ? activeBackgroud : inactiveBackground;
            if (pushAlarmHandle != null)
            {
                pushAlarmHandle.sprite = isOn ? activeHandle : inactiveHandle;
                Vector3 currentPos = pushAlarmHandle.rectTransform.anchoredPosition;
                float newX = isOn ? -Mathf.Abs(currentPos.x) : Mathf.Abs(currentPos.x);
                pushAlarmHandle.rectTransform.DOAnchorPosX(newX, moveDuration).SetEase(moveEase);
            }
            PlayerPrefs.SetInt(Keys.PUSH_ALARM, isOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}