using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class BulletParticle : MonoBehaviour
{
    // Start is called before the first frame update

        [SerializeField] private LayerMask destroyableLayerMask;
        [SerializeField] private float destroyRadius = 0.05f;
        Collider[] hitColliders;


    void OnCollisionEnter(Collision collision)
    {
       Collider[] hitColliders = Physics.OverlapSphere(transform.position, destroyRadius, destroyableLayerMask);
       foreach (Collider hitCollider in hitColliders)
       {
          if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Destructible"))
          {
              //GameObject debrisPrefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Count)];
              //GameObject debris = Instantiate(debrisPrefab, hit.point, Quaternion.identity);
              Destroy(hitCollider.gameObject);
              //Destroy(debris, 3f);

              //AudioSource.PlayClipAtPoint(hitSound, hit.point);
           }
      }

      if(collision.gameObject.CompareTag("Enemy"))
      {
            Destroy(collision.gameObject);
      }

    }
}
