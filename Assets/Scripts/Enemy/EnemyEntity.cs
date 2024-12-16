using System;
using UnityEngine;

public class EnemyEntity : Entity
{
    [SerializeField]
    private Transform deathEffectPrefab;

    [SerializeField]
    private AudioSource audioSource;

    public int killValue;

    private bool dead = false;

    private void Start()
    {
        EnemyDirector.Instance.RegisterEnemy(this);
        currentHealth = maxHealth;
    }

    public override bool DealDamage(float damage)
    {
        if (dead) return false;

        if (damage > 0) audioSource.PlaySound("hit", 0.5f);

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
        Score.Instance.IncrementScore(killValue);

        EnemyDirector.Instance.UnregisterEnemy(this);

        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
