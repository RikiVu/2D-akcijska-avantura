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
    private HeartManager heartManager;
    private PlayerScr plyScr;

    private float lifetime = 10f;
    private float lifetimeSeconds;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        plyScr =  Player.GetComponent<PlayerScr>();
        lifetimeSeconds = lifetime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            // collected = true;
            if(HeartManager.playerCurrentHealth < HeartManager.playerMaxHealth)
            {
                if(HeartManager.playerCurrentHealth == HeartManager.playerMaxHealth - 1)
                    HeartManager.playerCurrentHealth += 1;
                else
                    HeartManager.playerCurrentHealth += 2;                       
               
                FindObjectOfType<AudioManager>().Play("HealHeart");       
                plyScr.PlayerHealthSignal.Raise();
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        lifetimeSeconds -= Time.deltaTime;
        if (lifetimeSeconds <= 0)
            Destroy(this.gameObject);
    }


}
