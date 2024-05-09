using Pathfinding;
using System.Collections;
using UnityEditor.Rendering;
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
    //[SerializeField] private float speed = 500f;
    [SerializeField] private float sightRange = 25;
    private bool hasLineOfSight = false;
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
    private Vector3 tempPosition;
    private int stuckCounter = 0;

    private bool coroutineStarted = false;
    private bool coroutineStarted2 = false;

    [SerializeField] private bool canMove= false;
    [SerializeField] private bool attacking = false;

    private bool hasTarget = false;

    void Start()
    {
        InitializeVariables();
        currentStateAi = EnemyStateMachine.Patrol;
        anim.SetBool("StartWalking", true);
        initialPosition = transform.position;
        wanderDestination = GetRandomPointInRadius();
        UpdatePathWanderPos(wanderDestination);
        reachedEndOfPath = false;
        canMove = true;
        tempPosition = transform.position;
    }
    private void InitializeVariables()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        enemySprite = this.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        target = player.transform;
        seeker = GetComponent<Seeker>();
        Health = enemyScribtableObject.MaxHealth;
    }

    private Vector2 GetRandomPointInRadius()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized * wanderingRadius;
        return initialPosition + randomDirection;
    }

   
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(initialPosition, wanderingRadius);
    }

    void UpdatePathPlayer()
    {
        if (Vector2.Distance(target.position, transform.position) <= enemyScribtableObject.chaseRadius &&
            Vector2.Distance(target.position, transform.position) > enemyScribtableObject.attackRadius)
        {
            if (seeker.IsDone())
                seeker.StartPath(myRigidbody.position, target.position, OnPathComplete);
        }
    }
    void UpdatePathWanderPos(Vector2 pos)
    {
        if (seeker.IsDone())
            seeker.StartPath(myRigidbody.position, pos, OnPathComplete);
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
           // Debug.Log(ray.collider.gameObject.name);
            hasLineOfSight = ray.collider.CompareTag("Player");
            if (hasLineOfSight)
            {
                Debug.DrawRay(transform.position, target.position - transform.position, Color.green);
                if (!isUpdatingPath)
                {
                    canMove = true;
                    anim.SetBool("StartWalking", true);
                    StopCoroutine(WanderCooldown());
                    currentStateAi = EnemyStateMachine.Chase;
                    isUpdatingPath = true;
                    if (!hasTarget)
                        EmoteShow();
                  
                     
                    hasTarget = true;
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
        if(Vector3.Distance(target.position, transform.position) > enemyScribtableObject.attackRadius)
        {
            emoteAnimator.SetTrigger("noticed");
        }
           
    }

    void EmoteShow2()
    {
            emoteAnimator.SetTrigger("lostTarget");
    }
 
    private IEnumerator WanderCooldown()
    {
        if(hasTarget)
        {
            Debug.Log("NE NEG ovaj je kriv ");
            EmoteShow2();
        }
        coroutineStarted = true;
        canMove = false;
        currentStateAi = EnemyStateMachine.Patrol;
        hasTarget = false;
        myRigidbody.velocity = Vector2.zero;

        wanderDestination = GetRandomPointInRadius();
        UpdatePathWanderPos(wanderDestination);
        anim.SetBool("StartWalking", false);
        yield return new WaitForSeconds(3.0f);
        canMove = true;
        anim.SetBool("StartWalking", true);
        //Debug.Log("waiting done");
        coroutineStarted = false;
        anim.SetFloat("MoveX", (wanderDestination.x - transform.position.x));
        anim.SetFloat("MoveY", (wanderDestination.y - transform.position.y));
        
    }
    private IEnumerator WanderCooldown2()
    {
        coroutineStarted = true;
        yield return new WaitForSeconds(0.5f);
        coroutineStarted = false;
        if (!hasLineOfSight)
        {
            StartCoroutine(WanderCooldown());
        }
    }

    public float leapForce;

    private IEnumerator AttackCo()
    {

        coroutineStarted2 = true;
        canMove = false;
        yield return new WaitForSeconds(enemyScribtableObject.prepareToAttack);
        Vector2 directionToPlayer = (target.position - transform.position).normalized;
        // Apply leap force towards the player
        myRigidbody.velocity = directionToPlayer * leapForce;
        // Trigger attack animation
        anim.SetFloat("MoveX", directionToPlayer.x);
        anim.SetFloat("MoveY", directionToPlayer.y);
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(enemyScribtableObject.attackCooldown);
        canMove = true;
        coroutineStarted2 = false;
        Vector2 directionToWander = (wanderDestination - (Vector2)transform.position).normalized;
        anim.SetFloat("MoveX", directionToWander.x);
        anim.SetFloat("MoveY", directionToWander.y);
    }

    Vector2 noise;
    void Moving()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (currentStateAi == EnemyStateMachine.Patrol)
            {
                if (!coroutineStarted)
                    StartCoroutine(WanderCooldown());
                return;
            }
            else if(currentStateAi == EnemyStateMachine.Chase &&  !hasLineOfSight)
            {
                if (!coroutineStarted)
                    StartCoroutine(WanderCooldown2());
                return;
            }
            else if(hasLineOfSight && Vector3.Distance(target.position, transform.position) <= enemyScribtableObject.attackRadius) {
             
                if (!coroutineStarted2)
                    StartCoroutine(AttackCo());
                return;
            }
            else
            {
                if(Vector3.Distance(target.position, transform.position) > enemyScribtableObject.chaseRadius)
                {
                    StartCoroutine(WanderCooldown());
                }
                else
                {
                    Debug.Log("5");
                }
                return;
            }
        }

        if (currentStateAi == EnemyStateMachine.Patrol && canMove)
        {
            // Debug.Log("patrol logic");
           
            if (Vector2.Distance(transform.position, initialPosition) > wanderingRadius)
            {
                direction = ((Vector2)path.vectorPath[currentWaypoint] - myRigidbody.position).normalized;
                force = direction * enemyScribtableObject.speed * Time.deltaTime;
                myRigidbody.AddForce(force);
                distance = Vector2.Distance(myRigidbody.position, path.vectorPath[currentWaypoint]);
                if (distance < nextWaypointDistance)
                    currentWaypoint++;
                return;
            }


            if (!coroutineStarted)
            {
                direction = ((Vector2)path.vectorPath[currentWaypoint] - myRigidbody.position).normalized;
                force = direction * enemyScribtableObject.speed * Time.deltaTime;
                myRigidbody.AddForce(force);
                distance = Vector2.Distance(myRigidbody.position, path.vectorPath[currentWaypoint]);
                if (distance < nextWaypointDistance)
                    currentWaypoint++;
            }
            if (tempPosition == transform.position)
            {
                stuckCounter++;
                if (stuckCounter > 5)
                {
                    StopCoroutine(WanderCooldown());
                    StartCoroutine(WanderCooldown());
                    stuckCounter = 0;
                }
            }
            else
            {
                tempPosition = transform.position;
            }
           
        }
        else if (currentStateAi == EnemyStateMachine.Chase && canMove)
        {
           // Debug.Log("chase logic");
            if (Vector2.Distance(target.position, transform.position) > enemyScribtableObject.attackRadius)
            {
             
                anim.SetBool("StartWalking", true);
                direction = ((Vector2)path.vectorPath[currentWaypoint] - myRigidbody.position).normalized;
                if (hasLineOfSight)
                {
                    anim.SetFloat("MoveX", (target.position.x - transform.position.x));
                    anim.SetFloat("MoveY", (target.position.y - transform.position.y));
                }
                else
                {
                    anim.SetFloat("MoveX",  path.vectorPath[currentWaypoint].x - myRigidbody.position.x);
                    anim.SetFloat("MoveY", path.vectorPath[currentWaypoint].y - myRigidbody.position.y);
                }

                force = direction * enemyScribtableObject.speed * Time.deltaTime;
                myRigidbody.AddForce(force);
                distance = Vector2.Distance(myRigidbody.position, path.vectorPath[currentWaypoint]);
                if (distance < nextWaypointDistance)
                    currentWaypoint++;
               
            }
            else if (Vector3.Distance(target.position, transform.position) > enemyScribtableObject.chaseRadius)
            {
                Debug.Log(">chaseRadius");
               // anim.SetBool("StartWalking", false);
                return;
            }
            else if (hasLineOfSight && Vector3.Distance(target.position, transform.position) <= enemyScribtableObject.attackRadius)
            {
                if (!coroutineStarted2)
                    StartCoroutine(AttackCo());
            }

         

        }
    }


    void ChangeState(EnemyStateR newState)
    {
        if (currentState != newState)
            currentState = newState;
    }
}

