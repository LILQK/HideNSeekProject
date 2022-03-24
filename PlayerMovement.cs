using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    [Header("Variables jugador")] [SerializeField] private float maximumSpeed;

    [SerializeField] private float rotationSpeed;

    [SerializeField] private float jumpSpeed;

    [SerializeField] private float jumpButtonGracePeriod;

    [SerializeField] private Transform cameraTransform;

    [Header("Variables jugador")]

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    [SerializeField] private Vector3 respawnPosition;
    private bool isAttacking = false;

    [SerializeField] private bool isDead;

    public int vida;

    public const bool respawn = true;

    public Canvas ui;
    public UiManager uiManager;

    public ParticleSystem particleManager;
    public ParticleSystem waterParticle;

    public GameObject cameraFollower;

    private bool isRecovered;

    private bool isDamaged;

    private GameObject currEnemy;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        respawnPosition = transform.position;
        vida = 100;
        isDead = false;
        uiManager = ui.GetComponent<UiManager>();
  
}

    // Update is called once per frame
    void Update()
    {
        //Attack();
        if (!isDead) {
            Movement();
        }
        //Debug.Log(vida);
        if (vida <= 0 || transform.position.y < -5f && !isDead) {
            Debug.Log("die");
            isDead = true;
            StartCoroutine(Drawn());
            
            //isDead = true;
            //Respawn();
        }

        if (isDamaged) {
            //Vector3 trans = (transform.position - currEnemy.transform.position).normalized;
            Vector3 trans = (currEnemy.transform.position - transform.position).normalized;
            trans.y = 0f;
            transform.position = Vector3.MoveTowards(transform.position, (transform.position - trans), 3.5f * Time.deltaTime);
            //transform.Translate( trans * 7f * Time.deltaTime);
        }
        animator.SetBool("isDead", isDead);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void DamagePlayer(int hp, GameObject enemy) {
        vida += hp;
        uiManager.SetHealth(vida);
        uiManager.DamageScreen();
        StartCoroutine(GetHit());
        currEnemy = enemy;
        //particleManager.Play();
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isAttacking1", true);
            isAttacking = true;

        }
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isAttacking1", false);
            isAttacking = false;
        }
        if (Input.GetMouseButtonDown(0)) animator.SetLayerWeight(1, 1f);
    }

    IEnumerator  GetHit() {
        characterController.enabled = false;
        isDamaged = true;
        yield return new WaitForSeconds(.2f);
        characterController.enabled = true;
        isDamaged = false;
    }

    public void Respawn()
    {
        
        Debug.Log("Respawn");
        characterController.enabled = false;
        isRecovered = false;

        this.transform.position = respawnPosition;

        if (Vector3.Distance(transform.position , respawnPosition) < .5f && isDead) {

                vida = 100;
                characterController.enabled = true;
                ui.GetComponent<UiManager>().SetHealth(vida);
                animator.SetBool("isDead", false);
                isDead = false;

        }
    }

    private void Movement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude /= 1.7f;
            animator.SetBool("isSprint", true);
        }
        else animator.SetBool("isSprint", false);


        float speed = inputMagnitude * maximumSpeed;
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }

        Vector3 velocity = movementDirection * speed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            animator.SetBool("isRuning", true);
        }
        else { animator.SetBool("isRuning", false); }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("water")) {
            isDead = true;
            StartCoroutine(Drawn());
        
        }

    }

    IEnumerator Drawn() {

        //characterController.enabled = false;
        Vector3 freezPos = transform.position;
        for (float t = 0; t < 2f; t += Time.deltaTime) {
            animator.SetBool("isDead", true);
            
            if (t >= 1.8 && isDead) {
                
                Respawn();
            }
            yield return null;
        }
    }


}
