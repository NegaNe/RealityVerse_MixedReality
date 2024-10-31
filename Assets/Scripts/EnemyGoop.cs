using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[RequireComponent(typeof(AudioSource), typeof(NavMeshAgent))]
public class EnemyGoop : MonoBehaviour
{
    private ParticleSystem destroyParticles;
    public float Health;
    [SerializeField]
    private int AttackDmg;
    [SerializeField]
    private float Speed;
    public GoopData goopData, bigGoopData;
    public float wanderRadius = 5f; 
    public float detectionRadius = 15f; 

    public float proximityDuration = 5f; 
    public float wanderDistanceFromPlayer = 10f; 
    private NavMeshAgent agent;
    private GameObject player;
    private Vector3 wanderTarget;
    private bool isChasing = false;
    private float proximityTimer = 0f;
    private AudioSource audiosrc;

    [SerializeField]
    public enum EnemyType
    {
        Goop,
        BigGoop
    }

    public EnemyType enemyType;

    // Animator reference
    private Animator animator;

    void Start()
    {
        destroyParticles = GetComponent<ParticleSystem>();
    
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Get the Animator component
        player = GameObject.FindGameObjectWithTag("Player");
        InitStats();
        WanderTowardsPlayer(); 

        audiosrc = GetComponent<AudioSource>();
        audiosrc.volume = .5f;


    }

    void Update()
    {
    Debug.Log("enemy health : " + Health);
        if(Health<=0)
        {
            Destroy(gameObject);
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else 
        {
            WanderAroundPlayer();
        }

        DetectPlayer();
    }

    private void InitStats()
    {
        switch (enemyType)
        {
            case EnemyType.Goop:
                Health = goopData.Health;
                AttackDmg = goopData.Damage;
                Speed = goopData.Speed;
                break;
            case EnemyType.BigGoop:
                Health = bigGoopData.Health;
                AttackDmg = bigGoopData.Damage;
                Speed = bigGoopData.Speed;
                break;
        }
    }

    void WanderTowardsPlayer()
    {
        if (player == null) return; // Ensure the player reference exists

        // Generate a random direction around the player within wanderRadius
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += player.transform.position;

        // Sample a valid NavMesh position near the random direction
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            wanderTarget = hit.position;
            agent.speed = Speed;
            agent.SetDestination(wanderTarget);
        }
        else
        {
            // If a valid position is not found, call this method again
            WanderTowardsPlayer();
        }
    }

    void WanderAroundPlayer()
    {
        // Check if the enemy has reached wanderTarget; if so, generate a new one
        if (Vector3.Distance(transform.position, wanderTarget) <= agent.stoppingDistance)
        {
            WanderTowardsPlayer();
        }
    }

    void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                isChasing = true;
                proximityTimer = 0f;
                return;
            }
        }
        isChasing = false;
    }

    void ChasePlayer()
    {
        // Set the agent's speed
        agent.speed = Speed;

        // Move towards the player
        agent.SetDestination(player.transform.position);

        // Check if within attack range
        if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance+1)
        {
            // Stop moving
            agent.isStopped = true;

            // Change to attack animation
            animator.SetTrigger("Attack");


            // Handle proximity timer
            proximityTimer += Time.deltaTime;
            if (proximityTimer >= proximityDuration)
            {
                isChasing = false;
                WanderTowardsPlayer();
            }
        }
        else
        {
            // If not within attack range, resume movement
            animator.SetTrigger("Walk");
            agent.isStopped = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Destructible"))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void OnDestroy()
    {
    EnemyParticle();
        EnemySpawner.instance.KilledCounter();
        EnemySpawner.instance.NegateCounter();

    }

    public void AttackPlayer()
    {
    AudioSource.PlayClipAtPoint(GameController.Instance.GoopAttack, transform.position);
        GameController.Instance.PlayerHealth -= AttackDmg;
    }

        private void EnemyParticle()
    {
        if (destroyParticles != null)
        {
            destroyParticles.transform.parent = null;  // Detach the particle system
            destroyParticles.Play();                   // Play the particle system
        }

        Destroy(gameObject);  // Destroy the enemy object
    }

    public void BounceSound()
    {
    AudioSource.PlayClipAtPoint(GameController.Instance.GoopBounce, transform.position);
    }
}
