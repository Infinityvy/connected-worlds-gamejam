using UnityEngine;

public class EnemyEntity : Entity
{
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public override bool DealDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) Die();
        return true;
    }

    public override float GetHealth()
    {
        return currentHealth;
    }

    protected override void Die()
    {
        Debug.Log("I DIED! :(");
    }
}
