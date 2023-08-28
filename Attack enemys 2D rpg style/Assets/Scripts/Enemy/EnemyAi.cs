using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq.Expressions;
using System;

public class EnemyAi : Enemy
{
   // public float speed = 4;
    public float nextWaypointDistance = 3f;

    

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    


    // ostalo 
  
    public float chaseRadius;
    public float attackRadius;
    public Transform[] PathLocations;
    public int currentPoint;
    public float roundingDistance;
    public Transform currentGoal;

    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();

        seeker = GetComponent<Seeker>();
     

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

  

    void UpdatePath()
    {

        if (Vector3.Distance(target.position,
            transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            seeker.StartPath(myRigidbody.position, target.position, OnPathComplete);

        }


        if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            try{
                if (seeker.IsDone())
                {
                    seeker.StartPath(myRigidbody.position, PathLocations[currentPoint].position, OnPathComplete);
                }
            }
            catch {
                Debug.Log("Nema lokaciju");

            }
            
            
        }

    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Moving();
    }

    void Moving()
    {
        if (path == null && Health>0)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }



        if (Vector3.Distance(target.position,
            transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            anim.SetBool("StartWalking", true);
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                anim.SetFloat("MoveX", (target.position.x - transform.position.x));
                anim.SetFloat("MoveY", (target.position.y - transform.position.y));

                // Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                //rb.MovePosition(temp);

                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - myRigidbody.position).normalized;
                Vector2 force = direction * moveSpeed * Time.deltaTime;


                myRigidbody.AddForce(force);
                ChangeState(EnemyState.walk);

            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            anim.SetBool("StartWalking", true);
            if (Vector3.Distance(transform.position, PathLocations[currentPoint].position) > roundingDistance)
            {

                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - myRigidbody.position).normalized;

                Vector2 force = direction * moveSpeed * Time.deltaTime;
                myRigidbody.AddForce(force);


                anim.SetFloat("MoveX", (PathLocations[currentPoint].position.x - transform.position.x));
                anim.SetFloat("MoveY", (PathLocations[currentPoint].position.y - transform.position.y));
                ChangeState(EnemyState.walk);
            }
            else
            {
                    ChangeGoal();
            }

        }




                float distance = Vector2.Distance(myRigidbody.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

     void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
     void ChangeGoal()
    {
        if (currentPoint == PathLocations.Length - 1)
        {
            currentPoint = 0;
            currentGoal = PathLocations[0];
        }
        else
        {
            currentPoint++;
            currentGoal = PathLocations[currentPoint];


        }
    }
}

 
           
