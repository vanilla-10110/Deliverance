using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeart : MonoBehaviour
{

    public GameObject Flame1;
    public GameObject Flame2;


    public void Awake()
    {
        
    }

    public void ChangeSkull(SkullStatus status)
    {
        switch (status)
        {
            case SkullStatus.None:
                gameObject.SetActive(false);
                Flame1.SetActive(false);
                Flame2.SetActive(false);
                break;
            case SkullStatus.Skull:
                gameObject.SetActive(true);
                Flame1.SetActive(false);
                Flame2.SetActive(false);
                break;
            case SkullStatus.Flame:
                gameObject.SetActive(true);
                Flame1.SetActive(true);
                Flame2.SetActive(false);
                break;
            case SkullStatus.Flames:
                gameObject.SetActive(true);
                Flame1.SetActive(true);
                Flame2.SetActive(true);
                break;
        }
    }

}
public enum SkullStatus
{
    None = 0,
    Skull = 1,
    Flame = 2,
    Flames = 3,
}