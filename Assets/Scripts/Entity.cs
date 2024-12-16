using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float maxHealth;
    protected float currentHealth;


    public abstract bool DealDamage(float damage);
    public abstract float GetHealth();
    protected abstract void Die();
}
