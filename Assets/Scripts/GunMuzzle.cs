using System.Collections;
using System.Collections.Generic;
using Oculus.Haptics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

[RequireComponent(typeof(AudioSource))]

public class GunMuzzle : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float bulletSpeed = 5;
    public float BulletDamage;
    private bool IsFiring;
    public float BulletDestroyTime=3f;    
    private Transform spawnPoint;
    private AudioSource audiosrc;

    public enum FireType
    {
    Semi,
    Auto,
    Shotgun
    }

    public enum WeaponPos
    {
    Left,
    Right
    }

    public FireType firingType;
    [SerializeField]
    private WeaponPos weaponPos;
    // Update is called once per frame


    void Start()
    {
        audiosrc = GetComponent<AudioSource>();
        audiosrc.playOnAwake=false;
        audiosrc.spatialBlend=1;
        audiosrc.spatialize=true;
        audiosrc.maxDistance=10f;

    }

void Update()
{
    spawnPoint = transform;

    // For semi and shotgun firing
    if ((OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && weaponPos == WeaponPos.Right) ||
        (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) && weaponPos == WeaponPos.Left) ||
        Input.GetKeyDown(KeyCode.Space))
    {
        if (firingType == FireType.Semi)
        {
            SemiShoot();
        }
        else if (firingType == FireType.Shotgun)
        {
            Shotgun();
        }
    }

    // For automatic firing only
    if (firingType == FireType.Auto && !IsFiring &&
        ((OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && weaponPos == WeaponPos.Right) ||
         (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && weaponPos == WeaponPos.Left) ||
         Input.GetKey(KeyCode.Space)))
    {
        StartCoroutine(AutoShoot());
    }
}


    void SemiShoot()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();

        rb.velocity = spawnPoint.forward * bulletSpeed;
        audiosrc.PlayOneShot(GunManager.instance.PistolSound);
        Destroy(bulletInstance, 3f);
    }
    IEnumerator AutoShoot()
    {
        IsFiring = true;

        while (true)
        {
        audiosrc.PlayOneShot(GunManager.instance.ShotgunSound);

            GameObject bulletInstance = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
            
            rb.velocity = spawnPoint.forward * bulletSpeed;
            Destroy(bulletInstance, 2f);

            yield return new WaitForSeconds(0.3f);

            if (!OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && !OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
            {
                StopAllCoroutines();
                IsFiring = false;
                break;
            }
        }
    }

    void Shotgun()
{
    int pelletCount = 3; 
    float spreadAngle = 15f; 

    for (int i = 0; i < pelletCount; i++)
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();

        Vector3 spreadDirection = Quaternion.Euler(
            Random.insideUnitSphere * spreadAngle
        ) * spawnPoint.forward;

        rb.velocity = spreadDirection * bulletSpeed;

        Destroy(bulletInstance, BulletDestroyTime);
    }
        audiosrc.PlayOneShot(GunManager.instance.ShotgunSound);

}
}
