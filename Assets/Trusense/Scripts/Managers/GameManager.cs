using System;
using Trusense.Constants;
using Trusense.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Trusense.Managers
{
    /// <summary>
    /// GameManager is responsible for managing the game's state, including initialization,
    /// loading scenes, and handling game events.
    /// 
    /// Author: Nguyễn Duy Khánh
    /// Created: August 6, 2025
    /// Last Modified: August 6, 2025
    /// Version: 1.0
    /// </summary>

    public class GameManager : Singleton<GameManager>
    {

        // === Handle Privacy Policy
        /// <summary>
        /// 
        /// </summary>
        public void SavePrivacy()
        {
            PlayerPrefs.SetInt(Keys.PRIVACY, 1);
            PlayerPrefs.Save();
        }


        /// <summary>
        /// Checks if the privacy policy has been accepted.
        /// </summary>
        /// <returns>True if the privacy policy is accepted, false otherwise.</returns>
        public bool CheckPrivacy()
        {
            return PlayerPrefs.GetInt(Keys.PRIVACY, 0) == 1;
        }


    }
}