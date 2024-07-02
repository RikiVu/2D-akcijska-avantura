using UnityEngine;
using Pathfinding;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Rendering;


public enum Stages
{
    first,
    second,
    third
}

public class BossAi : EnemyR
{
    // General variables
    [SerializeField] private Stages currentStage;
    [SerializeField] private GameObject WallBoss;
    private int stageNumber = 1;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;

    // AI variables
    [SerializeField] private float attackRadius;
    [SerializeField] private Transform centerWaypoint;
    [SerializeField] private float nextWaypointDistance = 3f;

    // Shooting variables
    [SerializeField] private GameObject projectile;
    [SerializeField] private float FireDelay;
    [SerializeField] private float fireDelaySeconds;
    [SerializeField] private bool canFire = true;
    [SerializeField] private int counterOfProjectiles = 30;
    [SerializeField] private int numProjectiles = 100;
    [SerializeField] private float noiseMagnitude = 1f;
    [SerializeField] private float rotationSpeed = 2;
    [SerializeField] private Transform[] minionsSpawn;
    
    [SerializeField] private BoxCollider2D myCollider;
    [SerializeField] private BoxCollider2D myCollider2;
    private float startTime;
    private float currentTime = 0f;
    private bool isSecondStage = false;
    private int shootingCount = 0;
    private float shootingInterval = 3f;
    private float lastShootingTime = 0f;

    [SerializeField]
    private Vector3 temp;
    [SerializeField]
    private Vector3 tempVector;
    [SerializeField]
    private GameObject bossHealth;
    public static float chaseRadius;

    //Enemy drop
    public GameObject itemInside2;
    private Vector3 starInitailPosition;
    //load
    public static bool bossDefeated = false;
    private Vector3 bossInitialPosition;
    [SerializeField]
    private RoomMove rmMove;
    [SerializeField]
    private GameObject EnemyToSpawn;
    private List<GameObject> minions = new List<GameObject>();
   


    void Start()
    {
        seeker = GetComponent<Seeker>();
        currentStage = Stages.first;
        InvokeRepeating("UpdatePathPlayer", 0f, .5f);
        myRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        anim.SetFloat("MoveX", 0);
        anim.SetFloat("MoveY", -1);
        enemySprite = this.GetComponent<SpriteRenderer>();
        Health = enemyScribtableObject.MaxHealth;
        target = player.transform;
        chaseRadius = enemyScribtableObject.chaseRadius;
        bossInitialPosition = this.transform.position;
        starInitailPosition = itemInside2.transform.position;
    }

    public void Load(bool bossDefeated2)
    {
        bossDefeated = bossDefeated2;
        DeSelect();
        bossHealth.SetActive(false);
        RoomMove.bossFight = false;
        itemInside2.transform.position = starInitailPosition;
        if (minions!=null)
        {
            foreach (GameObject minion in minions)
            {
                Destroy(minion);
            }
            minions.Clear();
        }
    

        if (bossDefeated)
        {
            this.gameObject.SetActive(false);
            WallBoss.SetActive(false);
            rmMove.loadPrevious2();
        }
        else
        {
            this.gameObject.SetActive(true);
            //restore health..
            Health = enemyScribtableObject.MaxHealth;
            InitHearts2();
            InitHearts();
            this.transform.position = bossInitialPosition;
            chaseRadius = 35;
            anim.SetFloat("MoveX", 0);
            anim.SetFloat("MoveY", -1);
            rmMove.loadPrevious();
            currentStage = Stages.first;
            target = player.transform;
            currentState = EnemyStateR.idle;
            isTargetable = true;
            //numProjectiles = 100;
        }
    }

