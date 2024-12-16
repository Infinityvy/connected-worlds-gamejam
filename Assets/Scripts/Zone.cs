using System.Collections;
using UnityEngine;

public class Zone : MonoBehaviour
{
    private bool destroyed = false;
    private bool disarmed = false;

    private Dimension boundDimension;

    private float damage;
    private float speed;
    private float lifetime;
    private float birthday;

    private Transform playerTransform;

    public void Init(Dimension dimension, float damage, float speed, float lifetime)
    {
        playerTransform = PlayerMovement.Instance.transform;

        this.boundDimension = dimension;
        GetComponent<DimensionBound>().SetBoundDimension(dimension);

        this.damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;

        birthday = Time.time;
    }

    void Update()
    {
        if(destroyed) return;

        if (Time.time - birthday > lifetime)
        {
            Destroy();
            return;
        }

        transform.localScale += Vector3.one * speed * Time.deltaTime;

        if (disarmed) return;

        if(transform.localScale.x * 0.5f >= Vector3.Distance(transform.position, playerTransform.position))
        {
            disarmed = true;
            if (boundDimension == DimensionChanger.Instance.currentDimension) PlayerEntity.Instance.DealDamage(damage);
        }
    }

    private void Destroy()
    {
        destroyed = true;

        Destroy(gameObject);
    }
}