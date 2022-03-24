using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Slider healthbar;
    private int playerHealth;
    public GameObject player;
    public bool isDamaged;
    private float timer;
    public GameObject damageImage;
    // Start is called before the first frame update
    void Start()
    {
        isDamaged = false;
        playerHealth = player.GetComponent<PlayerMovement>().vida;
        healthbar.maxValue = 100;
        healthbar.value = 100;
        timer = 1f;
        //damageImage = GetComponentInChildren<Image>();

    }

    private void Update()
    {
        if (isDamaged) {
            timer -= Time.deltaTime;
            //Debug.Log(timer);
            if (timer < 0f) {

                isDamaged = false;
                timer = 1f;
            }
            damageImage.GetComponent<Image>().enabled = isDamaged;
            //damageImage.enabled = isDamaged;
        }

    }

    // Update is called once per frame
    public void SetHealth(int hp) {
        Debug.Log(hp);
        
        healthbar.value = hp;
        if (healthbar.value < 50) GetComponentInChildren<Image>().color = new Color32(255, 99, 71,255);
    }

    public void DamageScreen() {
        isDamaged = true;

    }

}
