using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DimensionChanger : MonoBehaviour
{
    public static DimensionChanger Instance;

    public Dimension currentDimension {  get; private set; } = Dimension.Blue;

    private PlayerInputActions playerInput;

    private InputAction jumpDimensionAction;
    private float jumpDimensionCooldown = 0.5f;
    private float timeWhenLastJumpedDimension = 0f;

    private List<DimensionBound> dimensionBounds;
    private List<Material> dimensionIndicators;


    private void Awake()
    {
        Instance = this;

        playerInput = GameSession.Instance.playerInput;

        if (playerInput == null) throw new System.Exception("playerInput was null");

        dimensionBounds = new List<DimensionBound>();
        dimensionIndicators = new List<Material>();
    }

    public void JumpDimension(InputAction.CallbackContext context)
    {
        if(Time.time - timeWhenLastJumpedDimension < jumpDimensionCooldown)
        {
            return;
        }

        timeWhenLastJumpedDimension = Time.time;

        if(currentDimension == Dimension.Blue)
        {
            currentDimension = Dimension.Red;

            gameObject.layer = LayerMask.NameToLayer("PlayerRed");
        }
        else
        {
            currentDimension = Dimension.Blue;

            gameObject.layer = LayerMask.NameToLayer("PlayerBlue");
        }

        foreach (DimensionBound dimBound in dimensionBounds)
        {
            dimBound.NotifyJump(currentDimension);
        }

        Color currentDimCol = GetDimensionColor(currentDimension);
        foreach(Material mat in dimensionIndicators)
        {
            mat.color = currentDimCol;
        }
    }

    public void RegisterDimensionBound(DimensionBound dimBound)
    {
        if(!dimensionBounds.Contains(dimBound)) dimensionBounds.Add(dimBound);
    }

    public void UnregisterDimensionBound(DimensionBound dimBound)
    {
        dimensionBounds.Remove(dimBound);
    }

    public void RegisterDimensionIndicator(Material material)
    {
        if(!dimensionIndicators.Contains(material)) dimensionIndicators.Add(material);
    }

    public void UnregisterDimensionIndicator(Material material)
    {
        dimensionIndicators.Remove(material);
    }

    private void OnEnable()
    {
        jumpDimensionAction = playerInput.Player.JumpDimension;
        jumpDimensionAction.Enable();
        jumpDimensionAction.performed += JumpDimension;
    }

    private void OnDisable()
    {
        jumpDimensionAction.Disable();
    }

    public static Color GetDimensionColor(Dimension dim)
    {
        switch(dim)
        {
            case Dimension.Blue:
                return new Color(0.439f, 0.961f, 0.812f);
            case Dimension.Red:
                return new Color(0.941f, 0.51f, 0.384f);
            default:
                return new Color(0, 1, 0);
        }
    }
}