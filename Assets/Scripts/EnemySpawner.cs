using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public static EnemySpawner instance;
    [SerializeField]
    private GameObject[] Spawners = new GameObject[4];
    [SerializeField]
    private GameObject[] EnemyPrefabs;
    [SerializeField]
    public int SpawnAreaPosThreshold = 3;
    public int EnemyCounter {get ; private set;}
    public int EnemiesKilled {get; private set;}
    private float SpawnTimer = 1f; //start spawn should be atleast 3-4 seconds per enemy, after around 20-30 enemies, it should be 1-2 seconds, even .5f.
    private GameObject EnemyInstance;
    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();
    
    void Start()
    {
        if (instance == null)
        {
        instance = this;
        } 
        else
        {
        Destroy(instance);
        }
        InvokeRepeating(nameof(SpawnEnemies), 0, SpawnTimer);
        Spawners = GameObject.FindGameObjectsWithTag("Spawner");
    }


    void SpawnEnemies()
    {
        if (GameController.Instance.GunTaken && GameController.Instance.StartGame && EnemyCounter < GameController.Instance.MaxEnemiesInMap && EnemyCounter <= GameController.Instance.MaxTotalEnemies && EnemiesKilled + EnemyCounter <= GameController.Instance.MaxTotalEnemies)
        {
            Vector3 spawnAreaPos = new Vector3(Random.Range(-SpawnAreaPosThreshold, SpawnAreaPosThreshold), 4, Random.Range(-SpawnAreaPosThreshold, SpawnAreaPosThreshold));
            int spawnRand = Random.Range(0, Spawners.Length);
            int enemyRand = Random.Range(0, EnemyPrefabs.Length);

            GameObject enemyInstance = Instantiate(EnemyPrefabs[enemyRand], Spawners[spawnRand].transform.position + spawnAreaPos, Spawners[spawnRand].transform.rotation);
            enemies.Add(enemyInstance); // Add the spawned enemy to the list

            if (EnemyCounter >= GameController.Instance.MaxEnemiesInMap && EnemiesKilled + EnemyCounter >= GameController.Instance.MaxTotalEnemies)
                CancelInvoke();
            else
                InvokeRepeating(nameof(SpawnEnemies), 0, SpawnTimer);

            EnemyCounter++;
        }
    }

    public void DeactivateAllEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(false); // Disable the enemy GameObject
            }
        }
        enemies.Clear(); // Clear the list after deactivating
        EnemyCounter = 0; // Reset the enemy counter if needed
    }

    public void RemoveAllEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                //Destroy(enemy); // Destroy the enemy GameObject
                enemy.SetActive(false);
            }
        }
        enemies.Clear(); // Clear the list after removing
        EnemyCounter = 0; // Reset the enemy counter if needed
    }



    public void NegateCounter()
    {
        EnemyCounter--;
    }

    public int? KilledCounter()
    {
        return EnemiesKilled++;
    }

}
