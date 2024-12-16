using UnityEngine;

public class WorldBlock : MonoBehaviour
{
    private bool dormant = true;
    private float height = 5.0f;
    private float transformationSpeed = 12.0f;
    private float errorMargin = 0.1f;

    private void Update()
    {
        if (dormant) return;


        if(!HeightWithinErrorMargin())
        {
            Vector3 direction = (transform.localScale.y < height ? 1 : -1) * Vector3.up;

            transform.localScale += direction * Time.deltaTime * transformationSpeed;

            Vector3 newDirection = (transform.localScale.y < height ? 1 : -1) * Vector3.up;

            if(newDirection != direction) BecomeDormant();
        }
        else
        {
            BecomeDormant();
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void SetHeight(float height)
    {
        dormant = false;
        this.height = height;
    }

    public bool IsDormant()
    {
        return dormant;
    }

    private bool HeightWithinErrorMargin()
    {
        if(Mathf.Abs(transform.localScale.y - height) <= errorMargin)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void BecomeDormant()
    {
        transform.localScale = new Vector3(transform.localScale.x, height, transform.localScale.z);
        dormant = true;
    }
}
