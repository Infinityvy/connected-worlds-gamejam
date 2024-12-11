using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;

    public Camera cam {  get; private set; }
    private PlayerInputActions playerInput;

    private InputAction lookAction;

    private float lowerXBounds = 275f;
    private float upperXBounds = 85f;

    private void Awake()
    {
        Instance = this;

        playerInput = GameSession.Instance.playerInput;

        if (playerInput == null) throw new System.Exception("playerInput was null");


        cam = GetComponent<Camera>();
    }

    void Start()
    {
    }

    void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        Vector2 lookVector = lookAction.ReadValue<Vector2>();

        float factor = Controls.lookSensitvity * Time.deltaTime * 10f;

        lookVector *= factor;

        float newX = transform.rotation.eulerAngles.x - lookVector.y;
        float newY = transform.rotation.eulerAngles.y + lookVector.x;

        if (newX < lowerXBounds && newX > 180) newX = lowerXBounds;
        else if (newX > upperXBounds && newX < 180) newX = upperXBounds;

        transform.rotation = Quaternion.Euler(newX, newY, 0);
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public Quaternion GetHorizontalRotation()
    {
        return Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    private void OnEnable()
    {
        lookAction = playerInput.Player.Look;
        lookAction.Enable();
    }

    private void OnDisable()
    {
        lookAction.Disable();        
    }
}
