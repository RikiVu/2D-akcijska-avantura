using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;
    public static float damageBoost = 0;

    // Start is called before the first frame update

    Rigidbody2D hit;
    private void OnTriggerEnter2D(Collider2D other)
    {
      

        if (other.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("PlayerHitBox")||  other.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Projectile") && other.isTrigger) 
        {
              other.GetComponent<pot>().Smash();
        }
      
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("PlayerHurtBox"))
        {

            if (other.gameObject.CompareTag("PlayerHurtBox"))
            {
               
                 hit = other.GetComponentInParent<Rigidbody2D>();
            }
            else
            {
                 hit = other.GetComponent<Rigidbody2D>();
            }
            
          
            if (hit != null)
            {
                Debug.Log(other.tag);
                if (other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Enemy"))
                {
                    return;
                }
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);
                if (other.gameObject.CompareTag("Enemy") &&  other.isTrigger)
                {
                    
                    hit.GetComponent<EnemyR>().currentState = EnemyStateR.stagger;
                    if (PlayerScr.IsDashing)
                    {
                        other.GetComponent<EnemyR>().Knock(hit, knockTime, damage + damageBoost);
                    }
                    else
                    {
                        
                        other.GetComponent<EnemyR>().Knock(hit, knockTime, damage + damageBoost);
                        AdrenalinScr.value += 0.1f;
                    }
                }
                if(other.gameObject.CompareTag("PlayerHurtBox"))
                {
                    if(other.GetComponentInParent<PlayerScr>().currentState != PlayerState.stagger)
                    {
                    hit.GetComponent<PlayerScr>().currentState = PlayerState.stagger;
                    other.GetComponentInParent<PlayerScr>().Knock(knockTime, damage);

                    }
                }

            }
        }

    }

}
