using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerComponent : MonoBehaviour
{

void OnTriggerStay(Collider other)
{
if(other.gameObject.layer == LayerMask.NameToLayer("Button"))
    other.gameObject.GetComponent<Renderer>().material.color = Color.green;
    
    if (other != null && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Button"))LevelChange(other.gameObject);
        
        ChangeGun(other.gameObject);
    }   

}

void OnTriggerExit(Collider other)
{
if(other.gameObject.layer == LayerMask.NameToLayer("Button"))
    other.gameObject.GetComponent<Renderer>().material.color = Color.red;
}

void LevelChange(GameObject other) //self-explanatory
{
    var button = other.GetComponent<ButtonScript>();

        if (button.buttonType == ButtonScript.ButtonUsage.Changelevel)
        {
            button.ButtonLevel(button.LevelIndex);
        }
        else if (button.buttonType == ButtonScript.ButtonUsage.ReloadLevel)
        {
            button.ReloadLevel();
        }
    }
    
void ChangeGun(GameObject other) 
{
if(other.layer == LayerMask.NameToLayer("GunDisplay")){
    switch (other.transform.gameObject.tag)
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
                }
            }
        }
}
