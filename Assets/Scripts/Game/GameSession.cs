using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public Material mat;

    public static GameSession Instance { get; private set; }

    public PlayerInputActions playerInput {  get; private set; }

    private InputAction resetAction;

    private void Awake()
    {
        Instance = this;

        playerInput = new PlayerInputActions();

        Time.timeScale = 1.0f;
    }

    void Start()
    {
        Controls.Init();
        Cursor.lockState = CursorLockMode.Locked;
        mat.color = Color.white;
    }

    private void OnEnable()
    {
        resetAction = playerInput.Player.Reset;
        resetAction.Enable();
        resetAction.performed += ResetSession;
    }

    public void ResetSession(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Main");
    }
}