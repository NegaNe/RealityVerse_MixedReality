using System.Collections;
using System.Collections.Generic;
using Meta.WitAi.Attributes;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using UnityEngine;

public class ControllerCast : MonoBehaviour
{
    private GunManager gunManager;
    public GunDisplay gunDisplay { get; private set; }

    [SerializeField]
    private GameObject rend;
    float raycastDistance = 0.4f;
    private GameObject _hitObject;

    
    // Start is called before the first frame update
    void Start()
    {
        gunManager = FindObjectOfType<GunManager>();
    }

    // Update is called once per frame

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
    if (_hitObject != null && _hitObject.layer == LayerMask.NameToLayer("GunDisplay"))
    {
        ChangeGun();
    }
}


void ChangeGun()
{
    GameController.Instance.GunTaken=true;
    GameController.Instance.StartGame=true;

    switch (_hitObject.transform.gameObject.tag)
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


void LevelChange()
{
try{
    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, raycastDistance) )
    {
        if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Button"))
        {
        rend = _hitObject;

                if(hit.collider.gameObject && hit.collider.gameObject.GetComponent<ButtonScript>().buttonType == ButtonScript.ButtonUsage.Changelevel)
                
                hit.collider.gameObject.GetComponent<ButtonScript>().ButtonLevel(hit.collider.gameObject.GetComponent<ButtonScript>().LevelIndex);        
                }

                if(hit.collider.gameObject.GetComponent<ButtonScript>().buttonType == ButtonScript.ButtonUsage.ReloadLevel)
                {
                hit.collider.gameObject.GetComponent<ButtonScript>().ReloadLevel();
                    if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) )
                    {

                    rend.GetComponent<Renderer>().material.color = Color.green;

                    }
                    else
                    {
                    rend.GetComponent<Renderer>().material.color = Color.red;
                    }
            } else
            {
            return;
            }
            
        }
    } catch {
    //ignored
    }}
}