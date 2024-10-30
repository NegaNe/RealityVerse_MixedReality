using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FlyingGoop : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float shootingInterval = 2f;
    private float shootingDistance;
    public GoopData FlyingGoopData;   // ScriptableObject or data class containing Speed and Health

    private float speed;
    private float health;

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

        // Set the agent's speed based on FlyingGoopData
        agent.speed = speed;
        shootingDistance = agent.stoppingDistance + 1;

        // Start the dodge coroutine
        StartCoroutine(DodgeRoutine());
    }

    void Update()
    {
        if (player == null) return;

        // Set agent destination to the player's position if not dodging
        if (!isDodging)
        {
            agent.SetDestination(player.transform.position);
        }

        // Smoothly rotate to face the player
        Vector3 lookPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(player.transform.position);

        // Check if within shooting range and if the agent has stopped
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
            yield return new WaitForSeconds(Random.Range(0.5f, 2f)); // Wait a random time up to 2 seconds

            if (!isDodging && CanDodge())
            {
                StartCoroutine(Dodge());
            }
        }
    }

    private bool CanDodge()
    {
        // Check for obstacles in the dodge direction
        Vector3 dodgeDirection = Random.value > 0.5f ? transform.right : -transform.right;
        Vector3 dodgeTarget = transform.position + dodgeDirection * 2f;  // Adjust dodge distance as needed

        // Raycast to check for obstacles
        if (Physics.Raycast(transform.position, dodgeDirection, 2f, obstacleLayer))
        {
            return false; // Obstacle detected, cannot dodge
        }

        return true; // No obstacles detected, can dodge
    }

    private IEnumerator Dodge()
    {
        isDodging = true;

        // Pick a random dodge direction (left or right)
        Vector3 dodgeDirection = Random.value > 0.5f ? transform.right : -transform.right;
        Vector3 dodgeTarget = transform.position + dodgeDirection * 2f;  // Adjust dodge distance as needed

        // Smooth dodge over time with Lerp
        float dodgeDuration = 0.5f;  // Duration for dodge to complete
        float elapsed = 0f;
        Vector3 startPosition = transform.position;

        while (elapsed < dodgeDuration)
        {
            transform.position = Vector3.Lerp(startPosition, dodgeTarget, elapsed / dodgeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset dodging status
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
}
