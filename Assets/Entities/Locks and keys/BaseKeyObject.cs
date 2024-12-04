using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using MyBox;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseKeyObject : MonoBehaviour
{
    [NonSerialized] public UnityEvent<KEY_STATE> Keytriggered = new();
    public enum KEY_STATE {
        LOCKED, UNLOCKED
    }
    [SerializeField] public AudioClip[] unlockedClips;

    [SerializeField] public KEY_STATE _currentState = KEY_STATE.LOCKED;

    public void SetKeyState(KEY_STATE newState){
        _currentState = newState;
        Keytriggered.Invoke(_currentState);
    }

    [Header("Debug")]
    [SerializeField] private SerializedDictionary<KEY_STATE, Color> debugColor = new(){
        {KEY_STATE.LOCKED, Color.red},
        {KEY_STATE.UNLOCKED, Color.green}
    };
    [SerializeField] private bool debugGizmo = false;

    private void OnDrawGizmos(){
        if (debugGizmo){
            Gizmos.color = debugColor[_currentState];
            Gizmos.DrawCube(transform.position,new(0.2f,0.2f));
        }
    }

    protected void PlayRandonUnlockSound(){
        SoundManager.Instance.PlaySoundFX(unlockedClips.GetRandom());
    }
}
