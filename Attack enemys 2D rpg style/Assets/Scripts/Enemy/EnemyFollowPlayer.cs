using Pathfinding;
using System.Collections;
using UnityEngine;

public enum EnemyStateMachine
{
    Patrol,
    Chase
}

public class EnemyFollowPlayer : EnemyR
{
    [Header("AI Behaviour")]
    public EnemyStateMachine currentStateAi;
    [SerializeField] private float speed = 500f;
    [SerializeField] private float sightRange = 25;
    private bool hasLineOfSight = false;
    private Transform target;
    private int layerMask = ~(1 << 8 | 1 << 6);

    // Seeker
    public float nextWaypointDistance = 3f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private float distance;
    private bool isUpdatingPath = false;

    // Temporary variables
    private Vector2 direction;
    private Vector2 force;

    // Test variables
    private bool shown = false;

    [Header("Wandering Settings")]
    public float wanderingRadius = 15f;
    public float minWanderInterval = 2.0f;
    public float maxWanderInterval = 4.0f;
    private Vector2 initialPosition;
    private Vector2 wanderDestination;
    private float nextWanderTime;

    void Start()
    {
        InitializeVariables();
        currentStateAi = EnemyStateMachine.Patrol;
        anim.SetBool("StartWalking", true);
        initialPosition = transform.position;
        nextWanderTime = Time.time + Random.Range(minWanderInterval, maxWanderInterval);
        RandomLocationWander();
    }

    private void InitializeVariables()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        target = player.transform;
        seeker = GetComponent<Seeker>();
    }

    private void Update()
    {
        if (currentStateAi == EnemyStateMachine.Patrol)
        {
            RandomLocationWander();
        }
    }

    private void RandomLocationWander()
    {
        if (Vector2.Distance(transform.position, initialPosition) > wanderingRadius)
        {
            MoveTowards(initialPosition, 7);
            return;
        }

        if (Time.time >= nextWanderTime)
        {
            wanderDestination = GetRandomPointInRadius();
            nextWanderTime = Time.time + Random.Range(minWanderInterval, maxWanderInterval);
        }

        MoveTowards(wanderDestination, 5);
    }

    private Vector2 GetRandomPointInRadius()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized * wanderingRadius;
        return initialPosition + randomDirection;
    }

    private void MoveTowards(Vector2 destination, float movementSpeed)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
    }

    /*
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(initialPosition, wanderingRadius);
    }
    */

    void UpdatePath()
    {
        if (Vector2.Distance(target.position, transform.position) <= chaseRadius &&
            Vector2.Distance(target.position, transform.position) > attackRadius)
        {
            if (seeker.IsDone())
                seeker.StartPath(myRigidbody.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        RayCastShot();
        Moving();
    }

    void RayCastShot()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, target.position - transform.position, sightRange, layerMask);
        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player");
            if (hasLineOfSight)
            {
                currentStateAi = EnemyStateMachine.Chase;
                EmoteShow();
                Debug.DrawRay(transform.position, target.position - transform.position, Color.green);
                if (!isUpdatingPath)
                {
                    isUpdatingPath = true;
                    InvokeRepeating("UpdatePath", 0f, .5f);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, target.position - transform.position, Color.red);
                if (isUpdatingPath)
                {
                    CancelInvoke("UpdatePath");
                    isUpdatingPath = false;
                }
            }
        }
    }

    void EmoteShow()
    {
        if (!shown)
        {
            emoteAnimator.SetTrigger("noticed");
            shown = true;
        }
    }

    void EmoteShow2()
    {
        if (shown)
        {
            emoteAnimator.SetTrigger("lostTarget");
            shown = false;
        }
    }

    private IEnumerator LostCoroutine()
    {
        anim.SetBool("StartWalking", false);
        EmoteShow2();
        yield return new WaitForSeconds(3f);
        if (!hasLineOfSight || Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            currentStateAi = EnemyStateMachine.Patrol;
            anim.SetBool("StartWalking", true);
        }
    }

    void Moving()
    {
        if (path == null || currentStateAi != EnemyStateMachine.Chase)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            StartCoroutine(LostCoroutine());
            return;
        }
        else
            reachedEndOfPath = false;

        if (Vector2.Distance(target.position, transform.position) > attackRadius)
        {
            anim.SetBool("StartWalking", true);

            if (currentState == EnemyStateR.idle || currentState == EnemyStateR.walk && currentState != EnemyStateR.stagger)
            {
                direction = ((Vector2)path.vectorPath[currentWaypoint] - myRigidbody.position).normalized;
                anim.SetFloat("MoveX", (direction.x));
                anim.SetFloat("MoveY", (direction.y));
                force = direction * speed * Time.deltaTime;
                myRigidbody.AddForce(force);
                distance = Vector2.Distance(myRigidbody.position, path.vectorPath[currentWaypoint]);
                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
                ChangeState(EnemyStateR.walk);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            Debug.Log(">chaseRadius");
            anim.SetBool("StartWalking", false);
            return;
        }
        else if (Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            Debug.Log("Attack");
            anim.SetFloat("MoveX", (target.position.x - transform.position.x));
            anim.SetFloat("MoveY", (target.position.y - transform.position.y));
        }
    }

    void ChangeState(EnemyStateR newState)
    {
        if (currentState != newState)
            currentState = newState;
    }
}

