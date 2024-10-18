using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Spawners;
    public GameObject[] EnemyPrefabs;
    public int MaxEnemies = 16;
    [SerializeField]
    private int MaxTotalEnemies = 112;
    public int SpawnAreaPosThreshold = 12;
    int EnemyCounter = 0;
    private float SpawnTimer = 1f; //start spawn should be atleast 3-4 seconds per enemy, after around 20-30 enemies, it should be 1-2 seconds, even .5f.

    
    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemies), 0, SpawnTimer);
    }


    void SpawnEnemies()
    {
        if(EnemyCounter < MaxEnemies)
        {
        EnemyCounter++;
        Vector3 spawnAreaPos = new Vector3(Random.Range(-SpawnAreaPosThreshold, SpawnAreaPosThreshold), 4, Random.Range(-SpawnAreaPosThreshold, SpawnAreaPosThreshold));
        int spawnRand = Random.Range(0, Spawners.Length);
        int enemyRand = Random.Range(0, EnemyPrefabs.Length);

        GameObject EnemyInstance = Instantiate(EnemyPrefabs[enemyRand], Spawners[spawnRand].transform.position + spawnAreaPos, Spawners[spawnRand].transform.rotation);        
        
        }
        else if(EnemyCounter >= MaxEnemies)
        {
            return;
        }

    }

}
