using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;


public class GunManager : MonoBehaviour
{    
    public static GunManager instance;
    public GameObject[] LeftHandControllerData, RightHandControllerData;
    public GameObject LeftController, RightController;
    public GameObject[] Pistol, Rifle, Shotgun;
    public float GunDamage;
    public GameObject ActiveLeft, ActiveRight;
    public AudioClip ShotgunSound, RifleSound, PistolSound;
    public GunData RifleData, ShotgunData, PistolData;

   public enum WeaponType
    {
        None,
        Pistol,
        Rifle,
        Shotgun,
    }
    [SerializeField]
    private WeaponType currentGun = WeaponType.None;

    void Start()
{
    InitializeControllerData(LeftController, ref LeftHandControllerData, ref ActiveLeft);
    InitializeControllerData(RightController, ref RightHandControllerData, ref ActiveRight);

    switch (currentGun)
    {
    case WeaponType.Pistol:
    GunDamage = PistolData.GunDamage;
    break;
    case WeaponType.Rifle:
    GunDamage = RifleData.GunDamage;
    break;
    case WeaponType.Shotgun:
    GunDamage = ShotgunData.GunDamage;
    break;
    }

}

void Update()
{
    ChangeGun(currentGun);
}

void Awake()
{
    if (instance == null)
    {
        instance = this;
    }
    else if (instance != this)
    {
        Destroy(gameObject);
    }
}

void InitializeControllerData(GameObject controller, ref GameObject[] controllerData, ref GameObject activeController)
{
    if (controller.layer != LayerMask.NameToLayer("GunDisplay"))
    {
        int childCount = controller.transform.childCount;
        controllerData = new GameObject[childCount];
        
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = controller.transform.GetChild(i).gameObject;
            controllerData[i] = child;
            child.AddComponent<ControllerCast>();

            if (child.activeInHierarchy)
            {
                activeController = child;
            }
        }
    }
}
    

    public void ChangeGun(WeaponType newGun)
    {
        currentGun = newGun;
 
        Pistol[0].SetActive(currentGun == WeaponType.Pistol);
        Pistol[1].SetActive(currentGun == WeaponType.Pistol);
        Rifle[0].SetActive(currentGun == WeaponType.Rifle);
        Rifle[1].SetActive(currentGun == WeaponType.Rifle);
        Shotgun[0].SetActive(currentGun == WeaponType.Shotgun);
        Shotgun[1].SetActive(currentGun == WeaponType.Shotgun);
    }


}
