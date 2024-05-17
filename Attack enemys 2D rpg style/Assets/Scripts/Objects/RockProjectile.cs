using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RockProjectile : Projectile
{
    public Transform target;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        FindObjectOfType<AudioManager>().Play("Spit");
    }
    private void Update()
    {
        /*if (Input.GetKey(KeyCode.P))
         {
             bool isTrue = true;
             if(isTrue)
             Launch(target.position);

         }
        */
       
        lifetimeSeconds -= Time.deltaTime;
        if (lifetimeSeconds <= 0)
        {
            //canFire = true;
            Destroy(this.gameObject);
            //lifetimeSeconds = lifetime;
        }

    }

    void Fire(Vector2 projectile)
    {
       
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target.position) < 0.2)
        {
           // PlayerScr. += 10;
            Destroy(gameObject);

        }
       
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerHurtBox"))
            Destroy(this.gameObject);
    }

}
