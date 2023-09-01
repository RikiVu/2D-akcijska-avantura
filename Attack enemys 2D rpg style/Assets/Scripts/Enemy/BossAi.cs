using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq.Expressions;
using System;
using UnityEditor;

public enum Stages
{
    first,
    second,
    third
}

public class BossAi : Enemy
{
    public Stages currentStage;
    private int stageNumber =1;
    // public float speed = 4;
    public float nextWaypointDistance = 3f;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    // ostalo 
    public float chaseRadius;
    public float attackRadius;
    //public Transform[] PathLocations;
    public int currentPoint;
    public float roundingDistance;
    public Transform centerWaypoint;
    Vector3 temp;
    Vector3 tempVector;

    //shooting
    public GameObject projectile;
    public float FireDelay;
    private float fireDelaySeconds;
    private bool canFire = true;
    public int counterOfProjectiles = 30;


    public int numProjectiles = 100;
    private float currentTime = 0f;
    private int projectilesShot = 0;

    private bool isSecondStage = false;
    private float startTime;

    public float noiseMagnitude = 1f;
    public float rotationSpeed = 2;
    public GameObject[] minions;



    public BoxCollider2D myCollider;
    public BoxCollider2D myCollider2;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        currentStage = Stages.first;
        InvokeRepeating("UpdatePath", 0f, .5f);
        anim.SetFloat("MoveX", 0);
        anim.SetFloat("MoveY", -1);
      
    }
    void UpdatePath()
    {
        if (Vector3.Distance(target.position,transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            seeker.StartPath(myRigidbody.position, target.position, OnPathComplete);
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
        
        if (Health > 30)
            currentStage = Stages.first;
        
        else if(Health > 20 &&  Health <= 30)
        {
            if (currentStage== Stages.first)
            {
                Instantiate(itemInside, SpawnPosition.position + new Vector3(UnityEngine.Random.Range(-1, 1), 0, UnityEngine.Random.Range(-1, 1)), Quaternion.identity);
                counterOfProjectiles = 30;
            }
                
          
               
            currentStage = Stages.second;
            anim.SetBool("StartWalking", false);
            
        }
            
        else if(Health > 10 && Health <= 20)
        {
            if (currentStage == Stages.second)
                Instantiate(itemInside, SpawnPosition.position + new Vector3(UnityEngine.Random.Range(-1, 1), 0, UnityEngine.Random.Range(-1, 1)), Quaternion.identity);
           
            currentStage = Stages.first;
        }
            
      
       
            
        else if (Health > 1 && Health <= 10)
        {
            if (currentStage == Stages.first)
            {
                foreach (var minion in minions)
                    minion.SetActive(true);
            }
            currentStage = Stages.second;
        }

      
        
            



        switch (currentStage)
        {
            case Stages.first:
                Moving();
                break;
            case Stages.second:

                //stays in center and shoots 
                if (transform.position!= centerWaypoint.position)
                {
                    temp = Vector3.MoveTowards(transform.position, centerWaypoint.position, 20 * Time.deltaTime);
                    myRigidbody.MovePosition(temp);
                    startTime = Time.time;
                    myCollider.enabled = false;
                    myCollider2.enabled = false;
                }
                else //spawn minions
                {
                    myCollider.enabled = true;
                    myCollider2.enabled = true;
                    ChangeState(EnemyState.idle); //Promjeni animaciju

                    //timer za pucanje
                    fireDelaySeconds -= Time.deltaTime;
                    if (fireDelaySeconds <= 0)
                    {
                        canFire = true;
                        anim.SetFloat("MoveX", (target.position.x - transform.position.x));
                        anim.SetFloat("MoveY", (target.position.y - transform.position.y));
                        fireDelaySeconds = FireDelay;
                        counterOfProjectiles--;
                    }
                    else
                    {
                        canFire = false;
                    }

                    if (canFire && counterOfProjectiles>0)
                    {
                        tempVector = target.transform.position - transform.position;
                        GameObject current = Instantiate(projectile, transform.position, Quaternion.identity);
                        current.GetComponent<Projectile>().Launch(tempVector);
                    }
                    else if(canFire && counterOfProjectiles <= 0 && counterOfProjectiles > -30)
                    {
                        FireDelay = 0.2f;
                        //shoot all around her 
                        ShootAllAround();
                    }        
                    else if(canFire && counterOfProjectiles <= 0 && counterOfProjectiles < -30)
                    {
                        if (shootingCount < 5)
                        {
                            if (Time.time - lastShootingTime >= shootingInterval)
                            {
                                lastShootingTime = Time.time;
                                shootingCount++;

                                ShootAllDirections();
                            }
                        }
                        else
                        {
                            counterOfProjectiles = 20;
                        }
                    }
                }
                break;
                    
        }


    }

    private void ShootAllAround()
    {
        float angle = Time.time * rotationSpeed;

        Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);

        GameObject current = Instantiate(projectile, transform.position, Quaternion.identity);
        current.GetComponent<Projectile>().Launch(direction);
    }
    private void ShootAllDirections()
    {
    
        // Adjust this value to control the amount of noise

        for (int i = 0; i < numProjectiles; i++)
        {
            float angle = i * (360f / numProjectiles);

            // Add random noise to the angle
            angle += UnityEngine.Random.Range(-noiseMagnitude, noiseMagnitude);

            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);

            GameObject current = Instantiate(projectile, transform.position, Quaternion.identity);
            current.GetComponent<Projectile>().Launch(direction);
        }
    }
    private int shootingCount = 0;
    private float shootingInterval = 3f;
    private float lastShootingTime = 0f;


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


        
        if (Vector3.Distance(target.position,transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            anim.SetBool("StartWalking", true);
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                anim.SetFloat("MoveX", (target.position.x - transform.position.x));
                anim.SetFloat("MoveY", (target.position.y - transform.position.y));
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - myRigidbody.position).normalized;
                Vector2 force = direction * moveSpeed * Time.deltaTime;
                myRigidbody.AddForce(force);
                ChangeState(EnemyState.walk); //Promjeni animaciju
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

  
        
}

 
           
