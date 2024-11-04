using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject GunChoice;
    [SerializeField]
    public bool HasGunAppear;
    // Start is called before the first frame update
    public static GameEvents Instance;


    private void Awake()
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



public IEnumerator AppearGunChoice()
{
    yield return new WaitForSeconds(6f);
    if(!HasGunAppear)
    {
    Instantiate(GunChoice);
    HasGunAppear=true;
    StopCoroutine(AppearGunChoice());
    }
    yield return null;
}




}
