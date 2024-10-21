using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public UnityEvent unityEvent = new UnityEvent();
    [SerializeField]
    private GameObject GunChoice;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(StartSequence));
    }

    // Update is called once per frame
IEnumerator StartSequence()
{
    yield return new WaitForSeconds(6);
    AppearGunChoice();
    yield return new WaitForSeconds(3);
    

yield break;
}

private void AppearGunChoice()
{
    
}
}
