using System.Collections;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    public PlayerInputActions playerInput {  get; private set; }

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
}