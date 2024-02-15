using Pathfinding;
using UnityEngine;

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

    //temp vars
    Vector2 direction;
    Vector2 force;
    void Start()
    {
        target = player.transform;
        layerMask = ~layerMask;

        //seeker
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, .5f);
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
        if (hasLineOfSight)
        {
            Moving();
            //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {

        }
    }

    void RayCastShot()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position, sightRange, layerMask);
        if (ray.collider != null)
        {
            //debug collider
            /*string objectName = ray.collider.gameObject.name;
            Debug.Log("Object name: " + objectName);*/
            hasLineOfSight = ray.collider.CompareTag("Player");
            if (hasLineOfSight)
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
           
            else
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
         
        }
    }

    void Moving()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
            reachedEndOfPath = false;
        
        direction = ((Vector2)path.vectorPath[currentWaypoint]-myRigidbody.position).normalized;
        force = direction * speed * Time.deltaTime; 
        myRigidbody.AddForce(force);
        distance = Vector2.Distance(myRigidbody.position, path.vectorPath[currentWaypoint]);
        if(distance<nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
