using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    private PlayerInputActions playerInput;

    private InputAction leftClickAction;

    void Awake()
    {
        playerInput = new PlayerInputActions();
    }

    private void StartGame(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Main");
    }

    private void OnEnable()
    {
        leftClickAction = playerInput.Player.Fire;
        leftClickAction.Enable();
        leftClickAction.performed += StartGame;
    }

    private void OnDisable()
    {
        leftClickAction.Disable();
    }
}
