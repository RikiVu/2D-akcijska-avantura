
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class PlayerScr : MonoBehaviour
{

#pragma warning disable 649
    #region Variables;
    public static bool playerCanMove = true;
    public  bool SetGodMode = false;
    public static bool GodMode;
    //Player Move , animations, 
   // public Transform Spawn;
    public PlayerState currentState;
    public static bool canRun = true;
    public static float speed;
    float moveSpeed=100;
    private Vector3 change;
    private Rigidbody2D myRididbody;
    public Animator animator;
    private bool attackActive;
    private bool moving;
    //attack Coldown
    private float cooldown = 0.3f;
    private float cooldownSeconds;
    private bool cooldownBool;
    //Money
    public static float Gold = 4550;

    //Health
    public bool isTargetable = true;
   // public FloatValue currentHealth;
    public SignalSender PlayerHealthSignal;
    //Position
    public VectorValue startingPosition;

    // target 
    public Transform MyTarget { get; set; }
    public static bool CantAtt = true;

    //flash
    private bool FlashActive;
    [SerializeField]
    private float flashLenght = 0f;
    private float flashCounter = 0f;
    public SpriteRenderer PlayerSprite;

    //Sword
    public static bool haveSword= false;
    //Bow
    public static bool haveBow = false;
    public static int Arrows;
    public static int MaxArrows = 20;
    private float AimMAx = 0.7f;
    float startTime= 0;
    public GameObject projectile;
    public TextMeshProUGUI arrowCounter;
    public Vector3 Location;


    //layer change on obstacle 
    bool inCollision = false;

    //Dash
    public static bool CanDash = false;
    public static bool IsDashing = false;

    //armor
    public ArmorManager armorManager;

    private static PlayerScr playerInstance;

    public AlertPanelScr alertPanelScr;
    void Awake()
    {
        GodMode = SetGodMode;
        DontDestroyOnLoad(this);

        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static bool trci = false;
    #endregion


    void Start()
    {
        
        Arrows = 0;
        PlayerSprite = gameObject.GetComponent<SpriteRenderer>();
        //transform.position = Spawn.position;
        //cooldownSeconds = cooldown;
        // InvokeRepeating("PlaySound", 0.0f, 0.5f);
        moving = false;
        cooldownBool = true;
        currentState = PlayerState.walk;
        
        myRididbody = GetComponent<Rigidbody2D>();

        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        transform.position = startingPosition.initialValue;

        //pocne sva cetitri napadat kada se ne pomakne iz prve trea stavit pocetni float npr da je dolje 1
        if (GodMode)
        {
            haveSword = true;
           
            haveBow = true;
            Gold += 10000;
            Arrows = 20;
            Knockback.damageBoost = 2;
        }

    }
    private void Update()
    {
        if(arrowCounter !=null)
        arrowCounter.text = Arrows.ToString();
        if (Input.GetKeyDown(KeyCode.Q) && currentState != PlayerState.attack && currentState != PlayerState.stagger && cooldownBool == true && !trci && MyTarget != null && CanDash && !CantAtt)
        {
            FindObjectOfType<AudioManager>().Play("Dash");
            animator.SetFloat("moveX", MyTarget.position.x - transform.position.x);
            animator.SetFloat("moveY", MyTarget.position.y - transform.position.y);
            Vector3 tempVector = MyTarget.transform.position - transform.position;
            IsDashing = true;
            Dash(tempVector);
        }
    }

  

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FlashActive)
        {
            flash();
        }
        // Debug.Log(startTime);
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(KeyCode.Space) && currentState != PlayerState.attack && currentState != PlayerState.stagger && moving == false && cooldownBool == true && haveSword && !trci)
        {
            cooldownBool = false;
            StartCoroutine(AttackCo());           
        }
        else if( currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            if(playerCanMove)
                UpdateAnimationAndMove();
        }

        if (Input.GetKey(KeyCode.R) && currentState != PlayerState.attack && currentState != PlayerState.stagger && moving == false && cooldownBool == true && haveBow && !trci && !CantAtt)
        {

            if (MyTarget != null && Vector3.Distance(MyTarget.position, transform.position) <= 30 && Arrows > 0)
            {
                animator.SetFloat("moveX", MyTarget.position.x - transform.position.x);
                animator.SetFloat("moveY", MyTarget.position.y - transform.position.y);
                startTime += 1f * Time.deltaTime;
                if (startTime >= AimMAx)
                {
                    Arrows--;
                    Vector3 tempVector = MyTarget.parent.transform.position - transform.position;
                    Location = tempVector;
                    GameObject current = Instantiate(projectile, transform.position, Quaternion.identity);
                    current.GetComponent<Projectile>().Launch(tempVector);
                    StartCoroutine(FreezeCo());

                    // holding = false;

                }
                animator.SetBool("Bow", true);
            }

            else if(Arrows > 0)
             {
                Vector3 tempVector2 = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"), 1);
                Location = tempVector2;
                startTime += 1f * Time.deltaTime;
                if (startTime >= AimMAx)
                {
                   // Debug.Log("Shoot");
                    Arrows--;

                    GameObject current = Instantiate(projectile, transform.position, Quaternion.identity);
                    current.GetComponent<Projectile>().Launch(tempVector2);
                    StartCoroutine(FreezeCo());
                   // GameManager.turnOff = true;

                    // holding = false;

                }
                animator.SetBool("Bow", true);
            }
            else
            {
                startTime = 0;
                animator.SetBool("Bow", false);
            }
        }
        else
        {
            startTime = 0;
            animator.SetBool("Bow", false);
        }
    }
    #region Moving
    private void Dash(Vector2 InitialVel)
    {
       
        isTargetable = false;   
        myRididbody.velocity = InitialVel.normalized * moveSpeed;
        StartCoroutine(CastSpell());
       
    }

    private void UpdateAnimationAndMove()
    {
        if(attackActive== false)
        {
        if(change != Vector3.zero)
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


    internal void setactive(bool v)
    {
        throw new NotImplementedException();
    }
    
    public void MoveCharacter()
    {
        
        change.Normalize();
        myRididbody.MovePosition(transform.position + change * speed * Time.fixedDeltaTime);
       
        

    }
    public void AnimateMovement(Vector2 direction)
    {
       // animator.SetLayerWeight(1, 1);
       if(direction != Vector2.zero )
        {           
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
        animator.SetBool("moving", true);
        }
       
     }
    #endregion

    //Attack animation

    private IEnumerator FreezeCo()
    {
        cooldownBool = false;
        FindObjectOfType<AudioManager>().Play("BowShoot");
       
        yield return new WaitForSeconds(1);
        
        animator.SetBool("Bow", false);
        cooldownBool = true;

    }
    private IEnumerator CastSpell()
    {
        myRididbody.drag = 9;
        cooldownBool = false;
        
        animator.SetBool("attacking", true);
        animator.SetFloat("X", 1);
        currentState = PlayerState.attack;
        
        yield return new WaitForSeconds(0.5f);

        animator.SetBool("attacking", false);
        attackActive = false;
        currentState = PlayerState.walk;
        isTargetable = true;
        cooldownBool = true;
       
        animator.SetFloat("X", 0);
        myRididbody.drag = 2;
        AdrenalinScr.value = 0;
        CanDash = false;
        IsDashing = false;
        myRididbody.velocity = Vector2.zero;
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        attackActive = true;
        yield return new WaitForSeconds(0.2f);
        yield return null;

        animator.SetBool("attacking", false);
        attackActive = false;
        yield return new WaitForSeconds(0.1f);

        currentState = PlayerState.walk;
        yield return new WaitForSeconds(0.1f);
        cooldownBool = true;
    }
    private IEnumerator DeathCo()
    {
        GameManager.gameOver = true;
        FlashActive = false;
        PlayerSprite.color = new Color(255, 255, 255);
        myRididbody.velocity = Vector2.zero;
        animator.SetBool("Death", true);
        yield return new WaitForSeconds(1.5f);
        alertPanelScr.showAlertPanel("Game over!");
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
    }


    private void BowShoot()
    {
            animator.SetBool("Bow", false);
    }

                                                              //Knocking
    public void Knock(float knockTime, float damage)
    {
        if(isTargetable)
        {
            if (!GodMode)
            {
                if (armorManager.activeProtection && armorManager.hasProtection)
                {
                    FlashActive = true;
                    flashCounter = flashLenght;
                    AdrenalinScr.value -= 0.1f;
                    //FindObjectOfType<AudioManager>().Play("PlayerPain");
                    StartCoroutine(KnockCo(knockTime));
                    armorManager.ArmorHit();
                }

                else if (armorManager.activeProtection == false)
                {
                    HeartManager.playerCurrentHealth -= damage;
                    FlashActive = true;
                    flashCounter = flashLenght;
                    PlayerHealthSignal.Raise();
                    AdrenalinScr.value -= 0.1f;
                    if (HeartManager.playerCurrentHealth > 0)
                    {
                        FindObjectOfType<AudioManager>().Play("PlayerPain");
                        StartCoroutine(KnockCo(knockTime));
                    }
                    else
                    {
                        StartCoroutine(DeathCo());
                    }
                }
            }
            else
            {
                myRididbody.velocity = Vector2.zero;
                currentState = PlayerState.idle;
            }

        }
        else
        {
            currentState = PlayerState.idle;
        }
        

    } 
    private IEnumerator KnockCo( float knockTime)
    {
        if (myRididbody != null && isTargetable)
        {
            yield return new WaitForSeconds(knockTime);
            myRididbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRididbody.velocity = Vector2.zero;


        }
        else if(myRididbody != null && !isTargetable)
        {
            currentState = PlayerState.idle;
        }
    }
                                                                    //Health system 
    internal void HurtPlayer(object damageToGive)
    {
        throw new NotImplementedException();
    }
 

                                                                    // Other

    void PlaySound() 
    {
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            //      FindObjectOfType<AudioManager>().Play("Step");
        }
    }
    private void flash() // kad ga udari da mjenja kao boju 
    {
        if (FlashActive)
        {
            if (flashCounter > flashLenght * .99f)
            {
                PlayerSprite.color = new Color(0.4433962f, 0.03973833f, 0.03973833f);
            }
            else if (flashCounter > flashLenght * .82f)
            {
                PlayerSprite.color = new Color(PlayerSprite.color.r, PlayerSprite.color.g, PlayerSprite.color.b, 1f);

            }
            else if (flashCounter > flashLenght * .66f)
            {
                PlayerSprite.color = new Color(PlayerSprite.color.r, PlayerSprite.color.g, PlayerSprite.color.b, 0f);

            }
            else if (flashCounter > flashLenght * .49f)
            {
                PlayerSprite.color = new Color(PlayerSprite.color.r, PlayerSprite.color.g, PlayerSprite.color.b, 1f);

            }
            else if (flashCounter > flashLenght * .33f)
            {
                PlayerSprite.color = new Color(PlayerSprite.color.r, PlayerSprite.color.g, PlayerSprite.color.b, 0f);

            }
            else if (flashCounter > flashLenght * .16f)
            {
                PlayerSprite.color = new Color(PlayerSprite.color.r, PlayerSprite.color.g, PlayerSprite.color.b, 1f);

            }
            else if (flashCounter > 0f)
            {
                PlayerSprite.color = new Color(PlayerSprite.color.r, PlayerSprite.color.g, PlayerSprite.color.b, 0f);

            }
            else
            {
                PlayerSprite.color = new Color(255, 255, 255);
                FlashActive = false;

            }
            flashCounter -= Time.deltaTime;
        }
    }
}
