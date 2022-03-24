using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public bool respawn;
    [SerializeField] private Vector3 respawnPosition;

    private Animator animator;

    private bool isAttacking = false;


    void Start()
    {
        Declaraciones();
    }

    // Update is called once per frame
    void Update()
    {
        
        Respawn();
        Attack();


    }
    public void Respawn() {

        if (transform.position.y < -5f) {
            Debug.Log("Respawn");
            this.transform.position = respawnPosition;

        }
    
    }


    void Declaraciones() {

        respawnPosition = transform.position;

        respawn = true;

        animator = GetComponentInChildren<Animator>();
    }

    void Attack() {
        if (Input.GetMouseButtonDown(0)) {
            animator.SetBool("isAttacking1", true);
            isAttacking = true;
            
        }
        if (Input.GetMouseButtonUp(0)) {
            animator.SetBool("isAttacking1", false);
            isAttacking = false;
        }
    }
}
