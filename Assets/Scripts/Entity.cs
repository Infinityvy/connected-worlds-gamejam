using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected float maxHealth = 100.0f;
    protected float currentHealth;


    public abstract bool DealDamage(float damage);
    public abstract float GetHealth();
    protected abstract void Die();
}
