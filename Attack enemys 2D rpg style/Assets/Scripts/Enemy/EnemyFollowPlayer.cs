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
    //private float nextWanderTime;
    private bool coroutineStarted = false;

    [SerializeField] private float noiseMagnitude = 0.5f;
    [SerializeField] private float noiseStrength = 0.1f;

    void Start()
    {
        InitializeVariables();
        currentStateAi = EnemyStateMachine.Patrol;
        anim.SetBool("StartWalking", true);
        initialPosition = transform.position;
        wanderDestination = initialPosition;
        //nextWanderTime = Time.time + Random.Range(minWanderInterval, maxWanderInterval);
        //RandomLocationWander();
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
            MoveTowards(initialPosition, 5);
            return;
        }
        if (Vector2.Distance(transform.position, wanderDestination) < 0.2)
        {
            if(!coroutineStarted)
                StartCoroutine(WanderCooldown(3.0f));
            

            return;
        }

        if(!coroutineStarted)
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

    
    private void OnDrawGizmosSelected()
    {
        /*Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);*/
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
  

    void UpdatePathPlayer()
    {
        if (Vector2.Distance(target.position, transform.position) <= chaseRadius &&
            Vector2.Distance(target.position, transform.position) > attackRadius)
        {
            if (seeker.IsDone())
                seeker.StartPath(myRigidbody.position, target.position, OnPathComplete);
        }
    }
    void UpdatePathInitialPos()
    {
            if (seeker.IsDone())
                seeker.StartPath(myRigidbody.position, initialPosition, OnPathComplete);
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

    RaycastHit2D ray;
    void RayCastShot()
    {
        ray = Physics2D.Raycast(transform.position, target.position - transform.position, sightRange, layerMask);
        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player");
            if (hasLineOfSight)
            {
                currentStateAi = EnemyStateMachine.Chase;
                Debug.DrawRay(transform.position, target.position - transform.position, Color.green);
                if (!isUpdatingPath)
                {
                    isUpdatingPath = true;
                    EmoteShow();
                    InvokeRepeating("UpdatePathPlayer", 0f, .5f);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, target.position - transform.position, Color.red);
                if (isUpdatingPath)
                {
                    CancelInvoke("UpdatePathPlayer");
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
    private IEnumerator WanderCooldown(float waitTime)
    {
        coroutineStarted = true;
        Debug.Log("change location");
        wanderDestination = GetRandomPointInRadius();
        Debug.Log(wanderDestination);
        anim.SetBool("StartWalking", false);
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("StartWalking", true);
        Debug.Log("waiting done");
        coroutineStarted = false;
        //UpdatePathInitialPos();
        anim.SetFloat("MoveX", (wanderDestination.x - transform.position.x));
        anim.SetFloat("MoveY", (wanderDestination.y - transform.position.y));

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

                // Add noise to the direction vector
                Vector2 noise = Random.insideUnitCircle * noiseMagnitude;
                direction += noise.normalized * noiseStrength;
                if (hasLineOfSight)
                {
                    anim.SetFloat("MoveX", (target.position.x - transform.position.x));
                    anim.SetFloat("MoveY", (target.position.y - transform.position.y));
                }
                else
                {
                    anim.SetFloat("MoveX", (path.vectorPath[currentWaypoint].x));
                    anim.SetFloat("MoveY", (path.vectorPath[currentWaypoint].y));
                }
               
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

