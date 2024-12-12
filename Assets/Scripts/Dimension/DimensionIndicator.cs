using UnityEngine;

public class DimensionIndicator : MonoBehaviour
{
    [SerializeField]
    private Material material;

    private void Start()
    {
        DimensionChanger.Instance.RegisterDimensionIndicator(material);
    }
}
