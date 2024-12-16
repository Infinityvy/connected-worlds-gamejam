using UnityEngine;

public class WallDetector : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private int objectsInsideTrigger = 0;

    [SerializeField]
    private Collider col;

    private void Start()
    {
        playerMovement = PlayerMovement.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Entity>() != null) return;

        objectsInsideTrigger++;

        playerMovement.SetIsColliding(true);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Entity>() != null) return;

        objectsInsideTrigger--;

        if(objectsInsideTrigger < 0) objectsInsideTrigger = 0;

        if (objectsInsideTrigger == 0)
        {
            playerMovement.SetIsColliding(false);
        }
    }
}
