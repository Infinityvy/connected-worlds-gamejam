using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerEntity : Entity
{
    public static PlayerEntity Instance { get; private set; }

    [SerializeField]
    private Volume globalVolume;

    private Vignette vignette;

    private bool dead = false;
    public bool isFrozen = false;

    private float healRate = 5.0f;


    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;

        globalVolume.profile.TryGet(out vignette);

        if (vignette == null) throw new System.Exception("no vignette found :(");
    }

    private void Update()
    {
        if(dead) return;    

        if (transform.position.y < -70) Die();

        if(currentHealth < maxHealth) DealDamage(-healRate * Time.deltaTime);
    }

    public override bool DealDamage(float damage)
    {
        if (dead) return false;

        if(damage > 0) audioSource.PlaySound("oof" + Random.Range(0, 3).ToString(), 0.2f);

        currentHealth -= damage;
        vignette.intensity.Override(0.5f - 0.5f * (currentHealth / maxHealth));
        if (currentHealth < 0) Die();
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        return true;
    }

    public override float GetHealth()
    {
        return currentHealth;
    }

    protected override void Die()
    {
        audioSource.PlaySound("deathmoan", 0.5f);
        dead = true;
        Time.timeScale = 0.5f;
        vignette.intensity.Override(0.5f);
        PlayerEntity.Instance.isFrozen = true;
        Invoke(nameof(ResetSession), 1f);
    }

    private void ResetSession()
    {
        SceneManager.LoadScene("Main");
    }
}
