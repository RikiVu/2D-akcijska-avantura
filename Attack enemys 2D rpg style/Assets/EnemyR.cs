using UnityEngine;

public enum EnemyStateR
{
    idle,
    walk,
    attack,
    stagger,
}

public class EnemyR : MonoBehaviour
{
    [Header("Enemy Basics")]
    protected EnemyStateR currentState;
    protected GameObject player;
    protected Rigidbody2D myRigidbody;
    protected Animator anim;
    [SerializeField] protected Animator emoteAnimator;
    [SerializeField] protected float chaseRadius = 20f;
    [SerializeField] protected float attackRadius = 0.9f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    
}
