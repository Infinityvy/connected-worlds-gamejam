using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DimensionChanger : MonoBehaviour
{
    public static DimensionChanger Instance;

    private PlayerEntity playerEntity;

    public Dimension currentDimension {  get; private set; } = Dimension.Blue;

    private PlayerInputActions playerInput;

    private InputAction jumpDimensionAction;
    private float jumpDimensionCooldown = 0.8f;
    private float timeWhenLastJumpedDimension = 0f;

    private List<DimensionBound> dimensionBounds;
    private List<Material> dimensionIndicators;

    [SerializeField]
    private GameObject[] playerObjects;

    [SerializeField]
    private Volume globalVolume;
    private ChromaticAberration chromatic;
    private LensDistortion lensDist;

    private bool animateJump = false;
    private float chromaticIntensity = 0f;
    private float jumpSpeed = 6.0f;

    [SerializeField]
    private AudioSource audioSource;


    private void Awake()
    {
        Instance = this;

        playerInput = GameSession.Instance.playerInput;

        if (playerInput == null) throw new System.Exception("playerInput was null");

        dimensionBounds = new List<DimensionBound>();
        dimensionIndicators = new List<Material>();
    }

    private void Start()
    {
        playerEntity = PlayerEntity.Instance;

        globalVolume.profile.TryGet(out chromatic);
        globalVolume.profile.TryGet(out lensDist);

        JumpDimension();
    }

    private void Update()
    {
        if(animateJump)
        {
            chromaticIntensity = chromaticIntensity + jumpSpeed * Time.deltaTime;
            if (chromaticIntensity >= 1)
            {
                animateJump = false;
                chromaticIntensity = 1;
                JumpDimension();
            }

            chromatic.intensity.Override(chromaticIntensity);
            lensDist.intensity.Override(-chromaticIntensity * 0.6f);
        }
        else if(chromaticIntensity > 0f)
        {
            chromaticIntensity = Mathf.Clamp01(chromaticIntensity - jumpSpeed * Time.deltaTime);
            chromatic.intensity.Override(chromaticIntensity);
            lensDist.intensity.Override(-chromaticIntensity * 0.6f);
        }
    }

    public void InitiateDimensionJump(InputAction.CallbackContext context)
    {
        if (playerEntity.isFrozen) return;

        if (Time.time - timeWhenLastJumpedDimension < jumpDimensionCooldown)
        {
            return;
        }

        animateJump = true;
        audioSource.PlaySound("snap-echo", 0.5f);
    }

    private void JumpDimension()
    {
        timeWhenLastJumpedDimension = Time.time;

        if(currentDimension == Dimension.Blue)
        {
            currentDimension = Dimension.Red;

            foreach(GameObject obj in playerObjects)
            {
                obj.layer = LayerMask.NameToLayer("PlayerRed");
            }
        }
        else
        {
            currentDimension = Dimension.Blue;

            foreach (GameObject obj in playerObjects)
            {
                obj.layer = LayerMask.NameToLayer("PlayerBlue");
            }
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
        jumpDimensionAction.performed += InitiateDimensionJump;
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