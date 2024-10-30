using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {

          if (collision.gameObject.layer == LayerMask.NameToLayer("Destructible"))
          {
              Destroy(collision.gameObject);
           }
      }
    

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
        GameController.Instance.PlayerHealth-=5;
        Destroy(gameObject);
        }
    }
}
