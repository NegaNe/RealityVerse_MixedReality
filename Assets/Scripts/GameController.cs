using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameController Instance;
    [SerializeField]
    public int MaxTotalEnemies = 16;
    public GameObject[] NavSurface;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } 
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if(EnemySpawner.instance.EnemiesKilled >= MaxTotalEnemies)
        {
            Debug.Log("Enemies Are All Dead!");
        } else
        {
            Debug.Log("There Are : " + EnemySpawner.instance.EnemiesKilled + " Enemies Left!");
        }
    }

    private int CountEnemy()
    {
        return 1;
    }

    void OnApplicationQuit()
    {
        
    }
}
