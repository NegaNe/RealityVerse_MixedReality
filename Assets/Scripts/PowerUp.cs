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
    DamageUp();
    break;
    }
}

private void HealthUp()
{
    AudioSource.PlayClipAtPoint(GameController.Instance.HealSound, transform.position);
    GameController.Instance.PlayerHealth+=20;
}

private void DamageUp()
{
    GunMuzzle[] weaponInstances = FindObjectsOfType<GunMuzzle>();

    foreach (var instance in weaponInstances)
    {
        // Increase the bullet damage by 20%
        float originalDamage = instance.BulletDamage;
        instance.BulletDamage += originalDamage * 0.2f;
        GameController.Instance.RageEffect(true);
        AudioSource.PlayClipAtPoint(GameController.Instance.RageSound, transform.position);

        // Start coroutine to reset the damage after 10 seconds
        StartCoroutine(ResetDamageAfterDelay(instance, originalDamage));
    }
}

private IEnumerator ResetDamageAfterDelay(GunMuzzle instance, float originalDamage)
{
    // Wait for 10 seconds
    yield return new WaitForSeconds(10f);

    // Reset the damage to the original value
    GameController.Instance.RageEffect(false);
    instance.BulletDamage = originalDamage;
}



}
