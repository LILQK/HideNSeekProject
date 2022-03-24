using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    public string[] Items = new string[99];
    // Start is called before the first frame update
    void Start()
    {
        AddItem("Sword");
    }

    // Update is called once per frame

    public void AddItem(string itm) {
        for (int i = 0; i < Items.Length - 1; i++) {
            if (Items[i] == "") {
                Items[i] = itm;
                Debug.Log(itm);
                return;
            }
        }
    }

    public bool HasItem(string itm) {

        bool hasitem = false;
        for (int i = 0; i < Items.Length - 1; i++) {
            if (itm == Items[i]) {
                hasitem =  true;
            }
        }
        return hasitem;
    
    }
    public void DeleteItem(string itm) {

        for (int i = 0; i < Items.Length - 1; i++) {
            if (itm == Items[i]) {
                Items[i] = "";
            }
        }

    
    }
}
