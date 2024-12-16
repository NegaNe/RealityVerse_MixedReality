using System.Collections;
using System.Collections.Generic;
using Meta.WitAi.Attributes;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using UnityEngine;
using Oculus.Interaction;

public class ControllerCast : MonoBehaviour
{
    // private GunManager gunManager;
    public GunDisplay gunDisplay { get; private set; }
    readonly float raycastDistance = 1f;
    private GameObject _hitObject;
    public Transform rayOrigin; 
    
    void Start()
    {
        rayOrigin = transform;

    }

void Update()
{
    CastRay();
    LevelChange();
    CheckForGunDisplay();
}



void CastRay()
{
    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, raycastDistance))
    {
        _hitObject = hit.collider.gameObject;
    }
}

void CheckForGunDisplay()
{
    if (_hitObject != null && _hitObject.layer == LayerMask.NameToLayer("GunDisplay") && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
    ChangeGun();
}

void ChangeGun() 
{
if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
{
    switch (_hitObject.transform.gameObject.tag)
        {
        
    case "DisplayPistol":
        GameController.Instance.GunTaken=true;
        GunManager.instance.ChangeGun(GunManager.WeaponType.
        Pistol);
        break;
    case "DisplayRifle":
            GameController.Instance.GunTaken=true;
        GunManager.instance.ChangeGun(GunManager.WeaponType.Rifle);
        break;
    case "DisplayShotgun":
            GameController.Instance.GunTaken=true;
        GunManager.instance.ChangeGun(GunManager.WeaponType.Shotgun);
        break;
            case "SecretGun":
            GameController.Instance.GunTaken=true;
        GunManager.instance.ChangeGun(GunManager.WeaponType.SecretGun);
        break;
                }
            }
        }

void LevelChange() //self-explanatory
{
    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, raycastDistance) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Button"))
    {
        var button = hit.collider.gameObject.GetComponent<ButtonScript>();

    if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger)){
        if (button.buttonType == ButtonScript.ButtonUsage.Changelevel)
        {
            button.ButtonLevel(button.LevelIndex);
        }
        else if (button.buttonType == ButtonScript.ButtonUsage.ReloadLevel)
        {
            button.ReloadLevel();
        }
        }
    }
    else
    {
        return;
    }
}
    }
