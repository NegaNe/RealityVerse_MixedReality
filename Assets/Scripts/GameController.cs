using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using TMPro;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameController : MonoBehaviour
{
    public static GameController Instance;

    // Serialized fields and other variables
    [SerializeField] 
    public int MaxTotalEnemies;
    public int MaxEnemiesInMap;
    public AudioClip DestroyWallSound, SelectSound, GoopBounce, GoopAttack, HealSound, RageSound;
    public Text EnemiesKilled;
    public TMP_Text TimerText, HealthText, EnemiesLeftText;
    public GameObject[] PowerUp, BuffText;
    public GameObject WinText, LoseText, SummaryMenu, StatsMenu;
    public GameObject[] LevelObject;
    public GameObject LevelSelector;
    public GameObject EnemyRushUI;
    public AudioClip rushSound;
    public List<GameObject> DebrisPrefabs;
    private GameObject Player;
    public float PlayerHealth = 100;
    public float Timer = 300; // Timer in seconds
    public bool GunTaken, StartGame;
    private bool isGameOver = false; 

    public void WallDestroySound(Vector3 WallPos)
    {
        AudioSource.PlayClipAtPoint(DestroyWallSound, WallPos);
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartGame = false;
        GunTaken = false;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {    
    if(!GunTaken)
    PlayerControllerSelectSound();
        if (Input.GetKeyDown(KeyCode.M))
        {
            LevelChange(1);
        }

        if (!isGameOver) 
        {
            GameStateChecker();
        }

        if (StartGame && GunTaken)
        {
            GameStatsControl();
        }
    }

 private void PlayerControllerSelectSound()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) || OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            AudioSource.PlayClipAtPoint(SelectSound, Player.transform.position);
    }

    private void GameTimer()
    {
        Timer -= Time.deltaTime;
        Timer = Mathf.Max(Timer, 0);

        int minutes = Mathf.FloorToInt(Timer / 60);
        int seconds = Mathf.FloorToInt(Timer % 60);

        TimerText.text = $"Time : {minutes:00}:{seconds:00}";
    }

    private void GameStateChecker()
    {

    float rushTimer = 5;

        if (PlayerHealth <= 0) 
        {
            EndGame("Lose");
        }
        else if (Timer <= 0) 
        {
            EndGame("Win");
        }
        if(Timer <= 60)
        {
        EnemyRushUI.gameObject.SetActive(true);
        MaxEnemiesInMap = 16;
        AudioSource.PlayClipAtPoint(rushSound, Player.transform.position);
            if(rushTimer >= 0)
            {
                rushTimer -= Time.deltaTime;
            }
            if(rushTimer <= 0)
            {
                EnemyRushUI.gameObject.SetActive(false);
            }
        }
    }

    private void EndGame(string result)
    {
        isGameOver = true;
        StartGame = false;
        GunTaken = false;


        // send scoreData to ScoreManager
        int enemiesKilled = EnemySpawner.instance.EnemiesKilled;
        FindObjectOfType<ScoreManager>().AddScore(enemiesKilled, Timer, result);

        SummaryMenu.SetActive(true);
        StatsMenu.SetActive(false);
        EnemiesKilled.text = $"Enemies Killed : {enemiesKilled}";

        if (result == "Win")
        {
            WinText.SetActive(true);
        }
        else
        {
            LoseText.SetActive(true);
        }

        EnemySpawner.instance.RemoveAllEnemies(); // kill all enemies
        GunManager.instance.ChangeGun(GunManager.WeaponType.None);
    }
    private void EmptyEnemies()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.SetActive(false);
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
        StartGame = true;
        LevelObject[index].SetActive(true);
        StartCoroutine(GameEvents.Instance.AppearGunChoice());
        LevelSelector.SetActive(false);
    }

    public void DebrisSpawner(Vector3 hit)
    {
        GameObject debrisPrefab = DebrisPrefabs[Random.Range(0, DebrisPrefabs.Count)];
        GameObject debris = Instantiate(debrisPrefab, hit, Random.rotation);
        Destroy(debris, 2.5f);
    }

    public void GameStatsControl()
    {
        StatsMenu.SetActive(true);
        GameTimer();
        HealthText.text = "Health : " + PlayerHealth;
        EnemiesLeftText.text = $"{EnemySpawner.instance.EnemiesKilled} / {MaxTotalEnemies} Enemies";
    }

    public void RageEffect(bool state)
    {
        foreach (var buffText in BuffText)
        {
            buffText.SetActive(state);
        }
    }

    public void DamageUp()
    {
        GunMuzzle[] weaponInstances = FindObjectsOfType<GunMuzzle>();
        foreach (var instance in weaponInstances)
        {
            float originalDamage = instance.BulletDamage;
            instance.BulletDamage += originalDamage * 0.2f;
            Instance.RageEffect(true);
            AudioSource.PlayClipAtPoint(RageSound, transform.position);

            StartCoroutine(ResetDamageAfterDelay(instance, originalDamage));
        }
    }

    public IEnumerator ResetDamageAfterDelay(GunMuzzle instance, float originalDamage)
    {
        yield return new WaitForSeconds(10f);
        RageEffect(false);
        instance.BulletDamage = originalDamage;
    }
}
