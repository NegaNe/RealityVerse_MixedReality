using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCast : MonoBehaviour
{
    private GunManager gunManager;
    public GunDisplay gunDisplay { get; private set; }
    
    
    // Start is called before the first frame update
    void Start()
    {
        gunManager = FindObjectOfType<GunManager>();
    }

    // Update is called once per frame

void Update()
{
    float raycastDistance = 0.3f;

if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, raycastDistance) && hit.collider.gameObject.layer == LayerMask.NameToLayer("GunDisplay"))
{

    GameController.Instance.GunTaken=true;
    GameController.Instance.StartGame=true;

    switch (hit.transform.gameObject.tag)
{
    case "DisplayPistol":
        gunManager.ChangeGun(GunManager.WeaponType.
        Pistol);
        break;
    case "DisplayRifle":
        gunManager.ChangeGun(GunManager.WeaponType.Rifle);
        break;
    case "DisplayShotgun":
        gunManager.ChangeGun(GunManager.WeaponType.Shotgun);
        break;
}
    
}
}
    
}
