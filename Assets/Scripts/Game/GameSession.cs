using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    public PlayerInputActions playerInput {  get; private set; }

    private InputAction resetAction;

    private void Awake()
    {
        Instance = this;

        playerInput = new PlayerInputActions();
    }

    void Start()
    {
        Controls.Init();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        resetAction = playerInput.Player.Reset;
        resetAction.Enable();
        resetAction.performed += ResetSession;
    }

    private void ResetSession(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Main");
    }
}