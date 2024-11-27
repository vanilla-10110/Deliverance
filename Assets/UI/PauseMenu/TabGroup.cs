using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    private List<Tab> tabs = new List<Tab>();

    private void Start(){
        foreach (var tab in gameObject.GetComponentsInChildren<Tab>()){
            tabs.Add(tab);
        }

        foreach (var tab in tabs){
            tab.TabSelected.AddListener(() => {
                tabs.FindAll((t) => t != tab).ForEach(t => {t.State = false;});
            });
        }
    }
}
