using UnityEngine;

public class ZonerWarning : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;

    private Vector3 defaultScale;

    private bool animating = false;
    private float pulsingSpeed = 5f;
    private float pulsingAmplitude = 1.2f;

    void Start()
    {
        defaultScale = transform.localScale;
        meshRenderer.enabled = false;
    }

    void Update()
    {
        if(!animating) return;

        float newScale = defaultScale.x + Mathf.Sin(Time.time * pulsingSpeed) * pulsingAmplitude;

        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public void StartWarning()
    {
        animating = true;
        meshRenderer.enabled = true;
    }

    public void StopWarning()
    {
        animating = false;
        meshRenderer.enabled = false;
    }
}
