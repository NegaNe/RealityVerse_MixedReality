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

[SerializeField]
    private int Health;
[SerializeField]
    private int AttackDmg;
[SerializeField]
    private float Speed;
    public  GoopData goopData, bigGoopData;
    public float wanderRadius = 5f; 
    public float detectionRadius = 15f; 

    public float proximityDuration = 5f; 
    public float wanderDistanceFromPlayer = 10f; 
    private NavMeshAgent agent;
    private GameObject player;
    private Vector3 wanderTarget;
    private bool isChasing = false;
    private float proximityTimer = 0f;

    [SerializeField]
    public enum EnemyType
    {
    Goop,
    BigGoop
    }

    public EnemyType enemyType;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        InitStats();
        WanderTowardsPlayer(); 
        
    }

    void Update()
    {
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

        agent.speed = Speed;
        agent.SetDestination(player.transform.position);

        if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance)
        {
            proximityTimer += Time.deltaTime;
            if (proximityTimer >= proximityDuration)
            {
                isChasing = false;
                // WanderTowardsPlayer();  
            }
            

        }
    }

    
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Destructible"))
        {
        Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            if(enemyType == EnemyType.Goop)
            {
                GameController.Instance.PlayerHealth-=1;
            }
                if(enemyType == EnemyType.BigGoop)
            {
                GameController.Instance.PlayerHealth-=5;
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void OnDestroy()
    {
        EnemySpawner.instance.KilledCounter();
        EnemySpawner.instance.NegateCounter();
    }


}