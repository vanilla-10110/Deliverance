using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseLockObject : MonoBehaviour
{
    protected UnityEvent<LOCK_STATE> LockStateChanged = new();
    [SerializeField] private BaseKeyObject[] _lockKeys;
    [SerializeField] protected AudioClip _unlockSound;

    protected enum LOCK_STATE {
        LOCKED, UNLOCKED
    }

    [SerializeField] private LOCK_STATE _currentState = LOCK_STATE.LOCKED;
    protected LOCK_STATE CurrentState {
        get { return _currentState; } 
        private set {
            if (_currentState != value) {
                _currentState = value;
                LockStateChanged.Invoke(_currentState); 
            }
        }
    }

    protected virtual void Update(){
        if (KeysToUnlock() <= 0 && CurrentState == LOCK_STATE.LOCKED){
            CurrentState = LOCK_STATE.UNLOCKED;
            SoundManager.Instance.PlaySoundFX(_unlockSound, 0.7f);
        }
        else if (KeysToUnlock() > 0 && CurrentState == LOCK_STATE.UNLOCKED) {
            CurrentState = LOCK_STATE.LOCKED;
        }
    }

    private int KeysToUnlock(){
        int keysNeededToUnlock = 0;
        foreach (BaseKeyObject key in _lockKeys){
            if (key._currentState == BaseKeyObject.KEY_STATE.LOCKED){
                keysNeededToUnlock += 1;
            }
        }

        return keysNeededToUnlock;
    }

    [Header("Debug")]
    [SerializeField] private SerializedDictionary<LOCK_STATE, Color> debugColor = new(){
        {LOCK_STATE.LOCKED, Color.red},
        {LOCK_STATE.UNLOCKED, Color.green}
    };
    [SerializeField] private bool debugGizmo = false;

    private void OnDrawGizmos(){
        if (debugGizmo){
            Gizmos.color = debugColor[CurrentState];
            Gizmos.DrawCube(transform.position,new(0.2f,0.2f));
        }
    }
}
