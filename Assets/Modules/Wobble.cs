using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    // [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _flickerIntensity = 1f;
    [SerializeField] private float _flickerFrequency = 1f;

    private void Update (){
        float YPos = Mathf.Sin(_flickerFrequency * Time.time) * _flickerIntensity;
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (YPos * Time.deltaTime), gameObject.transform.position.y);
    }

}
