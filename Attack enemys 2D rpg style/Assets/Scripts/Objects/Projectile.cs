using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    
    public float moveSpeed;
    public Vector2 directionToMove;
    public float lifetime;
    public float lifetimeSeconds;
    public Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        
        lifetimeSeconds = lifetime;
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       

        lifetimeSeconds -=  Time.deltaTime;
        if (lifetimeSeconds <= 0)
        {
            //canFire = true;
            Destroy(this.gameObject);
            //lifetimeSeconds = lifetime;
        }
    }
    /*
    private Vector2 Normalize (Vector3 Current)
    {
        
        Current.x =  x;
        Current.y =  y;
       
        return Current;
    }
    */

    public void Launch(Vector2 InitialVel)
    {

        myRigidbody.velocity = InitialVel.normalized * moveSpeed;

        
    }
    
}
