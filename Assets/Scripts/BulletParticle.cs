using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticle : MonoBehaviour
{
    [SerializeField] private float destroyRadius = 0.05f;
    private TrailRenderer trailRenderer;

    void Awake()
    {

        trailRenderer = GetComponent<TrailRenderer>();
    }

    void OnEnable()
    {
 
        if (trailRenderer != null)
        {
            trailRenderer.Clear(); 
            trailRenderer.enabled = true;
        }
    }

    void OnDisable()
    {
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Destructible"))
        {
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            try{
            other.gameObject.GetComponent<EnemyGoop>().Health -=GunManager.instance.GunDamage;
            other.gameObject.GetComponent<FlyingGoop>().health-=GunManager.instance.GunDamage;
            gameObject.SetActive(false);
            }
            catch
            {
            return;
            } 
        }
    }
}
