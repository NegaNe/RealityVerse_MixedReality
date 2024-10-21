using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{    
    public GameObject[] LeftHandControllerData, RightHandControllerData;
    public GameObject LeftController, RightController;
    public GameObject Pistol, Rifle, Shotgun;
    public enum WeaponType
    {
        None,
        Pistol,
        Rifle,
        Shotgun,
    }
    public WeaponType GunType;       
    public readonly GunManager ControllerType = new();
    


    void Start()
    {
        LeftHandControllerData = new GameObject[LeftController.transform.childCount];
        RightHandControllerData = new GameObject[RightController.transform.childCount];

        for (int i=0 ; i < LeftController.transform.childCount ; i++) {
        LeftHandControllerData[i] = LeftController.transform.GetChild(i).gameObject;
        }
        
        for (int i=0 ; i < RightController.transform.childCount ; i++) {
        RightHandControllerData[i] = RightController.transform.GetChild(i).gameObject;
        }
    }

    public void ChangeGun()
    {
        switch (GunType)
        {
            case WeaponType.Pistol:
                Pistol.SetActive(true);
                Rifle.SetActive(false);
                Shotgun.SetActive(false);
                break;
            case WeaponType.Rifle:
                Pistol.SetActive(false);
                Rifle.SetActive(true);
                Shotgun.SetActive(false);
                break;
            case WeaponType.Shotgun:
                Pistol.SetActive(false);
                Rifle.SetActive(false);
                Shotgun.SetActive(true);
                break;
        }
    }

}
