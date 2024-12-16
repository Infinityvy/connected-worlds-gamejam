using UnityEngine;
using UnityEngine.InputSystem;

public class MenuControls : MonoBehaviour
{
    private PlayerInputActions playerInput;

    private InputAction pauseAction;

    [SerializeField]
    private GameObject pauseMenu;

    private bool paused = false; 
    private float timeScaleCash = 1f;

    private void Awake()
    {
        playerInput = GameSession.Instance.playerInput;

        if (playerInput == null) throw new System.Exception("playerInput was null");
    }
    private void Pause(InputAction.CallbackContext context)
    {
        if(paused)
        {
            paused = false;
            Time.timeScale = timeScaleCash;
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            PlayerEntity.Instance.isFrozen = false;
        }
        else
        {
            paused = true;
            timeScaleCash = Time.timeScale;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            PlayerEntity.Instance.isFrozen = true;
        }
    }

    private void OnEnable()
    {
        pauseAction = playerInput.Player.Pause;
        pauseAction.Enable();
        pauseAction.performed += Pause;
    }

    private void OnDisable()
    {
        pauseAction.Disable();
    }

    
}
