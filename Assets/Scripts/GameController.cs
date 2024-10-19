using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameController Instance;
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

    private int CountEnemy()
    {
        return 1;
    }

    void OnApplicationQuit()
    {
        
    }
}
