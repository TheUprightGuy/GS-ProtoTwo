using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        #region Singleton
        public static AudioManager instance;
        private void Awake()
        {
            if (instance != null)
            {
                Debug.Log("More than one AudioManager in scene!");
                Destroy(this.gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(this.gameObject);
            
            
            InitialisePrivateVariables();
            foreach (var audioSource in audioSources)
            {
                AddAudioSourceToDictionary(audioSource);
            }

            _soundsUnrestricted = new List<string> {"ui"};
        }

        #endregion
        
        [Header("Audio Sources Used To Create Sound Dictionary:")]
        public List<AudioSource> audioSources;
        public float masterVolume = 1.0f;

        private Dictionary<string, SoundInfo> _soundDictionary;
        private List<string> _soundsUnrestricted;    //Audio source will play this sound regardless of if it's already playing
        [SerializeField] private GameObject volumeSlider = null;
        private Slider _slider;
        private AudioSource _musicSource;
        private float _musicDefaultVolume;

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
            masterVolume = _slider.value / 10f;
            _musicSource.volume = _musicDefaultVolume * masterVolume;
            PlaySound("ui");
        }
    

        private void PlaySound(AudioSource audioSource) //Only play sound if it's not already playing
        {
            AdjustPitchAndVolume(audioSource);
            if (!audioSource.isPlaying || _soundsUnrestricted.Contains(audioSource.name))
                audioSource.Play();
        }

        private void AdjustPitchAndVolume(AudioSource audioSource)
        {
            audioSource.pitch *= (Random.value * 0.5f + 0.75f); //Pitch is default multiplied by random value between 0.75 and 1.25
            audioSource.volume *= masterVolume;
        }
    }
}