using System;
using UnityEngine;

public class SignalBus : MonoBehaviour 
{

    public static GameSignal StartMenuTriggerSignal = new();
    public static GameSignal newSceneLoaded = new();

    public static SignalBus Instance {get; private set;}

    private void Awake(){
        // DontDestroyOnLoad(this);

        if (Instance != null && Instance != this){
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }

    
    // // you have to call the Signals class to use these, they are only here to be defined 
    // public class PauseGameSignal : ASignal {}
}