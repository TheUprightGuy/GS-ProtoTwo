using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        #region Singleton

        public static AudioManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("More than one AudioManager in scene!");
                Destroy(this.gameObject);
                return;
            }

            Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }

        #endregion
        
        [Header("Audio Sources Used To Create Sound Dictionary:")]
        public List<AudioSource> audioSources;
        public float masterVolume = 1.0f;

        private Dictionary<string, SoundInfo> _soundDictionary;
        [SerializeField] private GameObject volumeSlider = null;
        private Slider _slider;
        private AudioSource _musicSource;
        private float _musicDefaultVolume;

        private void Start()
        {
            InitialisePrivateVariables();
            foreach (var audioSource in audioSources)
            {
                AddAudioSourceToDictionary(audioSource);
            }
        }

        private void InitialisePrivateVariables()
        {
            _soundDictionary = new Dictionary<string, SoundInfo>();
            if (volumeSlider != null) _slider = volumeSlider.GetComponent<Slider>();
            _musicSource = this.GetComponent<AudioSource>();
            _musicDefaultVolume = _musicSource.volume;
        }

        private void AddAudioSourceToDictionary(AudioSource audioSource)
        {
            var soundName = audioSource.name;
            var soundInfo = new SoundInfo();
            soundInfo.InitialiseSound(soundName);
            _soundDictionary.Add(soundName, soundInfo);
        }

        public void PlaySound(string soundName)
        {
            var sound = _soundDictionary[soundName];
            sound.Reset();
            PlaySound(sound.AudioSource);
        }

        public void StopSound(string soundName)
        {
            _soundDictionary[soundName].AudioSource.Stop();
        }

        public void OnVolumeAdjusted()
        {
            masterVolume = _slider.value;
            _musicSource.volume = _musicDefaultVolume * masterVolume;
        }
    

        private void PlaySound(AudioSource audioSource) //Only play sound if it's not already playing
        {
            AdjustPitchAndVolume(audioSource);
            if (!audioSource.isPlaying || audioSource.name == "ui")    //Should expand if any sounds(apart from UI) need to be played like this
                audioSource.Play();
        }

        private void AdjustPitchAndVolume(AudioSource audioSource)
        {
            audioSource.pitch *= (Random.value * 0.5f + 0.75f); //Pitch is default multiplied by random value between 0.75 and 1.25
            audioSource.volume *= masterVolume;
        }
    }
}