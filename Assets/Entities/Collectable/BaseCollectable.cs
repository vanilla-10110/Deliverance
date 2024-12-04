using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseCollectable : MonoBehaviour {

    [SerializeField] protected AudioClip _pickupSound;
    protected UnityEvent CollectedEvent = new();

    

}
