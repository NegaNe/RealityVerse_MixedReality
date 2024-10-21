using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public static EnemySpawner instance;
    public GameObject[] Spawners;
    public GameObject[] EnemyPrefabs;
    public int MaxEnemies = 8;
    [SerializeField]
    public int SpawnAreaPosThreshold = 0;
    public int EnemyCounter = 0;
    public int EnemiesKilled = 0;
    private float SpawnTimer = 1f; //start spawn should be atleast 3-4 seconds per enemy, after around 20-30 enemies, it should be 1-2 seconds, even .5f.

    
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
    }


    void SpawnEnemies()
    {
        if(EnemyCounter < MaxEnemies && EnemiesKilled >= GameController.Instance.MaxTotalEnemies)
        {
        Vector3 spawnAreaPos = new Vector3(Random.Range(-SpawnAreaPosThreshold, SpawnAreaPosThreshold), 4, Random.Range(-SpawnAreaPosThreshold, SpawnAreaPosThreshold));
        int spawnRand = Random.Range(0, Spawners.Length);
        int enemyRand = Random.Range(0, EnemyPrefabs.Length);

        GameObject EnemyInstance = Instantiate(EnemyPrefabs[enemyRand], Spawners[spawnRand].transform.position + spawnAreaPos, Spawners[spawnRand].transform.rotation);
        if(EnemyInstance)
        EnemyCounter++;
        Debug.Log("Enemy Count : " + EnemyCounter);
        }
        else if(EnemyCounter >= MaxEnemies)
        {
            return;
        }

    }

    public void NegateCounter()
    {
        EnemyCounter--;
    }

    public int KilledCounter()
    {
        return EnemiesKilled++;
    }

}
