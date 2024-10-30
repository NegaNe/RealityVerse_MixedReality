using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using TMPro;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameController Instance;
    [SerializeField]
    public int MaxTotalEnemies;
    public int MaxEnemiesInMap;
    public TMP_Text EnemiesKilled;
    public TMP_Text TimerText;
    public TMP_Text HealthText;
    public TMP_Text EnemiesLeftText;
    public GameObject WinText;
    public GameObject LoseText;

    public float PlayerHealth = 100;
    public float Timer;
    public bool GunTaken, StartGame;
    public GameObject[] LevelObject;
    public GameObject LevelSelector;
    public GameObject SummaryMenu;
    public GameObject StatsMenu;
    public UnityEvent AfterPickLevel;

    public List<GameObject> DebrisPrefabs;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } 
        else
        {
            Destroy(Instance);
        }
    }

    void Start()
    {
        EnemiesKilled = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        
        GameState();

        if(StartGame)
        {
        GameStatsControl();
        }
    }

    private void GameTimer()
    {
        Timer += Time.deltaTime;

        // Convert the timer to minutes and seconds
        int minutes = Mathf.FloorToInt(Timer / 60);
        int seconds = Mathf.FloorToInt(Timer % 60);

        // Format the time and display it on the UI Text
        TimerText.SetText(string.Format("Time : {0:00}:{1:00}", minutes, seconds) );
    }

    private void GameState()
    {
        try
        {
            if (EnemySpawner.instance.EnemiesKilled >= MaxTotalEnemies )
            {
                GunManager.instance.ChangeGun(GunManager.WeaponType.None);
                StartGame = false;
                SummaryMenu.SetActive(true);
                WinText.SetActive(true);
                StatsMenu.SetActive(false);
                EnemiesKilled.SetText("Enemies Killed : " + EnemySpawner.instance.EnemiesKilled);

            } else if (PlayerHealth <= 0)
            {
                GunManager.instance.ChangeGun(GunManager.WeaponType.None);
                StartGame = false;
                SummaryMenu.SetActive(true);
                LoseText.SetActive(true);

                StatsMenu.SetActive(false);
                EnemiesKilled.SetText("Enemies Killed : " + EnemySpawner.instance.EnemiesKilled);

            }
        }
        catch
        {
            return;
        }
    }

    private int CountEnemy()
    {
        return 1;
    }

    public void RestartGame()
    {
    SceneManager.LoadScene(0);
    }

    public void LevelChange(int index)
    {
    StartGame=true;
    LevelObject[index].SetActive(true);
    StartCoroutine(GameEvents.Instance.AppearGunChoice());
    LevelSelector.SetActive(false);

    }   

    public void DebrisSpawner(Vector3 hit)
    {
        GameObject debrisPrefab = DebrisPrefabs[Random.Range(0, DebrisPrefabs.Count)];
        GameObject debris = Instantiate(debrisPrefab, hit, Quaternion.identity);
        Destroy(debris, 1f);
    }

    public void GameStatsControl()
    {
    StatsMenu.SetActive(true);
    GameTimer();
    HealthText.SetText("Health : " + PlayerHealth);
    EnemiesLeftText.SetText(EnemySpawner.instance.EnemiesKilled + " / " + MaxTotalEnemies + " Enemies");

    }
}
