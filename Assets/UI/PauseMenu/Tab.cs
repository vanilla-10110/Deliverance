using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteAlways]
public class Tab : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text textComponent;
    private Image Image;
    private Button button;

    [SerializeField] private string title;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite unselectedSprite;

    [NonSerialized] public UnityEvent TabSelected = new();

    private bool state = false;
    public bool State {
        get{ return state;} 
        set{ 
            state = value;
            if (state){
                Image.sprite = selectedSprite;
            }
            else Image.sprite = unselectedSprite;
        }
    }

    private void Start(){
        Image = GetComponent<Image>();
        button = GetComponent<Button>();

        button.onClick.AddListener(OnButtonClicked);
    }

    private void Update (){
        if (textComponent.text != title){
            textComponent.text = title;
        }
    }

    private void OnButtonClicked(){
        Debug.Log("clicked");
        TabSelected.Invoke();
        State = true;
    }

}
