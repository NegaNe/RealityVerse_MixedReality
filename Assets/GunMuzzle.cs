using System.Collections;
using System.Collections.Generic;
using Oculus.Haptics;
using Unity.VisualScripting;
using UnityEngine;

public class GunMuzzle : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 5;
    private Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnPoint = transform;

        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        Shoot();


        
    }

    void Shoot()
    {
        // Instantiate the bullet at the spawn point
        GameObject bulletInstance = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
        
        // Apply force to the bullet in the direction of the spawn point's forward direction
        rb.velocity = spawnPoint.forward * bulletSpeed;

        // Optional: Destroy the bullet after some time to avoid cluttering the scene
        Destroy(bulletInstance, 5f);
    }
}
