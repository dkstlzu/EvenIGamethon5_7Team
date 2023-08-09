using System;
using System.Collections.Generic;
using dkstlzu.Utility;
using UnityEngine;

namespace MoonBunny
{
    public class SoundManager : Singleton<SoundManager>
    {
        private List<AudioSource> AudioSourceList = new List<AudioSource>();

        [SerializeField] private List<AudioSource> _playingSourceList = new List<AudioSource>();
        [SerializeField] private List<AudioSource> _stayingSourceList = new List<AudioSource>();
        [SerializeField] private GameObject _audioSourceParent;
        [SerializeField] private AudioSource _bgmAudioSource;

        public AudioClip IntroBGM;
        public AudioClip LobbyBGM;
        public AudioClip Stage1BGM;
        public AudioClip Stage2BGM;
        public AudioClip Stage3BGM;
        public AudioClip Stage4BGM;
        public AudioClip Stage5BGM;
        public AudioClip StageChallengeBGM;
        public AudioClip EndingBGM;

        private void Awake()
        {
            AudioSourceList.AddRange(_audioSourceParent.GetComponentsInChildren<AudioSource>());
            _stayingSourceList.AddRange(AudioSourceList);
            
            Dictionary<string, Action> callbackDict = GameManager.instance.SCB.SceneLoadCallBackDict;
            
            callbackDict[SceneName.Scenario] += () => PlayBGM(IntroBGM);
            callbackDict[SceneName.Start] += () => PlayBGM(LobbyBGM);
            callbackDict[SceneName.Stage1_1] += () => PlayBGM(Stage1BGM);
            callbackDict[SceneName.Stage1_2] += () => PlayBGM(Stage1BGM);
            callbackDict[SceneName.Stage1_3] += () => PlayBGM(Stage1BGM);
            callbackDict[SceneName.Stage2_1] += () => PlayBGM(Stage2BGM);
            callbackDict[SceneName.Stage2_2] += () => PlayBGM(Stage2BGM);
            callbackDict[SceneName.Stage2_3] += () => PlayBGM(Stage2BGM);
            callbackDict[SceneName.Stage3_1] += () => PlayBGM(Stage3BGM);
            callbackDict[SceneName.Stage3_2] += () => PlayBGM(Stage3BGM);
            callbackDict[SceneName.Stage3_3] += () => PlayBGM(Stage3BGM);
            callbackDict[SceneName.Stage4_1] += () => PlayBGM(Stage4BGM);
            callbackDict[SceneName.Stage4_2] += () => PlayBGM(Stage4BGM);
            callbackDict[SceneName.Stage4_3] += () => PlayBGM(Stage4BGM);
            callbackDict[SceneName.Stage5_1] += () => PlayBGM(Stage5BGM);
            callbackDict[SceneName.Stage5_2] += () => PlayBGM(Stage5BGM);
            callbackDict[SceneName.Stage5_3] += () => PlayBGM(Stage5BGM);
            callbackDict[SceneName.StageChallenge] += () => PlayBGM(StageChallengeBGM);
            callbackDict[SceneName.Ending] += () => PlayBGM(EndingBGM);
        }

        public void PlayBGM(AudioClip clip)
        {
            if (_bgmAudioSource == null) return;
            if (clip == null) return;
            
            _bgmAudioSource.clip = clip;
            _bgmAudioSource.loop = true;
            _bgmAudioSource.Play();
        }

        public void PlayClip(AudioClip clip)
        {
            if (_stayingSourceList.Count == 0) return;
            if (clip == null) return;

            AudioSource targetSource = _stayingSourceList[0];
            targetSource.clip = clip;
            targetSource.Play();
            _playingSourceList.Add(targetSource);
            _stayingSourceList.RemoveAt(0);
            CoroutineHelper.Delay(() =>
            {
                _playingSourceList.Remove(targetSource);
                _stayingSourceList.Add(targetSource);
            }, clip.length);
        }
    }
}