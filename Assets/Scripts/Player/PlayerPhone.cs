using UnityEngine;

public class PlayerPhone : MonoBehaviour
{
    private Vector3 defaultPosition;
    private float swayStrength = 0.01f;
    private float maxSwayStep = 0.1f;

    private PlayerMovement playerMovement;

    private void Start()
    {
        defaultPosition = transform.localPosition;

        playerMovement = PlayerMovement.Instance;
    }

    private void Update()
    {
        AnimateSway();
    }

    private void AnimateSway()
    {
        Vector3 newLocalPos = defaultPosition - Quaternion.Inverse(PlayerCamera.Instance.GetRotation()) * playerMovement.getVelocity() * swayStrength;

        Vector3 difference = newLocalPos - transform.localPosition;

        if (difference.magnitude > maxSwayStep * Time.deltaTime) difference = difference.normalized * maxSwayStep * Time.deltaTime;

        transform.localPosition += difference;
    }
}
