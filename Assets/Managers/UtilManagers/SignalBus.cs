using System;
using deVoid.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class SignalBus : MonoBehaviour 
{
    SignalHub hub = new SignalHub();

    private SignalBus Instance;

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }


    public void Connect<SignalClassName>(Action action) where SignalClassName : ISignal, new()  {
        (Instance.hub.Get<SignalClassName>() as ASignal).AddListener(action);
    }
    
    // you have to call the Signals class to use these, they are only here to be defined 
    public class PauseGameSignal : ASignal {}
}