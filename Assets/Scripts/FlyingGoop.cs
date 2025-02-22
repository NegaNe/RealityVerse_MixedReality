using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]

public class FlyingGoop : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float shootingInterval = 2f;
    private float shootingDistance;
    public GoopData FlyingGoopData;
    public GameObject particlePrefab;
    private float explosionForce = 100f;

    private float speed;
    public float health;

    private NavMeshAgent agent;
    private GameObject player;
    private bool isShooting = false;
    private bool isDodging = false;

    // LayerMask for obstacles
    public LayerMask obstacleLayer;

    void Start()
    {
        // Initialize speed and health from FlyingGoopData
        speed = FlyingGoopData.Speed;
        health = FlyingGoopData.Health;

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found! Ensure the player has the 'Player' tag.");
            return;
        }

        agent.speed = speed;
        shootingDistance = agent.stoppingDistance + 1;

        

        StartCoroutine(DodgeRoutine());
    }

    void Update()
    {
        if(health<=0)
        {
        Destroy(gameObject);
        }


        if (player == null) return;
        try{
        if (!isDodging)
        {
            agent.SetDestination(player.transform.position);
        }
        } catch{return;}

        Vector3 lookPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(player.transform.position);
        if (!isShooting && agent.remainingDistance <= shootingDistance)
        {
            agent.isStopped = true;
            StartCoroutine(ShootAtPlayer());
        }


    }

    private IEnumerator ShootAtPlayer()
    {
        isShooting = true;

        while (Vector3.Distance(transform.position, player.transform.position) <= shootingDistance)
        {
            AudioSource.PlayClipAtPoint(GunManager.instance.PistolSound, transform.position);
            GameObject bulletInstance = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 shootingDirection = (player.transform.position - bulletSpawnPoint.position).normalized;
                rb.velocity = shootingDirection * 10f;
            }
            else
            {
                Debug.LogError("Bullet prefab is missing a Rigidbody component.");
            }

            yield return new WaitForSeconds(shootingInterval);
        }

        isShooting = false;
        agent.isStopped = false;
    }

    private IEnumerator DodgeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 5f));

            if (!isDodging && CanDodge())
            {
                StartCoroutine(Dodge());
            }
        }
    }

    private bool CanDodge()
    {
        Vector3 dodgeDirection = Random.value > 0.5f ? transform.right : -transform.right;
        Vector3 dodgeTarget = transform.position + dodgeDirection * 2f; 

        if (Physics.Raycast(transform.position, dodgeDirection, .5f, obstacleLayer))
        {
            return false; 
        }

        return true; 
    }

    private IEnumerator Dodge()
    {
        isDodging = true;

        Vector3 dodgeDirection = Random.value > 0.5f ? transform.right : -transform.right;
        Vector3 dodgeTarget = transform.position + dodgeDirection * 2f;  

        float dodgeDuration = 1f;  
        float elapsed = 0f;
        Vector3 startPosition = transform.position;

        while (elapsed < dodgeDuration)
        {
            transform.position = Vector3.Lerp(startPosition, dodgeTarget, elapsed / dodgeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        isDodging = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingDistance);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Destructible"))
        {
            Destroy(other.gameObject);
        }
    }

        private void OnKilled()
    {
            for (int i = 0; i < 5; i++)
        {
            GameObject particle = Instantiate(particlePrefab, transform.position, Random.rotation);
            
            Rigidbody rb = particle.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 explosionDirection = (Random.insideUnitSphere + Vector3.up).normalized;
                rb.AddForce(explosionDirection * explosionForce);
            }
            Destroy(particle, 1.5f);
        }
    }


    void OnDestroy()
    {
    OnKilled();
    if(EnemySpawner.instance.KilledCounter() != null)
    {
        EnemySpawner.instance.KilledCounter();
    }
        EnemySpawner.instance.NegateCounter();

        if(Random.value <= 0.1f)
        {
            Instantiate(GameController.Instance.PowerUp[Random.Range(1,GameController.Instance.PowerUp.Length )], transform.position, Quaternion.identity);
        }

    }
}
