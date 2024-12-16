using UnityEngine;
using UnityEngine.AI;

public class ZonerAI : MonoBehaviour
{
    public AIState state { get; private set; } = AIState.Pathing;

    private bool initiated = false;
    private bool charging = false;

    [SerializeField]
    private NavMeshAgent agent;
    private float pathFindingInterval = 0.2f;
    private float timeWhenLastPathFinded = 0;

    private float preferredDistanceToPlayer = 10.0f;

    private Transform playerTransform;

    private DimensionBound dimBound;

    private LayerMask layerMaskBlue;
    private LayerMask layerMaskRed;

    [SerializeField]
    private Transform zonePrefab;
    private float zoneDamage = 35f;
    private float zoneSpeed = 20f;
    private float zoneDuration = 3.5f;
    private float zoneCooldown = 5f;
    private float timeWhenLastZoned = 0f;

    [SerializeField]
    private ZonerWarning warning;
    private float chargeDuration = 1f;
    private float timeWhenChargeBegan = 0f;

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
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if(charging)
        {
            if(Time.time - timeWhenChargeBegan >= chargeDuration)
            {
                charging = false;
                warning.StopWarning();
                Attack();
            }
            return;
        }

        if (InRangeAndVisionOfPlayer())
        {
            if (state != AIState.Attacking)
            {
                state = AIState.Attacking;
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
        if (state == AIState.Attacking) BeginCharging();
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

        if (Vector3.Distance(transform.position, playerCenter) > preferredDistanceToPlayer) return false;

        Vector3 vecToPlayer = playerCenter - transform.position;

        LayerMask layerMask = (dimBound.GetBoundDimension() == Dimension.Blue ? layerMaskBlue : layerMaskRed);

        if (Physics.Raycast(transform.position, vecToPlayer.normalized, vecToPlayer.magnitude, layerMask)) return false;

        return true;
    }

    private void BeginCharging()
    {
        if (Time.time - timeWhenLastZoned < zoneCooldown) return;

        audioSource.PlaySound("charging", 0.2f);
        charging = true;
        timeWhenChargeBegan = Time.time;
        warning.StartWarning();
    }

    private void Attack()
    {
        timeWhenLastZoned = Time.time;

        audioSource.PlaySound("laserwave", 0.2f);
        Zone zone = Instantiate(zonePrefab, transform.position + Vector3.up * 2.64f, transform.rotation).GetComponent<Zone>();

        Dimension dim = dimBound.GetBoundDimension();
        zone.Init(dim, zoneDamage, zoneSpeed, zoneDuration);
    }
}
