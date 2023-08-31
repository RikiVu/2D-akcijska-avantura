using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public enum Type
{
    SamePosition,
    DiffrentPosition
}
public class EnemyTurret : Enemy
{
 //   private Rigidbody2D myRigidbody;
    //public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePositon;
  //  public Animator anim;

    public GameObject projectile;
    public float FireDelay;
    private float fireDelaySeconds;  
    private bool canFire = true;
    public Transform[] path;

    public int currentPoint;
    public Transform currentGoal;
    public float roundingDistance;
    private float Stoping = 3;
    private float stoppingSeconds;
    private bool StopGM = false;

    public Type currentEnemy;

    void Start()
    {
        currentState = EnemyState.idle;
       // myRigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        //   anim = GetComponent<Animator>();
        anim = GetComponent<Animator>();
    }
  

    // Update is called once per frame
    void FixedUpdate()
    {
        if(this.currentEnemy == Type.SamePosition)
        {
            CheckDistanceSameLoc();
        }
        if (this.currentEnemy == Type.DiffrentPosition)
        {
            CheckDistance();
        }
        
        fireDelaySeconds -= Time.deltaTime;
        if (fireDelaySeconds <= 0)
        {
            if (StopGM)
            {
                anim.SetFloat("MoveX", (path[currentPoint].position.x - transform.position.x));
                anim.SetFloat("MoveY", (path[currentPoint].position.y - transform.position.y));
                canFire = false;
                Vector3 temp = Vector3.MoveTowards(transform.position, path[currentPoint].position, moveSpeed * Time.deltaTime);
                myRigidbody.MovePosition(temp);
            }
            else if (!StopGM)
            {
                canFire = true;
                anim.SetFloat("MoveX", (target.position.x - transform.position.x));
                anim.SetFloat("MoveY", (target.position.y - transform.position.y));
                fireDelaySeconds = FireDelay;
            }
          

        }

    }
    public  void CheckDistance()
    {

        anim.SetBool("StartWalking", true);
        if (Vector3.Distance(transform.position, target.position) > chaseRadius)
        {
            if (Vector3.Distance(transform.position, path[currentPoint].position) > roundingDistance)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, path[currentPoint].position, moveSpeed * Time.deltaTime);
                anim.SetFloat("MoveX", (path[currentPoint].position.x - transform.position.x));
                anim.SetFloat("MoveY", (path[currentPoint].position.y - transform.position.y));
                myRigidbody.MovePosition(temp);
            }
            else
            {
                ChangeGoal();
                // anim.SetBool("StartWalking", false);
            }
        }

        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (Vector3.Distance(transform.position, path[currentPoint].position) > roundingDistance)
            {

               



                if (stoppingSeconds <= 0 && StopGM == false)
                {

                    StopGM = true;
                   
                    stoppingSeconds = Stoping;
                }
                else if (stoppingSeconds <= 0 && StopGM == true)
                {
                   
                    
                    StopGM = false;
                    stoppingSeconds = Stoping;
                }
                stoppingSeconds -= Time.deltaTime;
            }
            else
            {
                ChangeGoal();
                // anim.SetBool("StartWalking", false);
            }

            
            //Debug.Log(stoppingSeconds);
           
            

            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {

                if (canFire)
                {
                   
                    Vector3 tempVector = target.transform.position - transform.position;

                    GameObject current = Instantiate(projectile, transform.position, Quaternion.identity);
                    current.GetComponent<Projectile>().Launch(tempVector);
                    canFire = false;
                }

                // ChangeState(EnemyState.walk);

            }
        }

    }

    public void CheckDistanceSameLoc()
    {

        anim.SetBool("StartWalking", true);
        if (Vector3.Distance(transform.position, target.position) > chaseRadius)
        {
            if (Vector3.Distance(transform.position, path[currentPoint].position) > roundingDistance)
            {

                Vector3 temp = Vector3.MoveTowards(transform.position, path[currentPoint].position, moveSpeed * Time.deltaTime);
                anim.SetFloat("MoveX", (path[currentPoint].position.x - transform.position.x));
                anim.SetFloat("MoveY", (path[currentPoint].position.y - transform.position.y));
                myRigidbody.MovePosition(temp);



            }
            else
            {
                ChangeGoal();
                // anim.SetBool("StartWalking", false);
            }
        }

        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
           

                if (stoppingSeconds <= 0 && StopGM == false)
                {

                    StopGM = true;

                    stoppingSeconds = Stoping;
                }
                else if (stoppingSeconds <= 0 && StopGM == true)
                {


                    StopGM = false;
                    stoppingSeconds = Stoping;
                }
                stoppingSeconds -= Time.deltaTime;
           
           
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {

                if (canFire)
                {
                   
                    Vector3 tempVector = target.transform.position - transform.position;

                    GameObject current = Instantiate(projectile, transform.position, Quaternion.identity);
                    current.GetComponent<Projectile>().Launch(tempVector);
                    
                    canFire = false;
                }

                // ChangeState(EnemyState.walk);

            }
        }

    }

    private void ChangeGoal()
    {
        if (currentPoint == path.Length - 1)
        {
            currentPoint = 0;
            currentGoal = path[0];
        }
        else
        {
            currentPoint++;
            currentGoal = path[currentPoint];


        }
    }

    private void ChangeState(EnemyState newState)
    {
        if(currentState != newState)
        {
            currentState = newState;
        }
    }

  

}
