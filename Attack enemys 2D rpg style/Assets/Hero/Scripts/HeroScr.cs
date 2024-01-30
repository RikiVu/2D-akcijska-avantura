
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum HeroState
{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class HeroScr : MonoBehaviour
{
#pragma warning disable 649
    #region Variables;

    //Player Move , animations, 
    public HeroState currentState;
    public static float speed = 20;
    private Vector3 change;
    private Rigidbody2D myRididbody;
    public Animator animator;
    private bool attackActive;
    private bool moving;
    //attack Coldown
    private bool cooldownBool;

    //Position
    public VectorValue startingPosition;

    public SpriteRenderer PlayerSprite;

    //Sword
    public static bool haveSword = true;
  
    private static HeroScr playerInstance; 
    #endregion
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (playerInstance == null)
            playerInstance = this;

        else
            Destroy(gameObject);


        PlayerSprite = gameObject.GetComponent<SpriteRenderer>();

        moving = false;
        cooldownBool = true;
        currentState = HeroState.walk;

        myRididbody = GetComponent<Rigidbody2D>();

        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        transform.position = startingPosition.initialValue;
    }

    void FixedUpdate()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.Space) && currentState != HeroState.attack && currentState != HeroState.stagger && moving == false && cooldownBool == true && haveSword )
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == HeroState.walk || currentState == HeroState.idle)
        {
            UpdateAnimationAndMove();
        }
    }

    #region Moving
    private void UpdateAnimationAndMove()
    {
        if (attackActive == false)
        {
          
            if (change != Vector3.zero)
            {
                moving = true;
                MoveCharacter();
                animator.SetFloat("moveX", change.x);
                animator.SetFloat("moveY", change.y);
                animator.SetBool("moving", true);

            }
            else
            {
                animator.SetBool("moving", false);
                moving = false;
            }
        }
    }
    public void MoveCharacter()
    {
        change.Normalize();
        myRididbody.MovePosition(transform.position + change * speed * Time.fixedDeltaTime);
    }
    public void AnimateMovement(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            animator.SetBool("moving", true);
        }
    }
    #endregion

    //Attack animation
    private IEnumerator AttackCo()
    {
        cooldownBool = false;
        animator.SetBool("attacking", true);
        currentState = HeroState.attack;
        attackActive = true;
        do
        {
            yield return null;
        } while (animator.GetCurrentAnimatorStateInfo(0).IsName("attacking"));
        animator.SetBool("attacking", false);
        attackActive = false;
        yield return new WaitForSeconds(.1f);
        currentState = HeroState.walk;
        yield return new WaitForSeconds(.1f);
        cooldownBool = true;
    }

}
