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


    private void OnTriggerEnter2D(Collider2D other)
    {
      

        if (other.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("PlayerHitBox")||  other.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Projectile") && other.isTrigger) 
        {
              other.GetComponent<pot>().Smash();
        }
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player")&& other.isTrigger)
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if(hit != null)
            {
                if(other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Enemy"))
                {
                    return;
                }
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);
                if (other.gameObject.CompareTag("Enemy") &&  other.isTrigger)
                {
                    
                    hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    if (PlayerScr.IsDashing)
                    {
                        other.GetComponent<Enemy>().Knock(hit, knockTime, damage + damageBoost);
                    }
                    else
                    {
                        
                        other.GetComponent<Enemy>().Knock(hit, knockTime, damage + damageBoost);
                        AdrenalinScr.value += 0.1f;
                    }
                }
                if(other.gameObject.CompareTag("Player"))
                {
                    if(other.GetComponent<PlayerScr>().currentState != PlayerState.stagger)
                    {
                    hit.GetComponent<PlayerScr>().currentState = PlayerState.stagger;
                    other.GetComponent<PlayerScr>().Knock(knockTime, damage);

                    }
                }

            }
        }

    }

}
