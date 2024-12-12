using UnityEngine;

public class EyeGuyAI : MonoBehaviour
{
    private Transform playerTansform;

    void Start()
    {
        playerTansform = PlayerMovement.Instance.transform;
    }

    void Update()
    {
        transform.LookAt(playerTansform);
    }
}
