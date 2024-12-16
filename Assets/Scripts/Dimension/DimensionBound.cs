using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DimensionBound : MonoBehaviour
{
    [SerializeField]
    private Dimension boundDimension;

    [SerializeField]
    private ObjectType type;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private int dimMatIndex;
    private Material dimMat;

    [SerializeField]
    private Material wrongDimMat;

    private Material[] materials;
    private Material[] materialsWrongDim;

    private bool started = false;

    private void Awake()
    {
        materials = meshRenderer.materials;
    }

    private void Start()
    {
        Color dimCol = DimensionChanger.GetDimensionColor(boundDimension);

        dimMat = materials[dimMatIndex];
        dimMat.color = dimCol;

        materialsWrongDim = new Material[materials.Length];

        for (int i = 0; i < materialsWrongDim.Length; i++) materialsWrongDim[i] = wrongDimMat;

        DimensionChanger.Instance.RegisterDimensionBound(this);

        NotifyJump(DimensionChanger.Instance.currentDimension);

        SetLayer();

        started = true;
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

        SetLayer();

        if(started) NotifyJump(DimensionChanger.Instance.currentDimension);
    }

    public Dimension GetBoundDimension()
    {
        return boundDimension;
    }

    private void SetLayer()
    {
        if (type == ObjectType.None) return;

        gameObject.layer = LayerMask.NameToLayer(type.ToString() + boundDimension.ToString());
    }
}