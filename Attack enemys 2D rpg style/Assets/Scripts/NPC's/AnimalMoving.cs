using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMoving : MonoBehaviour
{
    public float speed;
    public float minChangeInterval = 5f;
    public float maxChangeInterval = 15f;
    protected int randomspots;
    public Animator anim;
    public Transform[] movespots;
    protected Vector2 direction;
    protected bool Reverse;
    private float changeTimer;
    private bool animationParametersSet = false;
    // Start is called before the first frame update
    void Start()
    {
        Reverse = false;
        randomspots = Random.Range(0, movespots.Length);
        anim = GetComponent<Animator>();
        direction = Vector2.zero;
        changeTimer = Random.Range(minChangeInterval, maxChangeInterval);
        anim.SetBool("Moving", true);

}

    void Update()
    {
        Move();
    }

    public void Move()
    {
        if (movespots.Length != 0)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, movespots[randomspots].position, speed * Time.deltaTime); //(current , target ,  speed)
            anim.SetFloat("x1", (movespots[randomspots].position.x - transform.position.x));
            anim.SetFloat("y1", (movespots[randomspots].position.y - transform.position.y));
            if (Vector2.Distance(transform.position, movespots[randomspots].position) < 0.2)  //(current , target ,  )
            {
                
                anim.SetBool("Moving", false);
                if (!animationParametersSet) // Check if animation parameters have not been set yet
                {
                    anim.SetFloat("x", Random.Range(-1f, 1f)); 
                    anim.SetFloat("y", Random.Range(-1f, 1f)); 
                    animationParametersSet = true; 
                }
                if (changeTimer <= 0)
                {
                    randomspots = Random.Range(0, movespots.Length);
                    changeTimer = Random.Range(minChangeInterval, maxChangeInterval);
                    anim.SetBool("Moving", true);
                    animationParametersSet = false;
                }
                else
                {
                    changeTimer -= Time.deltaTime;
                }

               
            }
        }
    }
}
