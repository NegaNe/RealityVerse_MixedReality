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
    public float bulletSpeed = 3;
    public float BulletDamage;
    private bool IsFiring;
    public float BulletDestroyTime = 3f;
    private Transform spawnPoint;
    private AudioSource audiosrc;

    private List<GameObject> bulletPool = new List<GameObject>();
    public int poolSize = 10; 

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

    void Start()
    {
        audiosrc = GetComponent<AudioSource>();
        audiosrc.playOnAwake = false;
        audiosrc.spatialBlend = 1;
        audiosrc.spatialize = true;
        audiosrc.maxDistance = 10f;

        InitializeBulletPool();
    }

    void Update()
    {
        spawnPoint = transform;

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
        if (firingType == FireType.Auto && !IsFiring &&
            ((OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && weaponPos == WeaponPos.Right) ||
             (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && weaponPos == WeaponPos.Left) ||
             Input.GetKey(KeyCode.Space)))
        {
            StartCoroutine(AutoShoot());
        }
    }

    void InitializeBulletPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    GameObject GetBulletFromPool()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(true);
        bulletPool.Add(newBullet);
        return newBullet;
    }

    void SemiShoot()
    {
        GameObject bulletInstance = GetBulletFromPool();
        bulletInstance.transform.position = spawnPoint.position;
        bulletInstance.transform.rotation = spawnPoint.rotation;
        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();

        rb.velocity = spawnPoint.forward * bulletSpeed;
        audiosrc.PlayOneShot(GunManager.instance.PistolSound);

        StartCoroutine(DeactivateBulletAfterTime(bulletInstance, BulletDestroyTime));
    }

    IEnumerator AutoShoot()
    {
        IsFiring = true;

        while (true)
        {
            audiosrc.PlayOneShot(GunManager.instance.ShotgunSound);

            GameObject bulletInstance = GetBulletFromPool();
            bulletInstance.transform.position = spawnPoint.position;
            bulletInstance.transform.rotation = spawnPoint.rotation;
            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();

            rb.velocity = spawnPoint.forward * bulletSpeed;
            StartCoroutine(DeactivateBulletAfterTime(bulletInstance, BulletDestroyTime));

            yield return new WaitForSeconds(0.1f);

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
            GameObject bulletInstance = GetBulletFromPool();
            bulletInstance.transform.position = spawnPoint.position;
            bulletInstance.transform.rotation = spawnPoint.rotation;
            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();

            Vector3 spreadDirection = Quaternion.Euler(Random.insideUnitSphere * spreadAngle) * spawnPoint.forward;

            rb.velocity = spreadDirection * bulletSpeed;
            StartCoroutine(DeactivateBulletAfterTime(bulletInstance, BulletDestroyTime));
        }

        audiosrc.PlayOneShot(GunManager.instance.ShotgunSound);
    }
    IEnumerator DeactivateBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
    }
}
