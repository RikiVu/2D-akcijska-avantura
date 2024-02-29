using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMoving : MonoBehaviour
{
    [SerializeField]
    protected Transform[] movespots;
    [SerializeField]
    protected Animator anim;
    protected int randomspots;
    private float timeWaiting;
    [SerializeField]
    private float moveCooldown;
    protected bool Reverse;
    [SerializeField]
    private float speed;
    private bool playerInRange;
    void Start()
    {
        Reverse = false;
        timeWaiting = moveCooldown;
        randomspots = 0;
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if(!playerInRange)
        {
            Move();
        }
       
    }
    public void Move()
    {
        if (movespots.Length != 0)
        {
            anim.SetBool("Moving", true);
            transform.position = Vector2.MoveTowards(transform.position, movespots[randomspots].position, speed * Time.deltaTime); //(current , target ,  speed)
            anim.SetFloat("x1", (movespots[randomspots].position.x - transform.position.x));
            anim.SetFloat("y1", (movespots[randomspots].position.y - transform.position.y));
            if (Vector2.Distance(transform.position, movespots[randomspots].position) < 0.2)  //(current , target ,  )
            {
                if (timeWaiting <= 0)
                {
                    if (randomspots < movespots.Length && Reverse == false)
                    {
                        if (randomspots != movespots.Length)
                        {
                            randomspots += 1;
                            if (randomspots == movespots.Length)
                                Reverse = true;
                        }
                    }
                    if (Reverse == true)
                    {
                        randomspots -= 1;
                        if (randomspots == 0)
                            Reverse = false;
                    }
                    anim.SetFloat("x", (movespots[randomspots].position.x - transform.position.x));
                    anim.SetFloat("y", (movespots[randomspots].position.y - transform.position.y));
                    anim.SetFloat("x1", (movespots[randomspots].position.x - transform.position.x));
                    anim.SetFloat("y1", (movespots[randomspots].position.y - transform.position.y));
                    timeWaiting = moveCooldown;
                }
                else
                {
                    timeWaiting -= Time.deltaTime;
                    anim.SetBool("Moving", false);
                }
            }
        }
    }

    public  void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            playerInRange = true;
            anim.SetBool("Moving", false);
        }
    }

    public  void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
            playerInRange = false;
    }
}


