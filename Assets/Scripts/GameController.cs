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
    public bool GunTaken, StartGame;
    public GameObject[] LevelObject;
    public GameObject LevelSelector;
    public GameObject SummaryMenu;
    public UnityEvent AfterPickLevel;


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

    private void GameState()
    {
        try
        {
            if (EnemySpawner.instance.EnemiesKilled >= MaxTotalEnemies)
            {
                GunManager.instance.ChangeGun(GunManager.WeaponType.None);
                StartGame = false;
                SummaryMenu.SetActive(true);
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

    void RestartGame()
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
}
