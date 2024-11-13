using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{

    public AudioClip placeholderMusic;

    private AudioSource audioSource;
    public static SoundManager Instance {get; private set;} 

    private void Awake(){
        // base.OnAwake();
        // DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this){
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }

    void Start()
    {

        audioSource = GetComponent<AudioSource>();

        if (placeholderMusic){
            
            audioSource.playOnAwake = placeholderMusic;
        }
    }

    void Update()
    {
        
    }
}
