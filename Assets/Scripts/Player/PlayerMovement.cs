using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    public bool isGrounded { get; private set; } = false;

    [SerializeField]
    private LayerMask layerMaskGroundBlue;

    [SerializeField]
    private LayerMask layerMaskGroundRed;

    private Rigidbody playerRigid;

    private PlayerInputActions playerInput;

    private InputAction moveAction;
    private float acceleration = 1000f;
    private float deceleration = 350f;
    private float maxSpeed = 12.0f;

    private InputAction jumpAction;
    private float jumpForce = 6.5f;
    private float jumpCooldown = 0.1f;
    private float timeWhenLastJumped = 0f;
    private float timeWhenLastGrounded = 0f;

    private InputAction dashAction;
    private float dashForce = 64.0f;
    private float dashDuration = 0.18f;
    private float dashCooldown = 0.6f;
    private float timeWhenLastDashed = 0f;

    private void Awake()
    {
        playerInput = GameSession.Instance.playerInput;

        if (playerInput == null) throw new System.Exception("playerInput was null");

        Instance = this;
    }

    private void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        setIfGrounded();

        Move();
        Jump();
    }

    private void Move()
    {
        Vector3 direction = moveAction.ReadValue<Vector2>().convertTo3DMovement();

        Vector3 forceVector = PlayerCamera.Instance.GetHorizontalRotation() * direction.normalized * Time.deltaTime;

        Vector3 horizontalVelocity = new Vector3(playerRigid.linearVelocity.x, 0, playerRigid.linearVelocity.z);

        if (forceVector != Vector3.zero
            && horizontalVelocity.magnitude < maxSpeed)
        {
            playerRigid.AddForce(acceleration * forceVector);
        }
        else if ((playerRigid.linearVelocity.x != 0 || playerRigid.linearVelocity.z != 0))
        {
            Vector3 reductionVector = deceleration * Time.deltaTime * -horizontalVelocity * (isGrounded ? 1 : 0.5f);

            playerRigid.AddForce(reductionVector);
        }
    }

    private void Jump()
    {
        if (!jumpAction.IsPressed()) return;

        if (isGrounded
            && Time.time - timeWhenLastJumped > jumpCooldown
            && Time.time - timeWhenLastGrounded > 0.01f)
        {
            timeWhenLastJumped = Time.time;
            playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (Time.time - timeWhenLastDashed < dashCooldown) return;

        timeWhenLastDashed = Time.time;

        Vector3 direction = moveAction.ReadValue<Vector2>().convertTo3DMovement().normalized;

        Quaternion horizontalRot = PlayerCamera.Instance.GetHorizontalRotation();

        Vector3 dashDirection = horizontalRot * (direction.magnitude == 0 ? transform.forward : direction);

        float tmpDashForce = dashForce;
        if (!isGrounded) tmpDashForce *= 0.5f;

        playerRigid.AddForce(dashDirection * tmpDashForce, ForceMode.Impulse);

        Invoke(nameof(decelerateDash), dashDuration);
    }
    private void decelerateDash()
    {
        Vector3 horizontalVelocity = new Vector3(playerRigid.linearVelocity.x, 0, playerRigid.linearVelocity.z);

        if (horizontalVelocity.magnitude < maxSpeed) return;

        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeed);
        playerRigid.linearVelocity = new Vector3(horizontalVelocity.x, playerRigid.linearVelocity.y, horizontalVelocity.z);
    }

    private void setIfGrounded()
    {
        LayerMask currentGroundMask = (DimensionChanger.Instance.currentDimension == Dimension.Blue ? layerMaskGroundBlue : layerMaskGroundRed);

        bool currentIsGrounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.05f, currentGroundMask);

        if(!isGrounded && currentIsGrounded)
        {
            timeWhenLastGrounded = Time.time;

            // code executed on landing
        }

        isGrounded = currentIsGrounded;
    }

    public Vector3 getVelocity()
    {
        return playerRigid.linearVelocity;
    }

    private void OnEnable()
    {
        moveAction = playerInput.Player.Move;
        moveAction.Enable();

        jumpAction = playerInput.Player.Jump;
        jumpAction.Enable();

        dashAction = playerInput.Player.Dash;
        dashAction.Enable();
        dashAction.performed += Dash;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        dashAction.Disable();
    }
}
