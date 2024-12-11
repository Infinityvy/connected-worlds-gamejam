using System.Collections;
using UnityEngine;

public class PowEffect : MonoBehaviour
{
    private float timeUntilDeath = 0.5f;
    private float sizeDecayRate = 3.0f;
    private bool shrink = false;

    private void Start()
    {
        StartCoroutine(nameof(DeathTimer));
        sizeDecayRate *= transform.localScale.x;
    }

    private void Update()
    {
        if (shrink)
            transform.localScale -= Vector3.one * sizeDecayRate * Time.deltaTime;
        else
            transform.localScale += Vector3.one * sizeDecayRate * Time.deltaTime;
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(timeUntilDeath * 0.2f);

        shrink = true;

        yield return new WaitForSeconds(timeUntilDeath * 0.8f);

        Destroy(gameObject);
    }
}
