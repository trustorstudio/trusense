using System;
using System.Collections;
using Trusense.Constants;
using Trusense.Tools;
using UnityEngine;

namespace Trusense.Managers
{
    /// <summary>
    /// EnergyManager is a singleton manager responsible for handling energy-related functionalities in the game.
    /// Provides energy and timer data through events and public methods, with debug logging every second.
    /// 
    /// Author: Nguyễn Duy Khánh
    /// Created: August 6, 2025
    /// Last Modified: August 7, 2025
    /// Version: 1.4
    /// </summary>
    public class EnergyManager : Singleton<EnergyManager>
    {
        // === Serialized Fields ===
        [Header("Energy Configuration")]
        [Tooltip("Maximum energy that can be stored.")]
        [SerializeField] private int maxEnergy = 60;
        [Tooltip("Duration in minutes to restore one energy unit.")]
        [SerializeField] private int restoreDuration = 1;

        // === Events Action ===
        public event Action<int> OnEnergyChanged;
        public event Action<string> OnTimerUpdated;

        // === Private Fields ===
        private bool isRestoring;
        private int currentEnergy;
        private DateTime nextEnergyTime;
        private DateTime lastEnergyTime;
        private Coroutine restoreCoroutine;

        /// <summary>
        /// Initializes the EnergyManager.
        /// Sets up the initial energy state, restores energy if needed, and subscribes to necessary events
        /// </summary>
        private void Start()
        {
            if (restoreDuration <= 0)
                restoreDuration = 1;

            if (maxEnergy <= 0)
                maxEnergy = 60;

            currentEnergy = PlayerPrefs.GetInt(Keys.CURRENT_ENERGY, maxEnergy);
            nextEnergyTime = ParseDateTime(PlayerPrefs.GetString(Keys.NEXT_ENERGY_TIME), DateTime.Now.AddMinutes(restoreDuration));
            lastEnergyTime = ParseDateTime(PlayerPrefs.GetString(Keys.LAST_ENERGY_TIME), DateTime.Now);

            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

            isRestoring = currentEnergy < maxEnergy;
            if (isRestoring && restoreCoroutine == null)
            {
                restoreCoroutine = StartCoroutine(RestoreCoroutine());
            }

            OnEnergyChanged?.Invoke(currentEnergy);
        }

        /// <summary>
        /// Cleans up the EnergyManager.
        /// Stops any ongoing restoration coroutine and unsubscribes from events.
        /// This method is called when the EnergyManager is destroyed.
        /// It ensures that the EnergyManager does not continue to run or update after it has been destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (restoreCoroutine != null)
            {
                StopCoroutine(restoreCoroutine);
            }
        }

