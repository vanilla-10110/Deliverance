using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class AreaKey : BaseKeyObject {
    [SerializeField] private Animator _animator;

    private void Start(){
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.CompareTag("Player")){
            if (_currentState == KEY_STATE.LOCKED){PlayRandomUnlockSound();}
            _animator.SetTrigger("TriggerOpen");
            SetKeyState(KEY_STATE.UNLOCKED);
            
        }
    }
}
