using UnityEngine;
using Pathfinding;
using Random = UnityEngine.Random;
public enum Stages
{
    first,
    second,
    third
}

public class BossAi : Enemy
{
    [SerializeField]
    private Stages currentStage;
    private int stageNumber = 1;

    [SerializeField]
    private float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;

    // ostalo
    
    public static float chaseRadius;

    [SerializeField]
    private float attackRadius;

    [SerializeField]
    private Transform centerWaypoint;

    [SerializeField]
    private Vector3 temp;

    [SerializeField]
    private Vector3 tempVector;

    // shooting
    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private float FireDelay;

    [SerializeField]
    private float fireDelaySeconds;

    [SerializeField]
    private bool canFire = true;

    [SerializeField]
    private int counterOfProjectiles = 30;

    [SerializeField]
    private int numProjectiles = 100;

    [SerializeField]
    private float currentTime = 0f;

    [SerializeField]
    private int projectilesShot = 0;

    [SerializeField]
    private bool isSecondStage = false;

    [SerializeField]
    private float startTime;

    [SerializeField]
    private float noiseMagnitude = 1f;

    [SerializeField]
    private float rotationSpeed = 2;

    [SerializeField]
    private GameObject[] minions;

    [SerializeField]
    private GameObject transferCollider;

    [SerializeField]
    private GameObject WallBoss;

    [SerializeField]
    private BoxCollider2D myCollider;

    [SerializeField]
    private BoxCollider2D myCollider2;

    void Start()
    {
        anim = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        currentStage = Stages.first;
        InvokeRepeating("UpdatePath", 0f, .5f);
        anim.SetFloat("MoveX", 0);
        anim.SetFloat("MoveY", -1);
        chaseRadius = 35;
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
            HealthTip_Go.SetActive(true);
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

    public override void Death()
    {
        DeSelect();
        HealthTip_Go.SetActive(false);
        Enemy1 = false;
        FindObjectOfType<AudioManager>().Play("DieLog");
        this.gameObject.SetActive(false);

        InitHearts2();
        Redirect.Killed(enemyName);

        Instantiate(itemInside, SpawnPosition.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)), Quaternion.identity);
        Instantiate(soul, SpawnPosition.position + new Vector3(0, 0, 0), Quaternion.identity);

        transferCollider.SetActive(false);
        RoomMove.bossFight = false;
        WallBoss.SetActive(false);
    }



    void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

  
        
}

 
           
