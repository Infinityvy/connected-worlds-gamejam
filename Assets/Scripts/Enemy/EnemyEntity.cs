using UnityEngine;

public class EnemyEntity : Entity
{
    [SerializeField]
    private Transform deathEffectPrefab;

    private bool dead = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public override bool DealDamage(float damage)
    {
        if (dead) return false;

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
        dead = true;

        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