    void UpdatePath()
    {
        if (Vector3.Distance(target.position,transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
            seeker.StartPath(myRigidbody.position, target.position, OnPathComplete);
    }
    void UpdatePathPlayer()
    {
        if (Vector2.Distance(target.position, transform.position) <= chaseRadius &&
            Vector2.Distance(target.position, transform.position) > enemyScribtableObject.attackRadius)
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(myRigidbody.position, target.position, OnPathComplete);
            }
         
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Health > 20 &&  Health <= 30)
        {
            if (currentStage== Stages.first)
            {
                Instantiate(itemInside, transform.position + new Vector3(UnityEngine.Random.Range(-1, 1), 0, UnityEngine.Random.Range(-1, 1)), Quaternion.identity);
                counterOfProjectiles = 30;
                shootingCount = 0;
            }
            currentStage = Stages.second;
            anim.SetBool("StartWalking", false);
        }
        else if(Health > 10 && Health <= 20)
        {
            if (currentStage == Stages.second)
                Instantiate(itemInside, transform.position + new Vector3(UnityEngine.Random.Range(-1, 1), 0, UnityEngine.Random.Range(-1, 1)), Quaternion.identity);
            currentStage = Stages.first;
        }
        else if (Health > 1 && Health <= 10)
        {
            if (currentStage == Stages.first)
            {
                counterOfProjectiles = 30;
                shootingCount = 0;
                Debug.Log("enemies lenght: " + minionsSpawn.Length);
                foreach (Transform minion in minionsSpawn)
                {
                    Debug.Log("spawining enemy");
                    minions.Add(Instantiate(EnemyToSpawn, minion.position + new Vector3(UnityEngine.Random.Range(-1, 1), 0, UnityEngine.Random.Range(-1, 1)), Quaternion.identity));
                }
                    
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
                    ChangeState(EnemyStateR.idle); //Promjeni animaciju

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
                            counterOfProjectiles = 30;
                            shootingCount = 0;
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
    public float leapForce;

    private bool coroutineStarted;
    private IEnumerator AttackCo()
    {

        coroutineStarted = true;
        yield return new WaitForSeconds(enemyScribtableObject.prepareToAttack);
        Vector2 directionToPlayer = (target.position - transform.position).normalized;
        // Apply leap force towards the player
        myRigidbody.velocity = directionToPlayer * leapForce;
        // Trigger attack animation
        anim.SetFloat("MoveX", directionToPlayer.x);
        anim.SetFloat("MoveY", directionToPlayer.y);
        //anim.SetTrigger("attack");
        yield return new WaitForSeconds(enemyScribtableObject.attackCooldown);
        coroutineStarted = false;
    }
    void Moving()
    {
        if (path == null && Health>0)
            return;
       
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            if(!coroutineStarted)
             StartCoroutine(AttackCo());
            return;
        }
        else
            reachedEndOfPath = false;
  
        if (Vector3.Distance(target.position,transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            anim.SetBool("StartWalking", true);
            bossHealth.SetActive(true);
            if (currentState == EnemyStateR.idle || currentState == EnemyStateR.walk && currentState != EnemyStateR.stagger)
            {
                anim.SetFloat("MoveX", (target.position.x - transform.position.x));
                anim.SetFloat("MoveY", (target.position.y - transform.position.y));
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - myRigidbody.position).normalized;
                Vector2 force = direction * enemyScribtableObject.speed * Time.deltaTime;
                myRigidbody.AddForce(force);
                ChangeState(EnemyStateR.walk); //Promjeni animaciju
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
        // Deselect the enemy
        DeSelect();

        // Deactivate health tip
        bossHealth.SetActive(false);

       

        // Play death sound
        FindObjectOfType<AudioManager>().Play("DieLog");

        // Deactivate the GameObject
        this.gameObject.SetActive(false);

        InitHearts2();

        // Report the enemy kill to the Redirect class
        Redirect.Killed(enemyScribtableObject.enemyName);

        // Spawn an item at a random position around the death location
        itemInside2.transform.position = this.transform.position;
        //itemInside2.SetActive(true);

        // Spawn a soul at the death location
        Instantiate(soul, transform.position + new Vector3(0, 0, 0), Quaternion.identity);


        // End the boss fight in the room movement
        RoomMove.bossFight = false;

        // Deactivate the boss wall
        WallBoss.SetActive(false);
        bossDefeated = true;
    }


    void ChangeState(EnemyStateR newState)
    {
        if (currentState != newState)
            currentState = newState;
    }

 


}

 
           
