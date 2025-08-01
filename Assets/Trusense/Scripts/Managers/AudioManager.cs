using System.Collections.Generic;
using Trusense.Tools;
using UnityEngine;

namespace Trusense.Managers
{
    /// <summary>
    /// AudioManager is a singleton class responsible for managing audio in the application.
    /// It handles background music (BGM) and sound effects (SFX) playback, volume control, and audio pausing/resuming.
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        // -- Background Music Settings --
        [Header("Background Music Settings")]
        [Tooltip("AudioSource component used for playing background music.")]
        [SerializeField] private AudioSource bgmSource;

        [Tooltip("List of background music clips to be used in the game.")]
        [SerializeField] private List<AudioClip> bgmClips;

        [Header("Sound Effects Settings")]
        [Tooltip("AudioSource component used for playing sound effects.")]
        [SerializeField] private AudioSource sfxSource;

        [Tooltip("List of sound effect clips to be used in the game.")]
        [SerializeField] private List<AudioClip> sfxClips;

        [Header("UI Sound Settings")]
        [Tooltip("Sound played when a popup is opened.")]
        [SerializeField] private AudioClip popupOpenSound;

        [Tooltip("Sound played when a popup is closed.")]
        [SerializeField] private AudioClip popupCloseSound;

        [Tooltip("Sound played when a button is clicked.")]
        [SerializeField] private AudioClip buttonClickSound;

        private Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

        protected override void Awake()
        {
            base.Awake(); // Call base Awake to handle Singleton initialization

            // Initialize audio dictionaries
            foreach (var clip in bgmClips)
            {
                if (clip != null)
                    bgmDictionary[clip.name] = clip;
            }
            foreach (var clip in sfxClips)
            {
                if (clip != null)
                    sfxDictionary[clip.name] = clip;
            }

            // Add UI sounds to SFX dictionary
            if (popupOpenSound != null)
                sfxDictionary[popupOpenSound.name] = popupOpenSound;
            if (popupCloseSound != null)
                sfxDictionary[popupCloseSound.name] = popupCloseSound;
            if (buttonClickSound != null)
                sfxDictionary[buttonClickSound.name] = buttonClickSound;
        }

        /// <summary>
        /// Gets the AudioSource associated to that GameObject, and asks the GameManager to play it.
        /// </summary>
        protected virtual void Start()
        {

        }

        /// <summary>
        /// Plays a background music clip by name.
        /// </summary>
        /// <param name="clipName">Name of the BGM clip to play.</param>
        public void PlayBGM(string clipName)
        {
            if (bgmDictionary.TryGetValue(clipName, out AudioClip clip))
            {
                bgmSource.clip = clip;
                bgmSource.Play();
            }
            else
            {
                Debug.LogWarning($"BGM clip {clipName} not found in AudioManager!");
            }
        }

        /// <summary>
        /// Stops the current background music.
        /// </summary>
        public void StopBGM()
        {
            bgmSource.Stop();
        }

        /// <summary>
        /// Plays a sound effect by name.
        /// </summary>
        /// <param name="clipName">Name of the SFX clip to play.</param>
        public void PlaySFX(string clipName)
        {
            if (sfxDictionary.TryGetValue(clipName, out AudioClip clip))
            {
                sfxSource.PlayOneShot(clip);
            }
            else
            {
                Debug.LogWarning($"SFX clip {clipName} not found in AudioManager!");
            }
        }

        /// <summary>
        /// Plays the sound for opening a popup.
        /// </summary>
        public void PlayPopupOpenSound()
        {
            if (popupOpenSound != null)
                sfxSource.PlayOneShot(popupOpenSound);
            else
                Debug.LogWarning("Popup open sound is not assigned in AudioManager!");
        }

        /// <summary>
        /// Plays the sound for closing a popup.
        /// </summary>
        public void PlayPopupCloseSound()
        {
            if (popupCloseSound != null)
                sfxSource.PlayOneShot(popupCloseSound);
            else
                Debug.LogWarning("Popup close sound is not assigned in AudioManager!");
        }

        /// <summary>
        /// Plays the sound for clicking a button.
        /// </summary>
        public void PlayButtonClickSound()
        {
            if (buttonClickSound != null)
                sfxSource.PlayOneShot(buttonClickSound);
            else
                Debug.LogWarning("Button click sound is not assigned in AudioManager!");
        }

        /// <summary>
        /// Sets the volume for background music.
        /// </summary>
        /// <param name="volume">Volume level (0 to 1).</param>
        public void SetBGMVolume(float volume)
        {
            bgmSource.volume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// Sets the volume for sound effects.
        /// </summary>
        /// <param name="volume">Volume level (0 to 1).</param>
        public void SetSFXVolume(float volume)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// Pauses all audio playback.
        /// </summary>
        public void PauseAll()
        {
            bgmSource.Pause();
            sfxSource.Pause();
        }

        /// <summary>
        /// Resumes all audio playback.
        /// </summary>
        public void ResumeAll()
        {
            bgmSource.UnPause();
            sfxSource.UnPause();
        }
    }
}