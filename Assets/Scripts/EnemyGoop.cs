using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class Goop
{
    public int Health = 20;
    public int AttackDmg = 8;
    public float Speed=.4f;

}

public class BigGoop
{
    public int Health = 65;
    public int AttackDmg = 20;
    public float Speed=.2f;

}

public class FlyingGoop
{
    public int Health = 15;
    public int AttackDmg = 3;
    public float Speed=.4f;

}

public class EnemyGoop : MonoBehaviour
{

[SerializeField]
    private int Health;
[SerializeField]
    private int AttackDmg;
[SerializeField]
    private float Speed;
    private Goop goop = new Goop();
    private BigGoop bigGoop = new BigGoop();
    private FlyingGoop flyingGoop = new FlyingGoop();
    public UnityEvent OnDeath;
    public UnityEvent OnSpawn;
    public float wanderRadius = 5f; 
    public float detectionRadius = 15f; 
    public float chaseSpeed = 0;
    public float wanderSpeed = 0;
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
    BigGoop,
    FlyingGoop
    }
    public EnemyType enemyType;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        InitStats();
        WanderTowardsPlayer(); 
    }

    private void InitStats()
    {
    switch (enemyType)
    {
    case EnemyType.Goop:
        Health = goop.Health;
        AttackDmg = goop.AttackDmg;
        Speed = goop.Speed;
    break;
    case EnemyType.BigGoop:
        Health = bigGoop.Health;
        AttackDmg = bigGoop.AttackDmg;
        Speed = bigGoop.Speed;
    break;
    case EnemyType.FlyingGoop:
        Health = flyingGoop.Health;
        AttackDmg = flyingGoop.AttackDmg;
        Speed = flyingGoop.Speed;
    break;
    }


    }

        void Awake()
    {
        OnSpawn.Invoke();
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
        EnemySpawner.instance.KilledCounter();
        EnemySpawner.instance.NegateCounter();
    }
}