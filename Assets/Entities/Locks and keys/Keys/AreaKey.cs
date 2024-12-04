using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class AreaKey : BaseKeyObject
{
    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.CompareTag("Player")){
            if (_currentState == KEY_STATE.LOCKED){PlayRandomUnlockSound();}
            
            SetKeyState(KEY_STATE.UNLOCKED);
            
        }
    }
}