        /// <summary>
        /// Parses a date time string and returns a DateTime object.
        /// If the string is null or empty, returns the default value provided.
        /// If the string cannot be parsed, returns the default value.
        /// This method is used to safely parse date time strings from PlayerPrefs.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private DateTime ParseDateTime(string dateTime, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(dateTime))
                return defaultValue;
            if (DateTime.TryParse(dateTime, out DateTime result))
                return result;
            return defaultValue;
        }

        /// <summary>
        /// Saves the current energy state to PlayerPrefs.
        /// This method stores the current energy, next energy restoration time, and last energy restoration time to PlayerPrefs.
        /// It ensures that the energy state is persistent across game sessions.
        /// </summary>
        private void Save()
        {
            PlayerPrefs.SetInt(Keys.CURRENT_ENERGY, currentEnergy);
            PlayerPrefs.SetString(Keys.NEXT_ENERGY_TIME, nextEnergyTime.ToString("o"));
            PlayerPrefs.SetString(Keys.LAST_ENERGY_TIME, lastEnergyTime.ToString("o"));
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Restores energy over time.
        /// This coroutine checks the time until the next energy restoration and updates the current energy accordingly.    
        /// </summary>
        /// <returns></returns>
        private IEnumerator RestoreCoroutine()
        {
            float logTimer = 0f;

            while (currentEnergy < maxEnergy)
            {
                TimeSpan timeUntilNext = nextEnergyTime - DateTime.Now;

                if (timeUntilNext.TotalSeconds <= 0)
                {
                    currentEnergy = Mathf.Min(currentEnergy + 1, maxEnergy);
                    lastEnergyTime = DateTime.Now;
                    nextEnergyTime = lastEnergyTime.AddMinutes(restoreDuration);
                    Save();
                    OnEnergyChanged?.Invoke(currentEnergy);

                    if (currentEnergy >= maxEnergy)
                    {
                        isRestoring = false;
                        OnTimerUpdated?.Invoke("Full");
                        yield break;
                    }
                }

                string timer = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    timeUntilNext.Hours, timeUntilNext.Minutes, timeUntilNext.Seconds);
                OnTimerUpdated?.Invoke(timer);

                logTimer += Time.deltaTime;
                if (logTimer >= 1f)
                {
                    logTimer = 0f;
                }


                float waitTime = Mathf.Max(0.1f, (float)timeUntilNext.TotalSeconds);
                yield return new WaitForSeconds(waitTime);
            }
        }

        /// <summary>
        /// Uses a specified amount of energy.
        /// This method decreases the current energy by the specified amount, ensuring it does not go below zero.
        /// It also checks if energy restoration should start if the current energy is below the maximum.
        /// </summary>
        /// <param name="amount"></param>
        public void Use(int amount)
        {
            if (amount <= 0 || currentEnergy < amount)
            {
                return;
            }

            currentEnergy -= amount;
            Save();
            OnEnergyChanged?.Invoke(currentEnergy);

            if (!isRestoring && currentEnergy < maxEnergy)
            {
                isRestoring = true;
                nextEnergyTime = DateTime.Now.AddMinutes(restoreDuration);
                restoreCoroutine ??= StartCoroutine(RestoreCoroutine());
            }
        }

        /// <summary>
        /// Adds a specified amount of energy.
        /// This method increases the current energy by the specified amount, ensuring it does not exceed the maximum energy.
        /// It also checks if energy restoration should start if the current energy is below the maximum.
        /// </summary>
        /// <param name="amount"></param>
        public void Add(int amount)
        {
            if (amount <= 0 || currentEnergy >= maxEnergy)
            {
                return;
            }

            currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
            Save();
            OnEnergyChanged?.Invoke(currentEnergy);


            if (!isRestoring && currentEnergy < maxEnergy)
            {
                isRestoring = true;
                nextEnergyTime = DateTime.Now.AddMinutes(restoreDuration);
                restoreCoroutine ??= StartCoroutine(RestoreCoroutine());
            }
        }

        /// <summary>
        /// Gets or sets the current energy.
        /// This property allows getting and setting the current energy, ensuring it stays within the bounds of 0 and maxEnergy.
        /// When setting the current energy, it also saves the state and triggers the OnEnergyChanged
        /// </summary>
        public int CurrentEnergy
        {
            get => currentEnergy;
            set
            {
                currentEnergy = Mathf.Clamp(value, 0, maxEnergy);
                Save();
                OnEnergyChanged?.Invoke(currentEnergy);

                if (!isRestoring && currentEnergy < maxEnergy && restoreCoroutine == null)
                {
                    isRestoring = true;
                    nextEnergyTime = DateTime.Now.AddMinutes(restoreDuration);
                    restoreCoroutine = StartCoroutine(RestoreCoroutine());
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum energy.    
        /// This property allows getting and setting the maximum energy, ensuring it is at least 1.
        /// When setting the maximum energy, it also adjusts the current energy if it exceeds the new maximum and saves the state.
        /// </summary>
        public int MaxEnergy
        {
            get => maxEnergy;
            set
            {
                maxEnergy = Mathf.Max(1, value);
                currentEnergy = Mathf.Min(currentEnergy, maxEnergy); // Adjust current energy if needed
                Save();
                OnEnergyChanged?.Invoke(currentEnergy);
            }
        }

        /// <summary>
        /// Checks if the current energy is full.
        /// This property returns true if the current energy is equal to the maximum energy, indicating that
        /// </summary>
        public bool IsEnergyFull()
        {
            return currentEnergy >= maxEnergy;
        }

        /// <summary>
        /// Gets the time remaining until the next energy restoration.
        /// This method calculates the time remaining until the next energy restoration based on the current time and the next energy restoration time.
        /// If the current energy is full, it returns "Full".
        /// </summary>
        /// <returns></returns>
        public string GetTimer()
        {
            if (currentEnergy >= maxEnergy)
                return "Full";

            TimeSpan timeUntilNext = nextEnergyTime - DateTime.Now;
            return string.Format("{0:D2}:{1:D2}:{2:D2}",
                timeUntilNext.Hours, timeUntilNext.Minutes, timeUntilNext.Seconds);
        }
    }
}