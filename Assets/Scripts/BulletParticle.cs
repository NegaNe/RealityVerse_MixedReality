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
            gameObject.SetActive(false); 
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); 
            gameObject.SetActive(false); 
        }
    }
}
