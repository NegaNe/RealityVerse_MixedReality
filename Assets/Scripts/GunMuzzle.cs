using System.Collections;
using System.Collections.Generic;
using Oculus.Haptics;
using Unity.VisualScripting;
using UnityEngine;


public class GunMuzzle : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 5;
    public float BulletDamage;
    private bool IsFiring;
    public float BulletDestroyTime=3f;    
    private Transform spawnPoint;
    // Start is called before the first frame update

    // Rifle rifle = new Rifle();

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

    [SerializeField]
    private FireType firingType;
    [SerializeField]
    private WeaponPos weaponPos;
    // Update is called once per frame
    void Update()
    {
        spawnPoint = transform;

    if(!IsFiring)
    {
        if(firingType == FireType.Semi){

        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && weaponPos == WeaponPos.Right)
        SemiShoot();
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) && weaponPos == WeaponPos.Left)
        SemiShoot();
        }

        if(firingType == FireType.Shotgun){

        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && weaponPos == WeaponPos.Right)
        Shotgun();
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) && weaponPos == WeaponPos.Left)
        Shotgun();
        }

        if(firingType == FireType.Auto){
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && weaponPos == WeaponPos.Right)
        StartCoroutine(AutoShoot());
        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && weaponPos == WeaponPos.Left)
        StartCoroutine(AutoShoot());
        }
}   
    }

    void SemiShoot()
    {
        // Instantiate the bullet at the spawn point
        GameObject bulletInstance = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
        
        // Apply force to the bullet in the direction of the spawn point's forward direction
        rb.velocity = spawnPoint.forward * bulletSpeed;

        // Optional: Destroy the bullet after some time to avoid cluttering the scene
        Destroy(bulletInstance, 3f);
    }
    IEnumerator AutoShoot()
    {
        IsFiring = true;

            GameObject bulletInstance = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
            
            // Apply force to the bullet in the direction of the spawn point's forward direction
            rb.velocity = spawnPoint.forward * bulletSpeed;

            GameController.Instance.audioSource.PlayOneShot(GameController.Instance.RifleSound);
            // Optional: Destroy the bullet after some time to avoid cluttering the scene
            Destroy(bulletInstance, 2f);


            yield return new WaitForSeconds(0.1f);
            IsFiring=false;
    }

    void Shotgun()
{
    int pelletCount = 5; // Number of pellets to fire
    float spreadAngle = 15f; // Cone angle spread

    for (int i = 0; i < pelletCount; i++)
    {
        // Instantiate each pellet at the spawn point
        GameObject bulletInstance = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();

        // Calculate a spread direction within the cone angle
        Vector3 spreadDirection = Quaternion.Euler(
            Random.Range(-spreadAngle, spreadAngle),
            Random.Range(-spreadAngle, spreadAngle),
            0
        ) * spawnPoint.forward;

        // Apply force in the spread direction
        rb.velocity = spreadDirection * bulletSpeed;

        // Optional: Destroy the bullet after some time to avoid cluttering the scene
        Destroy(bulletInstance, BulletDestroyTime);
    }
}
}
