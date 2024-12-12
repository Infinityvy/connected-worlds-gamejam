using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camTransform;

    private void Start()
    {
        camTransform = PlayerCamera.Instance.cam.transform;
    }

    private void Update()
    {
        transform.LookAt(camTransform);

        transform.eulerAngles = new Vector3(-transform.eulerAngles.x, transform.eulerAngles.y + 180f, transform.eulerAngles.z);
    }
}
