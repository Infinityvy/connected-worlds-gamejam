using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camTransform;

    private Quaternion rotAdjustment = Quaternion.Euler(0, 180.0f, 0);

    private void Start()
    {
        camTransform = PlayerCamera.Instance.cam.transform;
    }

    private void Update()
    {
        transform.LookAt(camTransform);

        transform.rotation = rotAdjustment * transform.rotation;
    }
}
