using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        foreach (Transform child in transform) {
            child.gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
}
