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
        // === Vibration Properties ===
        private bool isAndroid;
        private AndroidJavaObject vibrator;

        protected override void Awake()
        {
            base.Awake();
#if UNITY_ANDROID && !UNITY_EDITOR
                isAndroid = true;
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#endif
        }

        /// <summary>
        /// Checks if the device is Android.
        /// </summary>
        /// <returns></returns>
        public bool IsVibration()
        {
            return PlayerPrefs.GetInt(Keys.VIBRATION, 1) == 1;
        }

        /// <summary>
        /// Sets the vibration state.
        /// </summary>
        /// <param name="enabled"></param>
        public void SetVibration(bool enabled)
        {
            PlayerPrefs.SetInt(Keys.VIBRATION, enabled ? 1 : 0);
            PlayerPrefs.Save();
        }



        /// <summary>
        /// Vibrates the device for a specified duration.
        /// If vibration is disabled or not supported, it logs a warning.
        /// </summary>
        /// <param name="milliseconds"></param>
        public void Vibrate(long milliseconds = 100)
        {
            if (!IsVibration() || !SystemInfo.supportsVibration)
            {
                Debug.LogWarning("Vibration is disabled or not supported.");
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
                if (vibrator != null && vibrator.Call<bool>("hasVibrator"))
                {
                    if (SystemInfo.operatingSystem.Contains("API-26") || SystemInfo.operatingSystem.Contains("API-27") || SystemInfo.operatingSystem.Contains("API-28") || SystemInfo.operatingSystem.Contains("API-29") || SystemInfo.operatingSystem.Contains("API-30") || SystemInfo.operatingSystem.Contains("API-31") || SystemInfo.operatingSystem.Contains("API-32") || SystemInfo.operatingSystem.Contains("API-33") || SystemInfo.operatingSystem.Contains("API-34"))
                    {
                        AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                        AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, -1);
                        vibrator.Call("vibrate", vibrationEffect);
                    }
                    else
                    {
                        vibrator.Call("vibrate", milliseconds);
                    }
                }
#else
            Handheld.Vibrate();
#endif
        }

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