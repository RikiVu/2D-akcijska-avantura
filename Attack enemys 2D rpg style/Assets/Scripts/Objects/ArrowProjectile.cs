using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : Projectile
{
    public Transform target;
    private Animator animator;
    private Vector3 change;
    private Transform playerGM;
    private PlayerScr plyScr;
    // Start is called before the first frame update

    private void Awake()
    {
        change = transform.position;
        playerGM = GameObject.FindGameObjectWithTag("Player").transform;
        plyScr = playerGM.GetComponent<PlayerScr>();
        animator = GetComponent<Animator>();
        if(target !=null)
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
        
            AnimateMovement(plyScr.Location);

        //AnimateMovement(plyScr.Location - change);
        //  change = PlayerScr. .position;
    }

    // Update is called once per frame
    void Update()
    {
      //  Debug.Log(plyScr.MyTarget.position - plyScr.transform.position);
        
        lifetimeSeconds -= Time.deltaTime;
        if (lifetimeSeconds <= 0)
        {
            //canFire = true;
            Destroy(this.gameObject);
            //lifetimeSeconds = lifetime;
        }
    }
    public void AnimateMovement(Vector2 direction)
    {
      
        if (direction != Vector2.zero)
        {
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);

        }

    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("breakable") && other.isTrigger)
            {
             Destroy(this.gameObject);
        }
        else
        {
          // return;
        }

    }
}
