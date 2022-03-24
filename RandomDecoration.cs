using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDecoration : MonoBehaviour
{
    private void Awake()
    {
        foreach (Transform child in transform)
        {
            child.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360),Vector3.up);
        }
    }
}
