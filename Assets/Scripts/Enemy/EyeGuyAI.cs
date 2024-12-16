using UnityEngine;
using UnityEngine.AI;

public class EyeGuyAI : MonoBehaviour
{
    public AIState state {  get; private set; } = AIState.Pathing;

    private bool initiated = false;

    [SerializeField]
    private NavMeshAgent agent;
    private float pathFindingInterval = 0.2f;
    private float timeWhenLastPathFinded = 0;

    private float preferredDistanceToPlayer = 20.0f;

    private Transform playerTransform;

    private DimensionBound dimBound;

    private LayerMask layerMaskBlue;
    private LayerMask layerMaskRed;

    [SerializeField]
    private Transform bulletPrefab;
    private float bulletDamage = 10.0f;
    private float bulletSpeed = 25.0f;
    private float bulletScale = 2.5f;
    private float timeBetweenShots = 0.3f;
    private float timeWhenLastShot = 0;
    private float timeBetweenBursts = 2.5f;
    private float timeWhenLastBurst = 0;
    private int bulletsPerBurst = 3;
    private int bulletsThisBurst;

    [SerializeField]
    private AudioSource audioSource;

    void Start()
    {
        playerTransform = PlayerMovement.Instance.transform;
        dimBound = GetComponent<DimensionBound>();

        layerMaskBlue = LayerMask.GetMask("GroundBlue", "WallBlue", "UniversalGround");
        layerMaskRed = LayerMask.GetMask("GroundRed", "WallRed", "UniversalGround");

        Invoke(nameof(Init), Random.Range(0f, 1f));
    }

    void Update()
    {
        if (!initiated) return;

        transform.LookAt(playerTransform);

        if (InRangeAndVisionOfPlayer())
        {
            if (state != AIState.Attacking)
            {
                state = AIState.Attacking;
                bulletsThisBurst = bulletsPerBurst;
                timeWhenLastBurst = Time.time;
                agent.isStopped = true;
            }
        }
        else
        {
            if (state != AIState.Pathing)
            {
                state = AIState.Pathing;
                agent.isStopped = false;
            }
        }

        if (state == AIState.Pathing && Time.time - timeWhenLastPathFinded > pathFindingInterval) FindPathToPlayer();
        if (state == AIState.Attacking) Attack();
    }

    private void Init()
    {
        FindPathToPlayer();
        initiated = true;
    }

    private void FindPathToPlayer()
    {
        agent.SetDestination(playerTransform.position);
        timeWhenLastPathFinded = Time.time;
    }

    private bool InRangeAndVisionOfPlayer()
    {
        Vector3 playerCenter = playerTransform.position + Vector3.up * 1.6f;

        if(Vector3.Distance(transform.position, playerCenter) > preferredDistanceToPlayer) return false;

        Vector3 vecToPlayer = playerCenter - transform.position;

        LayerMask layerMask = (dimBound.GetBoundDimension() == Dimension.Blue ? layerMaskBlue : layerMaskRed);

        if (Physics.Raycast(transform.position, vecToPlayer.normalized, vecToPlayer.magnitude, layerMask)) return false;

        return true;
    }

    private void Attack()
    {
        if (Time.time - timeWhenLastBurst < timeBetweenBursts) return;
        if (Time.time - timeWhenLastShot < timeBetweenShots) return;

        bulletsThisBurst--;
        timeWhenLastShot = Time.time;
        if (bulletsThisBurst == 0)
        {
            bulletsThisBurst = bulletsPerBurst;
            timeWhenLastBurst = Time.time;
        }

        audioSource.PlaySound("lasershot", 0.05f);
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>();

        Dimension dim = dimBound.GetBoundDimension();
        bullet.Init(dim, LayerMask.GetMask("Player" + dim.ToString()), bulletDamage, bulletSpeed, 2.5f, bulletScale);
    }
}
