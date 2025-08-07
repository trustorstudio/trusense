using UnityEngine;
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
    public class Flag : View
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
        [SerializeField] private string language;



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

        public override void Clean()
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
        /// Initializes the Flag component.
        /// This method can be overridden to set up specific settings for the flag.
        /// </summary>
        public override void Initialized()
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
        }


        private void HandleSelect()
        {
            isSelected = !isSelected;
            checkIcon.gameObject.SetActive(isSelected); // Hiển thị/ẩn Icon Check
            SaveState();
        }


        public void SaveState()
        {
            // Lưu trạng thái chọn (bool) vào PlayerPrefs
            PlayerPrefs.SetInt($"Flag_{languageName}_Selected", isSelected ? 1 : 0);

            // Lưu tên Sprite (nếu có)
            if (flagSprite != null)
                PlayerPrefs.SetString($"Flag_{languageName}_Sprite", flagSprite.name);

            PlayerPrefs.Save(); // Lưu thay đổi
        }

        public void LoadState()
        {
            // Tải trạng thái chọn từ PlayerPrefs
            isSelected = PlayerPrefs.GetInt($"Flag_{languageName}_Selected", 0) == 1;
            checkIcon.gameObject.SetActive(isSelected); // Cập nhật giao diện

            // (Tùy chọn) Kiểm tra tên Sprite (nếu cần khôi phục)
            string savedSpriteName = PlayerPrefs.GetString($"Flag_{languageName}_Sprite", "");
            if (!string.IsNullOrEmpty(savedSpriteName) && flagSprite != null && flagSprite.name != savedSpriteName)
            {
                Debug.LogWarning($"Sprite cho {languageName} không khớp: {savedSpriteName}");
            }
        }

        // (Tùy chọn) Hàm để đặt trạng thái từ bên ngoài (dùng cho single-selection)
        public void SetSelected(bool selected)
        {
            isSelected = selected;
            checkIcon.gameObject.SetActive(isSelected);
            SaveState();
        }
    }
}