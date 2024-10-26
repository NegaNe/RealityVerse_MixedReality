using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;


public class GunManager : MonoBehaviour
{    
    public GunManager instance;
    public GameObject[] LeftHandControllerData, RightHandControllerData;
    public GameObject LeftController, RightController;
    public GameObject[] Pistol, Rifle, Shotgun;
    private ControllerCast controllerCast;
    public GameObject ActiveLeft, ActiveRight;
   public enum WeaponType
    {
        None,
        Pistol,
        Rifle,
        Shotgun,
    }
    public WeaponType currentGun = WeaponType.None;

    void Start()
    {
        //controllerCast = FindObjectOfType<ControllerCast>();

        if(LeftController.layer != LayerMask.NameToLayer("GunDisplay"))
        LeftHandControllerData = new GameObject[LeftController.transform.childCount];
        if(RightController.layer != LayerMask.NameToLayer("GunDisplay"))
        RightHandControllerData = new GameObject[RightController.transform.childCount];

        if(LeftController.layer != LayerMask.NameToLayer("GunDisplay")){
        for (int i=0 ; i < LeftController.transform.childCount ; i++) {
        LeftHandControllerData[i] = LeftController.transform.GetChild(i).gameObject;
        LeftHandControllerData[i].AddComponent<ControllerCast>();
            }
        }

        if(RightController.layer != LayerMask.NameToLayer("GunDisplay")){
        for (int i=0 ; i < RightController.transform.childCount ; i++) {
        RightHandControllerData[i] = RightController.transform.GetChild(i).gameObject;
        RightHandControllerData[i].AddComponent<ControllerCast>();
            }
        }


            for (int i = 0; i < RightController.transform.childCount; i++)
            {
                if (RightController.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    ActiveRight = RightController.transform.GetChild(i).gameObject;
                    break;
                }
            }
    }
    

    public void ChangeGun(WeaponType newGun)
    {
        Pistol[0].SetActive(newGun == WeaponType.Pistol);
        Pistol[1].SetActive(newGun == WeaponType.Pistol);
        Rifle[0].SetActive(newGun == WeaponType.Rifle);
        Rifle[1].SetActive(newGun == WeaponType.Rifle);
        Shotgun[0].SetActive(newGun == WeaponType.Shotgun);
        Shotgun[1].SetActive(newGun == WeaponType.Shotgun);
    }


}
