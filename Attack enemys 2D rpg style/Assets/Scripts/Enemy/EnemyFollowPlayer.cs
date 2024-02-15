using Pathfinding;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyFollowPlayer : EnemyR
{
    [Header("AI behaviour")]

    [SerializeField] private float speed = 500f;
    private bool hasLineOfSight = false;
    [SerializeField] private float sightRange = 20;
    int layerMask = 1 << 8;
    private Transform target;

    // Seeker
    public float nextWaypointDistance = 3f;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    float distance;
    private bool isUpdatingPath = false;

    //temp vars
    Vector2 direction;
    Vector2 force;


    //test vars
    private bool shown = false;

    void Start()
    {
        //parent override
        myRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();

        //this scr
        layerMask = ~layerMask;
        target = player.transform;

        //seeker
        seeker = GetComponent<Seeker>();
        //InvokeRepeating("UpdatePath", 0f, .5f);
    }
    void UpdatePath()
    {
        //Ako distance ne zadovoljava ne update-aj A*
        if (Vector2.Distance(target.position,
           transform.position) <= chaseRadius && Vector2.Distance(target.position, transform.position) > attackRadius)
        {
            //Ako vec ne racuna A*
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
            //debug collider
            /*string objectName = ray.collider.gameObject.name;
            Debug.Log("Object name: " + objectName);*/
            hasLineOfSight = ray.collider.CompareTag("Player");
            if (hasLineOfSight)
             {
                EmoteShow();
                Debug.DrawRay(transform.position, target.position - transform.position, Color.green);
                if (!isUpdatingPath)
                {
                 // If line of sight is established and not already updating path, start updating path
                 isUpdatingPath = true;
                 InvokeRepeating("UpdatePath", 0f, .5f);
                }
             }

            else
            {
                Debug.DrawRay(transform.position, target.position - transform.position, Color.red);
                // If no line of sight, cancel updating path
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

    void Moving()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            anim.SetBool("StartWalking", false);
            if(!hasLineOfSight)
                EmoteShow2();
            return;
        }
        else
            reachedEndOfPath = false;

        if ( Vector2.Distance(target.position, transform.position) > attackRadius)
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
            //SpawnEnemiesArea.currentMinionCount--;
            //Destroy(this.gameObject);
            // this.gameObject.SetActive(false);
            Debug.Log(">chaseRadius");
            anim.SetBool("StartWalking", false);
            return;
        }
        else if (Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            //all attack my warriors
            if (currentState != EnemyStateR.attack)
            {
                Debug.Log("Attack");
                anim.SetFloat("MoveX", (target.position.x - transform.position.x));
                anim.SetFloat("MoveY", (target.position.y - transform.position.y));
                //myRigidbody.velocity = Vector2.zero;
                //  StartCoroutine(AttackCo());
            }
        }
    }

    void ChangeState(EnemyStateR newState)
    {
        if (currentState != newState)
            currentState = newState;
    }
}
