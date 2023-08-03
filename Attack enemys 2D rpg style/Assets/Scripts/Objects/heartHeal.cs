using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heartHeal : MonoBehaviour
{
    private Transform Location;
    private Vector3 pos;
    private float speed = 30;
    private bool collected = false;
    private GameObject Player;
    private PlayerScr plyScr;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
      plyScr =  Player.GetComponent<PlayerScr>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            // collected = true;
            if(plyScr.currentHealth.RuntimeValue < plyScr.currentHealth.initialValue)
            {
                if(plyScr.currentHealth.RuntimeValue == plyScr.currentHealth.initialValue -1)
                {
                    plyScr.currentHealth.RuntimeValue += 1;
                }
                else
                {
                    plyScr.currentHealth.RuntimeValue += 2;                       
                }
                FindObjectOfType<AudioManager>().Play("HealHeart");       
                plyScr.PlayerHealthSignal.Raise();
                Destroy(gameObject);
            }
           
           

        }
    }

    void Animation(Vector3 position)
    {
        
        if (Vector2.Distance(transform.position, Location.position) < 0.2)
        {
          
            Destroy(gameObject);
            
          
        }

    }

    private void Update()
    {
       

    }
}
