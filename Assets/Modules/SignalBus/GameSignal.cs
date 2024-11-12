using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameSignal : MonoBehaviour {
    private readonly List<Action> listeners = new();

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
        foreach (Action listener in listeners){
            listener.Invoke();
        }
    }

    public void Connect(Action listener){
        listeners.Add(listener);
    }

    public void Disconnect(Action listener){
        listeners.Remove(listener);
    }

}