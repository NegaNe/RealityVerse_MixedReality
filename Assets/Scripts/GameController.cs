using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using TMPro;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameController Instance;
    [SerializeField]
    public int MaxTotalEnemies;
    public int MaxEnemiesInMap;
    public TMP_Text EnemiesKilled;

    public bool GunTaken, StartGame;
    public GameObject SummaryMenu;

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


    void Update()
    {
        GameState();

    }

    private void GameState ()
    {
        if(EnemySpawner.instance.EnemiesKilled >= MaxTotalEnemies)
        {

            StartGame=false;
            SummaryMenu.SetActive(true);
        }

    }

    private int CountEnemy()
    {
        return 1;
    }

    void OnApplicationQuit()
    {
        
    }
    void RestartGame()
    {
    SceneManager.LoadScene(0);
    }
}
