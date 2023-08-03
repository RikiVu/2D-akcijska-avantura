using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : Enemy 
{
    private Rigidbody2D myRigidbody;
    //public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePositon;
    public Animator anim;
    public Transform[] path;
    public int currentPoint;
    public Transform currentGoal;
    public float roundingDistance;
    public float CountTime;


    void Start()
    {
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();

    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDistance();

    }
    void CheckDistance()
    {
        if (Vector3.Distance(target.position,
            transform.position) <= chaseRadius
            && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            anim.SetBool("StartWalking", true);

            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                anim.SetFloat("MoveX", (target.position.x - transform.position.x));
                anim.SetFloat("MoveY", (target.position.y - transform.position.y));

                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);

            }
        }
        else if( Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            anim.SetBool("StartWalking", true);
            if (Vector3.Distance(transform.position, path[currentPoint].position)> roundingDistance)
            {
               // CountTime += Time.deltaTime;
                Vector3 temp = Vector3.MoveTowards(transform.position, path[currentPoint].position, moveSpeed * Time.deltaTime);
                anim.SetFloat("MoveX", (path[currentPoint].position.x - transform.position.x));
                anim.SetFloat("MoveY", (path[currentPoint].position.y - transform.position.y));  
                
                myRigidbody.MovePosition(temp);
             ChangeState(EnemyState.walk);


            }
            else
            {
                CountTime = 0;
                ChangeGoal();
            }
           
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
    private void ChangeGoal()
    {
        if(currentPoint == path.Length -1)
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

}
