using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
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


    if(GameController.Instance.StartGame)
    yield return new WaitForSeconds(8);
    AppearGunChoice();
    yield return new WaitForSeconds(4);

yield break;
}

private void AppearGunChoice()
{
   GameObject gunPod =  Instantiate(GunChoice);
   gunPod.GetComponent<Animator>();
}
}
