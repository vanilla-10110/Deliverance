using System;
using UnityEngine;
using UnityEngine.Events;

public class SignalBus : MonoBehaviour 
{
    // Game state related
    [NonSerialized] public static UnityEvent StartMenuTriggerSignal = new();
    [NonSerialized] public static UnityEvent newSceneLoaded = new();
    [NonSerialized] public static UnityEvent spawnPointUpdated = new();


    // game events
    [NonSerialized] public static UnityEvent CollectedSoulEvent = new();
    [NonSerialized] public static UnityEvent<String> AbilityUsedEvent = new();
    [NonSerialized] public static UnityEvent DestroyedEntityEvent = new();


    // public static SignalBus Instance {get; private set;}

    // private void Awake(){
    //     // DontDestroyOnLoad(this);

    //     if (Instance != null && Instance != this){
    //         Destroy(this);
    //     }
    //     else {
    //         Instance = this;
    //     }
    // }

    
    // // you have to call the Signals class to use these, they are only here to be defined 
    // public class PauseGameSignal : ASignal {}
}