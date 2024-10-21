using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDisplay : MonoBehaviour
{
    private GunManager gunManager;
    [Range(0.0f, 5f)]
    public float SpinSpeed;
    private bool WeaponTaken;

    public GameObject DisplayPistol, DisplayRifle, DisplayShotgun;

    private void Start() {
        gunManager = GetComponent<GunManager>();

        gunManager.GunType = GunManager.WeaponType.None;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, SpinSpeed,0);
    // Adjust the object's position to move up and down by 10cm
    float newY = transform.parent.position.y + Mathf.Sin(Time.time) * 0.1f; // 10cm = 0.1f in Unity units
    transform.position = new Vector3(transform.position.x, newY, transform.position.z);

    
    }

    public void TakeWeapon(bool WeaponTaken)
    {
        this.WeaponTaken = WeaponTaken;

        if(WeaponTaken)
        {
            Destroy(gameObject);
        }
    }



}
