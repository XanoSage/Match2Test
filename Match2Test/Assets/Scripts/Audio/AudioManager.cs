using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Audio 
{
    public enum SoundFxType {
        //gameProcess sound
        BlockPress = 0,     //Cubepress
        BlockPressError,    //CubePressError
        BonusAward,         //BoosterAward
        BlockMove,          //Collectable
        BombBonusUsed,      //ColorBomb
        GoalComplete,       //ReachedGoal

        //UI
        ButtonClick,        //Button
        LevelFail,          //Lose
        LevelComplete       //Win
    }

    public enum MusicType
    {
        BackgroundMsic_1 = 0,
        BackgroundMsic_2,
    }

    public class AudioManager : MonoBehaviour {
        [SerializeField] private List<AudioClipData> _audioClipDataList = new List<AudioClipData>();
        [SerializeField] private List<MusicClipData> _musicDataList = new List<MusicClipData>();

        [SerializeField] private AudioSource _soundFXSource = null;
        [SerializeField] private AudioSource _musicSource = null;

        private static AudioManager _instance;

        public static void PlaySoundFx(SoundFxType soundFx) {
            _instance?.PlaySfx(soundFx);
        }

        public static void PlayMusic(MusicType musicType) {
            _instance?.PlayMusicInner(musicType);
        }

        public static void SetMusicEnabled(bool isEnabled) {
            _instance?.SetMusicEnabledInner(isEnabled);
        }

        public static void SetSoundEnabled(bool isEnabled) {
            _instance?.SetSoundEnabledInner(isEnabled);
        }

        private void Awake() {
            _instance = this;
        }

        private void SetMusicEnabledInner(bool isEnabled) {
            _musicSource.mute = isEnabled;
        }

        private void SetSoundEnabledInner(bool isEnabled) {
            _soundFXSource.mute = isEnabled;
        }

        private void PlaySfx(SoundFxType soundFx) {
            var clip = GetAudioClip(soundFx);
            if (clip == null)
                return;

            _soundFXSource.PlayOneShot(clip);
        }

        private AudioClip GetAudioClip(SoundFxType soundFx) {
            var audioClipData = _audioClipDataList.Find(clipData => clipData.SoundFx == soundFx);
            if (audioClipData != null)
                return audioClipData.Clip;
            return null;
        }

        private void PlayMusicInner(MusicType musicType) {
            var music = GetMusicClip(musicType);
            if (music == null)
                return;
            _musicSource.clip = music;
            _musicSource.Play();
        }

        private AudioClip GetMusicClip(MusicType musicType) {
            var musicData = _musicDataList.Find(musicClip => musicClip.Type == musicType);
            if (musicData != null)
                return musicData.Clip;
            return null;
        }
    }

    [Serializable]
    public class AudioClipData {
        public SoundFxType SoundFx;
        public AudioClip Clip;
    }

    [Serializable]
    public class MusicClipData
    {
        public MusicType Type;
        public AudioClip Clip;
    }

}
