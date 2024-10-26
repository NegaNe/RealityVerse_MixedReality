using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class BulletParticle : MonoBehaviour
{
    // Start is called before the first frame update

        [SerializeField] private float destroyRadius = 0.05f;
        Collider[] hitColliders;


    void OnTriggerEnter(Collider collision)
    {

          if (collision.gameObject.layer == LayerMask.NameToLayer("Destructible"))
          {
              //GameObject debrisPrefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Count)];
              //GameObject debris = Instantiate(debrisPrefab, hit.point, Quaternion.identity);
              Destroy(collision.gameObject);
              //Destroy(debris, 3f);

              //AudioSource.PlayClipAtPoint(hitSound, hit.point);
           }
      }
    

    void OnCollisionEnter(Collision other)
    {
              if(other.gameObject.CompareTag("Enemy"))
      {
            Destroy(other.gameObject);
            Destroy(gameObject);
      }
    }

}
