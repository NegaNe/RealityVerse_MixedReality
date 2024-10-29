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

    [SerializeField]
    private GameObject rend;
    readonly float raycastDistance = 0.4f;
    private GameObject _hitObject;
    private Color OriginalColor;


    
    // Start is called before the first frame update
    void Start()
    {
        var buttonScript = FindAnyObjectByType<ButtonScript>();
        if (buttonScript != null && buttonScript.TryGetComponent(out Renderer renderer))
        {
            OriginalColor = renderer.material.color;
        }
        // gunManager = FindObjectOfType<GunManager>();
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
    Debug.DrawRay(new Vector3(transform.position.x , transform.position.y, transform.position.z+.1f), transform.forward * .05f, Color.red);
    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, raycastDistance))
    {
        _hitObject = hit.collider.gameObject;
    }
}

void CheckForGunDisplay()
{
    if (_hitObject != null && _hitObject.layer == LayerMask.NameToLayer("GunDisplay") && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
    ChangeGun();
}


void ChangeGun() //change gun from here, raycast take, raycast send data.
{
    GameController.Instance.GunTaken=true;
    GameController.Instance.StartGame=true;
if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
{
    switch (_hitObject.transform.gameObject.tag)
        {
    case "DisplayPistol":
        GunManager.instance.ChangeGun(GunManager.WeaponType.
        Pistol);
        break;
    case "DisplayRifle":
        GunManager.instance.ChangeGun(GunManager.WeaponType.Rifle);
        break;
    case "DisplayShotgun":
        GunManager.instance.ChangeGun(GunManager.WeaponType.Shotgun);
        break;
                }
            }
        }

void LevelChange() //self-explanatory
{
    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, raycastDistance) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Button"))
    {
        var button = hit.collider.gameObject.GetComponent<ButtonScript>();

    if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)){
        if (button.buttonType == ButtonScript.ButtonUsage.Changelevel)
        {
            button.ButtonLevel(button.LevelIndex);
        }
        else if (button.buttonType == ButtonScript.ButtonUsage.ReloadLevel)
        {
            button.ReloadLevel();
            rend = hit.collider.gameObject;
            rend.GetComponent<Renderer>().material.color = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) ? Color.green : OriginalColor;
        }
        }
    }
    else
    {
        return;
    }
}
    

    }
