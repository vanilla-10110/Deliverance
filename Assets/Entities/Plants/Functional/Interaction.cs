using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    public Animator Grow;
    public bool Grown = false;
    private bool Growable = false;


    // Update is called once per frame
    void Update()
    {
        if (InputManager.interactWasPressed && Growable == true)
        {
            Grow.SetBool("Grow", true);
            Grown = true;

}
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Growable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Growable = false;
        }
    }
}
