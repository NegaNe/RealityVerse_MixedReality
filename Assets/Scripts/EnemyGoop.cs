using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnemyGoop : MonoBehaviour
{
    public UnityEvent OnDeath;
    public UnityEvent OnSpawn;
    public float wanderRadius = 5f; 
    public float detectionRadius = 15f; 
    public float chaseSpeed = 5f;
    public float wanderSpeed = 2f;
    public float proximityDuration = 5f; 
    public float wanderDistanceFromPlayer = 10f; 
    private NavMeshAgent agent;
    private GameObject player;
    private Vector3 wanderTarget;
    private bool isChasing = false;
    private float proximityTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        WanderTowardsPlayer(); 

        OnDeath ??= new UnityEvent();
        OnDeath.AddListener(() => Destroy(gameObject));
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else 
        {
            // WanderAroundPlayer();
        }

        DetectPlayer();
    }

    void WanderTowardsPlayer()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderDistanceFromPlayer;
        randomDirection += player.transform.position;  
         NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        wanderTarget = hit.position;
        agent.speed = wanderSpeed;

        agent.SetDestination(wanderTarget);
    }


    void WanderAroundPlayer()
    {
        if (Vector3.Distance(transform.position, wanderTarget) <= agent.stoppingDistance)
            WanderTowardsPlayer();  
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
        Collider[] hits = Physics.OverlapSphere(transform.position, .8f);

        foreach (var hit in hits)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Destructible"))
            {
                isChasing=false;
                Destroy(hit.gameObject, .4f); 
            } else
            {
                isChasing=true;
            }
        }
        agent.speed = chaseSpeed;
        agent.SetDestination(player.transform.position);

        if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance)
        {
            proximityTimer += Time.deltaTime;
            if (proximityTimer >= proximityDuration)
            {
                isChasing = false;
                WanderTowardsPlayer();  
            }
        }
    }


void OnCollisionEnter(Collision other)
{
    if(other.gameObject.layer == LayerMask.NameToLayer("Destructible"))
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
        OnDeath.Invoke();
    }

    void Awake()
    {
        OnSpawn.Invoke();
    }
}
