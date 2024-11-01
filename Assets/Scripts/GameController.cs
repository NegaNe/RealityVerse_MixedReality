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
    // Start is called before the first frame update
    public static GameController Instance;
    [SerializeField]
    public int MaxTotalEnemies;
    public int MaxEnemiesInMap;
    public AudioClip SelectSound;
    public AudioClip GoopBounce;
    public AudioClip GoopAttack;
    public AudioClip HealSound;
    public AudioClip RageSound;
    public Text EnemiesKilled;
    public TMP_Text TimerText;
    public TMP_Text HealthText;
    public TMP_Text EnemiesLeftText;
    public GameObject[] PowerUp;
    public GameObject[] BuffText;
    public GameObject WinText;
    public GameObject LoseText;

    public float PlayerHealth = 100;
    public float Timer = 300; // set to seconds -> 300s = 5 minutes, 300s * 2 = 600s = 10 minutes
    public bool GunTaken, StartGame;
    public GameObject[] LevelObject;
    public GameObject LevelSelector;
    public GameObject SummaryMenu;
    public GameObject StatsMenu;
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


    void Update()
    {
    if(Input.GetKeyDown(KeyCode.M))
    {
        LevelChange(1);
    }

    PlayerControllerSelectSound();
        
        GameState();

        if(StartGame)
        {
        GameStatsControl();
        }
    }

    private void PlayerControllerSelectSound()
    {
        if(OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) || OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        AudioSource.PlayClipAtPoint(SelectSound, transform.position);
    }

private void GameTimer()
{
    // Decrease the timer by the time passed since the last frame
    Timer -= Time.deltaTime;

    // Clamp the Timer to zero to avoid negative time
    Timer = Mathf.Max(Timer, 0);

    // Convert the timer to minutes and seconds
    int minutes = Mathf.FloorToInt(Timer / 60);
    int seconds = Mathf.FloorToInt(Timer % 60);

    // Format the time and display it on the UI Text
    TimerText.SetText(string.Format("Time : {0:00}:{1:00}", minutes, seconds));
}


    private void GameState()
    {
        try
        {
            // if (EnemySpawner.instance.EnemiesKilled >= MaxTotalEnemies )
            // {
            //     GunManager.instance.ChangeGun(GunManager.WeaponType.None);
            //     StartGame = false;
            //     SummaryMenu.SetActive(true);
            //     WinText.SetActive(true);
            //     StatsMenu.SetActive(false);
            //     EnemiesKilled.text = "Enemies Killed : " + EnemySpawner.instance.EnemiesKilled;

            // } 
            if (PlayerHealth <= 0)
            {
                GunManager.instance.ChangeGun(GunManager.WeaponType.None);
                StartGame = false;
                SummaryMenu.SetActive(true);
                LoseText.SetActive(true);
                EnemySpawner.instance.RemoveAllEnemies();
                StatsMenu.SetActive(false);
                EnemiesKilled.text = "Enemies Killed : " + EnemySpawner.instance.EnemiesKilled;
                EmptyEnemies();
            }
            else if(Timer <=0)
            {
                GunManager.instance.ChangeGun(GunManager.WeaponType.None);
                StartGame = false;
                SummaryMenu.SetActive(true);
                WinText.SetActive(true);
                StatsMenu.SetActive(false);
                EnemySpawner.instance.RemoveAllEnemies();
                EnemiesKilled.text = "Enemies Killed : " + EnemySpawner.instance.EnemiesKilled;
            }
        }
        catch
        {
            return;
        }
    }

    private void EmptyEnemies()
    {
        foreach(var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
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

    public void RageEffect(bool state)
    {
        foreach(var buffText in BuffText)
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
{    yield return new WaitForSeconds(10f);

    RageEffect(false);
    instance.BulletDamage = originalDamage;
}

}
