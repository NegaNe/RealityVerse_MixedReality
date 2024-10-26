using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.Oculus.Input;
using Unity.XR.Oculus;
using UnityEngine.Events;

public class GunDisplay : MonoBehaviour
{
    
    [Range(0.0f, 5f)]
    public float SpinSpeed;

    public GameObject DisplayPistol, DisplayRifle, DisplayShotgun;
    public GameObject pivot;

    // Update is called once per frame
    void Update()
    {

    if(GameController.Instance.GunTaken==true)
    {
    gameObject.SetActive(false);
    }
    // Adjust the object's position to move up and down by 10cm
    float newY = pivot.transform.position.y + Mathf.Sin(Time.time) * 0.05f; // 10cm = 0.1f in Unity units
    
    DisplayPistol.transform.Rotate(0, SpinSpeed,0 );
    DisplayRifle.transform.Rotate(0, SpinSpeed,0 );
    DisplayShotgun.transform.Rotate(0, SpinSpeed,0 );


    DisplayPistol.transform.position = new Vector3(DisplayPistol.transform.position.x, newY, DisplayPistol.transform.position.z);
    DisplayRifle.transform.position = new Vector3(DisplayRifle.transform.position.x, newY, DisplayRifle.transform.position.z);
    DisplayShotgun.transform.position = new Vector3(DisplayShotgun.transform.position.x, newY, DisplayShotgun.transform.position.z);

    }



}
