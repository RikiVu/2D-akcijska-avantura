using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyR : MonoBehaviour
{

    [Header("Enemy Basics")]
    protected GameObject player;
    protected Rigidbody2D myRigidbody;
    [SerializeField] protected float chaseRadius = 20f;
    [SerializeField] protected float attackRadius = 0.9f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myRigidbody = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
