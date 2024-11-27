using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SpawnPopupBox(GameObject dialoguePopup, Transform parent){
        Instantiate(dialoguePopup, parent);
        dialoguePopup.SetActive(true);
        
    }
}
