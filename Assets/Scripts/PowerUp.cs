using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class PowerUp : MonoBehaviour
{

private enum PowerupType
{
    Health,
    Damage
}

[SerializeField]
private PowerupType type;

void OnCollisionEnter(Collision other)
{
    if(other.gameObject.CompareTag("Bullet"))
    {
        Destroy(gameObject);
    }
}

void OnDestroy()
{
    switch (type)
    {
    case PowerupType.Health:
    HealthUp();
    break;
    case PowerupType.Damage:
    GameController.Instance.DamageUp();
    break;
    }
}

private void HealthUp()
{
    AudioSource.PlayClipAtPoint(GameController.Instance.HealSound, transform.position);
    GameController.Instance.PlayerHealth+=20;
}
}
