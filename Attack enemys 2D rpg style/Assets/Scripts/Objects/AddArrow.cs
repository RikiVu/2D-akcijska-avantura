using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddArrow : MonoBehaviour
{
    private GameObject Player;
    private bool collected = false;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            // collected = true;
            if (PlayerScr.Arrows < PlayerScr.MaxArrows)
            {
                if (PlayerScr.MaxArrows -1 == PlayerScr.Arrows )
                {
                    PlayerScr.Arrows += 1;
                }
                else
                {
                    PlayerScr.Arrows += 2;
                }
               //FindObjectOfType<AudioManager>().Play("HealHeart");
               // plyScr.PlayerHealthSignal.Raise();
                Destroy(gameObject);
            }



        }
    }
}
