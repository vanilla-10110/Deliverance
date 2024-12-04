using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{

    public AudioClip backgroundTrack;

    [SerializeField] private AudioMixer _mainMixer;
    [SerializeField] private AudioMixerGroup _soundFXGroup;
    [SerializeField] private AudioMixerGroup _musicGroup;

    private AudioSource _musicSource;

    [SerializeField] private int _maxSoundFXs = 10;
    private List<AudioSource> _audioSources = new();
    public static SoundManager Instance {get; private set;} 

    private void Awake(){

        if (Instance != null && Instance != this){
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < _maxSoundFXs; i++){
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.name = "SoundFXSource " + i;
            source.outputAudioMixerGroup = _soundFXGroup;
            _audioSources.Add(source);
        }

        AudioSource musicSource = gameObject.GetComponent<AudioSource>();
        musicSource.name = "MusicSource";
        _musicSource = musicSource;

        _musicSource.outputAudioMixerGroup = _musicGroup;
        _musicSource.clip = backgroundTrack;
        _musicSource.loop = true;
        _musicSource.Play();

    }

    public void PlaySoundFX(AudioClip clip, float volume = 0.5f){
        foreach (AudioSource source in _audioSources){
            if (!source.isPlaying){

                source.volume = volume;

                source.clip = clip;
                source.Play();
                return;
            }
        }
    }


    public void PlayBackgroundTrack(AudioClip clip){
        _musicSource.Stop();
        _musicSource.clip = clip;
        _musicSource.Play();
    }


}
