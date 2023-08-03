using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deathAnimScr : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    public SpriteRenderer sprRend;
    private float lifetime =1.5f;
    private float lifetimeSeconds;
    //public Image image;
    // Start is called before the first frame update
    void Awake()
    {
        lifetimeSeconds = lifetime;
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lifetimeSeconds -= Time.deltaTime;
        if (lifetimeSeconds <= 0)
        {
            //canFire = true;
           this.gameObject.SetActive(false);
            
            //lifetimeSeconds = lifetime;
        }
        else
        {
            sprRend.color = new Color(1, 1, 1, lifetimeSeconds / 1.5f) ;
            myRigidbody.velocity = new Vector3(0, 3, 0);
        }
       
    }
   
}
