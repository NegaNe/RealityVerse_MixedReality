using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Goop Data", menuName = "Goop Data")]
public class GoopData : ScriptableObject
{
    public int Health;
    public int Damage;
    public float Speed;
    
}
