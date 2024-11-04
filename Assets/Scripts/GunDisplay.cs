using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.Oculus.Input;
using Unity.XR.Oculus;
using UnityEngine.Events;
using UnityEngine.AI;

public class GunDisplay : MonoBehaviour
{
    
    [Range(0.0f, 5f)]
    public float SpinSpeed;

    public GameObject DisplayPistol, DisplayRifle, DisplayShotgun;
    public GameObject pivot;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (GameController.Instance.GunTaken)
        {
            gameObject.SetActive(false);
        }

        float newY = pivot.transform.position.y + Mathf.Sin(Time.time * SpinSpeed) * 0.05f;

        DisplayPistol.transform.SetPositionAndRotation(new Vector3(DisplayPistol.transform.position.x, newY, DisplayPistol.transform.position.z), DisplayPistol.transform.rotation * Quaternion.Euler(0, SpinSpeed , 0));
        DisplayRifle.transform.SetPositionAndRotation(new Vector3(DisplayRifle.transform.position.x, newY, DisplayRifle.transform.position.z), DisplayRifle.transform.rotation * Quaternion.Euler(0, SpinSpeed , 0));
        DisplayShotgun.transform.SetPositionAndRotation(new Vector3(DisplayShotgun.transform.position.x, newY, DisplayShotgun.transform.position.z), DisplayShotgun.transform.rotation * Quaternion.Euler(0, SpinSpeed , 0));
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("NonDestructible"))
        {
            GameController.Instance.DebrisSpawner(new Vector3(transform.position.x , transform.position.y+.5f, transform.position.z));
                // implement sound & debris
        }
    }
}
