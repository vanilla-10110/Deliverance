using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameSignal : MonoBehaviour {
    private List<UnityEvent> listeners = new List<UnityEvent>();

    private GameSignal Instance;

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }

    public void Emit(){
        foreach (UnityEvent listener in listeners){
            listener.Invoke();
        }
    }

    public void Connect(UnityEvent listener){
        listeners.Add(listener);
    }

    public void Disconnect(UnityEvent listener){
        listeners.Remove(listener);
    }

}