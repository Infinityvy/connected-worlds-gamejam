using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DimensionBound : MonoBehaviour
{
    [SerializeField]
    private Dimension boundDimension;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material wrongDimMat;

    private Material[] materials;
    private Material[] materialsWrongDim;

    private void Awake()
    {
        materials = meshRenderer.materials;
    }

    private void Start()
    {
        Color dimCol = DimensionChanger.GetDimensionColor(boundDimension);

        materials[0].color = dimCol;

        materialsWrongDim = new Material[materials.Length];

        for (int i = 0; i < materialsWrongDim.Length; i++) materialsWrongDim[i] = wrongDimMat;

        DimensionChanger.Instance.RegisterDimensionBound(this);

        NotifyJump(DimensionChanger.Instance.currentDimension);
    }

    private void OnDestroy()
    {
        DimensionChanger.Instance.UnregisterDimensionBound(this);
    }

    public void NotifyJump(Dimension newDimension)
    {
        if (boundDimension == newDimension)
        {
            meshRenderer.materials = materials;
        }
        else
        {
            meshRenderer.materials = materialsWrongDim;
        }
    }

    public void SetBoundDimension(Dimension dimension)
    {
        boundDimension = dimension;

        NotifyJump(DimensionChanger.Instance.currentDimension);
    }
}