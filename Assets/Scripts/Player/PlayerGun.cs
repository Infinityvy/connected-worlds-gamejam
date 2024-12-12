using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    private Transform bulletPrefab;

    [SerializeField]
    private Transform muzzleFlash;
    private bool shrinkMuzzleFlash = false;

    [SerializeField]
    private ParticleSystem smoke;

    [SerializeField]
    private AudioSource shotSound;

    private PlayerInputActions playerInput;

    private InputAction fireAction;
    private float fireCooldown = 0.4f;
    private float timeWhenLastFired = 0f;
    private int bulletCount = 16;
    private float bulletDamage = 10.0f;
    private float bulletSpeed = 50.0f;
    private float spreadAngle = 3.0f;

    private Vector3 defaultPosition;
    private float swayStrength = 0.01f;
    private float maxSwayStep = 0.1f;
    private float recoilRecoverySpeed = 75.0f;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerInput = GameSession.Instance.playerInput;

        if (playerInput == null) throw new System.Exception("playerInput was null");
    }

    private void Start()
    {
        defaultPosition = transform.localPosition;

        playerMovement = PlayerMovement.Instance;
    }

    private void Update()
    {
        AnimateSway();
        RecoverRecoil();

        if(shrinkMuzzleFlash)
        {
            muzzleFlash.localScale -= Vector3.one * Time.deltaTime * 3.5f;
        }
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (Time.time - timeWhenLastFired < fireCooldown) return;

        timeWhenLastFired = Time.time;
        smoke.Play();
        shotSound.PlaySound("shotgun-shot");

        transform.localRotation = Quaternion.Euler(330.0f, 0, 0);

        StartCoroutine(nameof(flashMuzzleFlash));

        Dimension currentDim = DimensionChanger.Instance.currentDimension;

        LayerMask layerMask;

        if(currentDim == Dimension.Blue)
        {
            layerMask = LayerMask.GetMask("EnemyBlue", "GroundBlue", "WallBlue", "UniversalGround");
        }
        else
        {
            layerMask = LayerMask.GetMask("EnemyRed", "GroundRed", "WallRed", "UniversalGround");
        }

        Quaternion camRotation = PlayerCamera.Instance.GetRotation();
        Vector3 bulletSpawnPos = PlayerCamera.Instance.transform.position;

        for (int i = 0; i < bulletCount; i++)
        {
            float xOffsetAngle = Random.Range(-spreadAngle, spreadAngle);
            float yOffsetAngle = Random.Range(-spreadAngle, spreadAngle);
            float zOffsetAngle = Random.Range(-spreadAngle, spreadAngle);

            Quaternion offsetRot = Quaternion.Euler(xOffsetAngle, yOffsetAngle, zOffsetAngle);

            Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPos, offsetRot * camRotation).GetComponent<Bullet>();

            bullet.Init(currentDim, layerMask, bulletDamage, bulletSpeed, 2.5f);
        }
    }

    private void AnimateSway()
    {
        Vector3 newLocalPos = defaultPosition - Quaternion.Inverse(PlayerCamera.Instance.GetRotation()) * playerMovement.getVelocity() * swayStrength;

        Vector3 difference = newLocalPos - transform.localPosition;

        if(difference.magnitude > maxSwayStep * Time.deltaTime) difference = difference.normalized * maxSwayStep * Time.deltaTime;

        transform.localPosition += difference;
    }

    private void RecoverRecoil()
    {
        Vector3 localRotation = transform.localRotation.eulerAngles;

        if (localRotation.x == 0) return;

        int direction = (localRotation.x < 180 ? -1 : 1);

        transform.Rotate(direction * localRotation.normalized * Time.deltaTime * recoilRecoverySpeed, Space.Self);

        int newDirection = (transform.localRotation.eulerAngles.x < 180 ? -1 : 1);

        if(newDirection != direction) transform.localRotation = Quaternion.identity;
    }

    private IEnumerator flashMuzzleFlash()
    {
        muzzleFlash.Rotate(Vector3.forward, Random.Range(0f, 90f), Space.Self);
        muzzleFlash.gameObject.SetActive(true);
        shrinkMuzzleFlash = true;

        yield return new WaitForSeconds(0.15f);

        muzzleFlash.gameObject.SetActive(false);
        shrinkMuzzleFlash = false;
        muzzleFlash.localScale = Vector3.one;
    }

    private void OnEnable()
    {
        fireAction = playerInput.Player.Fire;
        fireAction.Enable();
        fireAction.performed += Fire;
    }

    private void OnDisable()
    {
        fireAction.Disable();
    }
}
