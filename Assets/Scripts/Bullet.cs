using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private ParticleSystem destroyParticles;

    private Vector3 lastPosition;
    private LayerMask layerMask;

    private float damage = 1.0f;
    private float speed = 10.0f;
    private float lifetime = 5.0f;
    private float birthday = 0f;

    private bool initiated = false;
    private bool destroyed = false;

    public void Init(Dimension dimension, LayerMask layerMask, float damage = 1.0f, float speed = 10.0f, float lifetime = 5.0f)
    {
        lastPosition = transform.position;

        GetComponent<DimensionBound>().SetBoundDimension(dimension);
        this.layerMask = layerMask;
        this.damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;


        birthday = Time.time;

        initiated = true;
    }

    void Update()
    {
        if (!initiated) return;
        if(destroyed) return;  
        if(Time.time - birthday > lifetime)
        {
            Destroy();
            return;
        }

        Vector3 rayDirection = lastPosition - transform.position;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, rayDirection, rayDirection.magnitude, layerMask);

        if (hits.Length == 0)
        {
            transform.position = transform.position + transform.forward * Time.deltaTime * speed;
        }
        else
        {
            transform.position = hits[0].point;

            Entity entity;
            bool isEntity = hits[0].transform.TryGetComponent<Entity>(out entity);

            if (isEntity)
            {
                entity.DealDamage(damage);
                meshRenderer.enabled = false;
            }

            Destroy();
        }
    }

    private void Destroy()
    {
        destroyed = true;

        destroyParticles.Play();
    }
}