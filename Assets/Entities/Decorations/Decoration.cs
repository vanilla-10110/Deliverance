using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration : MonoBehaviour
{

    [SerializeField] private AnimationClip _animationClip;
    [SerializeField] private Sprite _sprite;

    [SerializeField] private Animation _anim;

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = _sprite;
        
        _anim = gameObject.GetComponent<Animation>();
        if (_animationClip != null){
            _anim.clip = _animationClip;
            _anim.Play();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
