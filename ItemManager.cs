using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    Vector3 RandomRot;
    public Transform player;
    PlayerItems items;
    bool isPicked;
    // Start is called before the first frame update

    private void Start()
    {
        items = player.GetComponent<PlayerItems>();
        isPicked = false;
    }
    // Update is called once per frame
    void Update()
    {
        
        RandomRot = new Vector3(Random.Range(0, 80), Random.Range(0, 80), 0);
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).gameObject.CompareTag("key")) transform.GetChild(i).gameObject.transform.Rotate(RandomRot * Time.deltaTime);
            if (transform.GetChild(i).gameObject.CompareTag("light")) transform.GetChild(i).gameObject.GetComponent<Light>().range = Mathf.Max(Mathf.Abs(Mathf.Sin(1f * Time.time)/2f),0.15f);
        }

        if (!isPicked && Vector3.Distance(transform.position, player.position) > .1f && Vector3.Distance(transform.position, player.position) <  1.5f) {
            transform.Translate(-(transform.position - player.position).normalized * Time.deltaTime);
        }
        if (!isPicked && Vector3.Distance(transform.position, player.position) <= .5f)
        {
            
            items.AddItem(transform.name);
            Destroy(gameObject,0f);
            isPicked = true;

        }
    }

}
