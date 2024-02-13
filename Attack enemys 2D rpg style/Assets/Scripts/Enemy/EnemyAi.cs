using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq.Expressions;
using System;

public enum EnemyType
{
    spawn,
    patrol
}

public class EnemyAi : Enemy
{
    public EnemyType currentType;
    #region spawn;

    #endregion

    #region patrol;
    public float nextWaypointDistance = 3f;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    public Transform[] PathLocations;
    #endregion

    Path path;
    Seeker seeker;
    public float chaseRadius;
    public float attackRadius;
  
    public int currentPoint;
    public float roundingDistance;
    public Transform currentGoal;
    float distance;



    void Start()
    {
        anim = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, .5f);
       
        this.gameObject.SetActive(false);
    }

    void UpdatePath()
    {

        if (Vector3.Distance(target.position,
            transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (this.gameObject.activeInHierarchy != true)
            {
                this.gameObject.SetActive(true);
            }
            seeker.StartPath(myRigidbody.position, target.position, OnPathComplete);
        }
    }
    void UpdatePathPatrol()
    {
        if (Vector3.Distance(target.position,
            transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            seeker.StartPath(myRigidbody.position, target.position, OnPathComplete);
        }

        if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            try
            {
                if (seeker.IsDone())
                {
                    seeker.StartPath(myRigidbody.position, PathLocations[currentPoint].position, OnPathComplete);
                }
            }
            catch
            {
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

    void FixedUpdate()
    {
        Moving();
    }

    void Moving()
    {
        if (path == null && Health>0)
            return;

        if (Vector3.Distance(target.position,transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            anim.SetBool("StartWalking", true);
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                
                anim.SetFloat("MoveX", (target.position.x - transform.position.x));
                anim.SetFloat("MoveY", (target.position.y - transform.position.y));
                if (path.vectorPath.Count > currentWaypoint)
                {
                    Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - myRigidbody.position).normalized;
                    Vector2 force = direction * moveSpeed * Time.deltaTime;
                    myRigidbody.AddForce(force);
                }
                ChangeState(EnemyState.walk);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            SpawnEnemiesArea.currentMinionCount--;
            Destroy(this.gameObject);
            // this.gameObject.SetActive(false);
            return;
        }
        else if(Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            //all attack my warriors
            if(currentState != EnemyState.attack)
            {
                Debug.Log("Attack");
                anim.SetFloat("MoveX", (target.position.x - transform.position.x));
                anim.SetFloat("MoveY", (target.position.y - transform.position.y));
                //myRigidbody.velocity = Vector2.zero;
                StartCoroutine(AttackCo());
            }
        }

        if (path.vectorPath.Count > currentWaypoint)
        {
            distance = Vector2.Distance(myRigidbody.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                    currentWaypoint++;
            }
        }

    }

    private IEnumerator AttackCo()
    {
        currentState = EnemyState.attack;
        
        yield return new WaitForSeconds(0.4f);
        myRigidbody.velocity = Vector2.zero;
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(0.2f);
        yield return null;
        currentState =  EnemyState.idle;
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Attack", false);
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


    void MovingTemp()
    {
        if (path == null && Health > 0)
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
            this.gameObject.SetActive(false);
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


}





