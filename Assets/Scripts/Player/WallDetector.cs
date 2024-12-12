using UnityEngine;

public class WallDetector : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = PlayerMovement.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        playerMovement.isColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerMovement.isColliding = false;
    }
}
