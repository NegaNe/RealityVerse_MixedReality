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
    readonly float raycastDistance = 1f;
    private GameObject _hitObject;
    private Color OriginalColor;

    public Transform rayOrigin; 
    private LineRenderer lineRenderer;
    
    void Start()
    {
        rayOrigin = transform;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

void Update()
{
    DrawRayCast();
    CastRay();
    LevelChange();
    CheckForGunDisplay();
}

void DrawRayCast()
{
        RaycastHit hit;
        Vector3 direction = rayOrigin.forward;

        if (Physics.Raycast(rayOrigin.position, direction, out hit, raycastDistance))
        {
            // Visualize the ray only in the build
#if !UNITY_EDITOR
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, rayOrigin.position);
            lineRenderer.SetPosition(1, hit.point);
#endif
            // Handle hit object behavior here
            Debug.Log("Hit: " + hit.collider.name);
        }
        else
        {
            // Hide the line when no hit occurs
            lineRenderer.enabled = true;
        }
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
