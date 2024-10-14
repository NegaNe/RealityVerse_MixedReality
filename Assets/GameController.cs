using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Surfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameController Instance;

    public GameObject NavSurface;
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
        StartCoroutine(nameof(BuildNavMesh), 2f);
    }

    void BuildNavMesh()
    {
        NavMeshSurface navsurface = NavSurface.GetComponent<NavMeshSurface>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    private int CountEnemy()
    {

        return 1;
    }
}
