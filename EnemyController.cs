using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public Transform[] waypoints;
    public Transform player;
    private int _waypoints;
    public float detectionDistance;
    public float attackDistance;
    public float giveUpDistance;

    private NavMeshAgent agent;
    public float timer;
    private float og_timer;

    private int state;
    private int next_state;

    private const int st_follow = 0;
    private const int st_patrol = 1;
    private const int st_idle = 2;
    private const int st_attack = 3;

    private RaycastHit[] hit;
    private Ray beam;


    private Animator animator;

    public ParticleSystem particleManager;

    [SerializeField] private int health;

    // Start is called before the first frame update
    void Start()
    {
        Declaraciones();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) {
            Destroy(gameObject);
        }
        timer -= Time.deltaTime;

        //Si el rayo toca al jugador, lo persigue
        beam = new Ray(transform.position, (player.transform.position - transform.position));

        Debug.DrawRay(beam.origin, beam.direction * detectionDistance);

        if (Physics.RaycastNonAlloc(beam, hit, detectionDistance) > 0 && Vector3.Distance(player.position, transform.position) <= giveUpDistance)
        {
            if (hit[0].collider.gameObject.CompareTag("Player"))
            {
                next_state = st_follow;
                if (Vector3.Distance(player.position, transform.position) <= agent.stoppingDistance) next_state = st_attack;
            }

        } else if (state != st_idle) {
            next_state = st_patrol;
        }


        switch (state){
            case st_follow:
                    Follow();
                    break;
            case st_patrol:
                Patrol();
                break;
            case st_idle:
                IdleWait();
                break;
            case st_attack:
                Attack();
                break;
        }

        animator.SetFloat("Blend", state);
        state = next_state;

    }

    void IdleWait() 
    {
        agent.velocity = Vector3.zero;
        if(timer < 0) {
            next_state = st_patrol;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player") {
            
            other.gameObject.GetComponent<PlayerMovement>().DamagePlayer(-15, this.gameObject);
            particleManager.transform.position = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            particleManager.Play();
        }
        if (other.gameObject.name == "SwordPolyart") {
            DamageEnemy(20);
            particleManager.Play();
        }
    }

    void Declaraciones() {
        _waypoints = 0;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        agent.destination = waypoints[_waypoints].position;
        agent.autoBraking = false;
        og_timer = timer;
        state = next_state = st_patrol;
        hit = new RaycastHit[1];
        health = 10;
    }

    void Follow() {
        agent.speed = 1.75f;
        agent.destination = player.position;
        if (Physics.RaycastNonAlloc(beam, hit, detectionDistance) == 0)
        {
            next_state = st_patrol;
        }
    }

    void Patrol() {
        if (!agent.pathPending && agent.remainingDistance < 0.4f)
        {
            // Returns if no waypoints have been set up
            if (waypoints.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = waypoints[_waypoints].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            _waypoints = (_waypoints + 1) % waypoints.Length;

            timer = og_timer;

            next_state = st_idle;

        }
    }

    void Attack() {
        transform.LookAt(player.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    public void DamageEnemy(int hp)
    {
        health += hp;

        //particleManager.Play();
    }
}
