using Trusense.Constants;
using Trusense.Tools;
using UnityEngine;

namespace Trusense.Managers
{
    /// <summary>
    /// LanguageManager is a singleton class that manages the current language settings
    /// for the Trusense application. It inherits from Singleton<LanguageManager>.
    /// 
    /// Author: Nguyễn Duy Khánh
    /// Created: August 6, 2025
    /// Last Modified: August 6, 2025
    /// Version: 1.0.0
    /// </summary>
    public class LanguageManager : Singleton<LanguageManager>
    {

        /// <summary>
        /// Sets the current language for the application.
        /// This method updates the PlayerPrefs with the selected language
        /// </summary>
        /// <param name="language"></param>
        public void SetLanguage(string language)
        {
            PlayerPrefs.SetString(Keys.LANGUAGE, language);
            PlayerPrefs.Save();
            EventManager.Current.OnLanguageChanged.Invoke(language);
        }

        /// <summary>
        /// Gets the current language from PlayerPrefs.
        /// If no language is set, it defaults to "English".
        /// </summary>
        /// <returns></returns>
        public string GetLanguage()
        {
            return PlayerPrefs.GetString(Keys.LANGUAGE, "English");
        }

        /// <summary>
        /// Clears the current language setting from PlayerPrefs.
        /// This method can be used to reset the language to the default state.
        /// </summary>
        public void ClearLanguage()
        {
            PlayerPrefs.DeleteKey(Keys.LANGUAGE);
            PlayerPrefs.Save();
            EventManager.Current.OnLanguageChanged.Invoke("");
        }
    }
}