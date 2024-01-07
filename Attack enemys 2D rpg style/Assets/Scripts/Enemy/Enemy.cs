using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger,
}

public class Enemy : MonoBehaviour
{
   public Rigidbody2D myRigidbody;
    public Animator anim;
    public EnemyState currentState;
    private PlayerScr player;
    //Health
    public float Health = 4;
    public float MaxHealth = 4;
    public string enemyName;   
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public bool isTargetable = true;
    public float moveSpeed;
    protected bool Enemy1 = false;
    private Vector3 pos;
    public GameObject healthBar;
    public GameObject HealthTip_Go;
    public TextMeshProUGUI name;
    [SerializeField]
    private CanvasGroup healthGroup;
    public Transform hitBox;
    protected Transform target;
    public GameObject enemy;
    private GameObject Current;
    //Flash
    private bool FlashActive;
    [SerializeField]
    private float flashLenght = 0f;
    private float flashCounter = 0f;
    public SpriteRenderer enemySprite;

    //Enemy drop
    public Transform SpawnPosition;
    public GameObject itemInside;
    public GameObject soul;
    public int XpGive;

    // quest
    public Redirect_Quest Redirect;

    public virtual void DeSelect()
    {
         if(enemyName != "Arachne")
        {
            GameManager.haveTarget = false;
            InitHearts2();
            healthGroup.alpha = 0;
        }
     //   player.MyTarget = null;
        // player.MyTarget = null;

    }
    public virtual  Transform Select()
    {
        name.text = enemyName;
        Enemy1 = true;
        healthGroup.alpha = 1;
        InitHearts();     
        Current = this.gameObject;
        return hitBox;
    }
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

     
    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
      
        enemySprite = enemy.GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (Enemy1 == true)
        {
            if (Vector3.Distance(target.position,  Current.transform.position) >= 25 )
                DeSelect();
            UpdateHearts();
            pos = Camera.main.WorldToScreenPoint(healthBar.transform.position);
            ShowHealth(pos);
        }
        if (FlashActive)
            flash();
       
    }
    private void flash()
    {
         if(FlashActive && Health>0)
       {
           if(flashCounter > flashLenght *.99f)
           {
               enemySprite.color = new Color(1, 0.52f, 0.52f);
            }
           else if (flashCounter > flashLenght *.82f)
           {
               enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);

           }
           else if (flashCounter > flashLenght * .66f)
           {
               enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 0f);

           }
           else if (flashCounter > flashLenght * .49f)
           {
               enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);

           }
           else if (flashCounter > flashLenght * .33f)
           {
               enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 0f);

           }
           else if (flashCounter > flashLenght * .16f)
           {
               enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);

           }
           else if (flashCounter > 0f)
           {
               enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 0f);

           }
           else
           {
               enemySprite.color = new Color(255,255, 255);
               FlashActive = false;

           }
           flashCounter -= Time.deltaTime;
       }
      
    }

    public void ShowHealth(Vector3 position)
    {
        if(enemyName != "Arachne")
        {
            HealthTip_Go.transform.position = position;
            HealthTip_Go.SetActive(true);
        }
    }

    public void InitHearts()
    {
        for (int i = 0; i < MaxHealth / 2; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
    }
    public void InitHearts2()
    {
        Enemy1 = false;
        foreach (Image img in hearts)
        {
            img.gameObject.SetActive(false);
            HealthTip_Go.SetActive(false);
            //img.sprite = fullHeart;
        }
    }


    public void UpdateHearts()
    {

        name.text = enemyName;
        float tempHealth = Health / 2;
        for (int i = 0; i < MaxHealth/2; i++)
        {
            if (i <= tempHealth - 1)
            {
                //full health
                hearts[i].sprite = fullHeart;
            }
            else if (i >= tempHealth)
            {
                //emptyHeart
                hearts[i].sprite = emptyHeart;
            }
            else
            {
                //halfHeart
                hearts[i].sprite = halfHeart;
            }
        }
    }

    public virtual void Knock(Rigidbody2D myRigidbody, float knockTime, float damage)
    {
        if(isTargetable)
        {
            isTargetable = false;
            Health -= damage;
            //s isTargetable = false;
            if(Health<=0)
            {
                Death();
            }
            else
            {
                FlashActive = true;
                flashCounter = flashLenght;
                FindObjectOfType<AudioManager>().Play("LogPain");
                StartCoroutine(KnockCo(myRigidbody, knockTime));
            }
        }
    }

    public virtual void Death()
    {
        DeSelect();
        HealthTip_Go.SetActive(false);
        Enemy1 = false;
        FindObjectOfType<AudioManager>().Play("DieLog");
        this.gameObject.SetActive(false);
        //myRigidbody.velocity = Vector2.zero;
        //PlayerScr.CantAtt = true;
        InitHearts2();
        Redirect.Killed(enemyName);

        //remove restriction
        Instantiate(itemInside, SpawnPosition.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)), Quaternion.identity);
        Instantiate(soul, SpawnPosition.position + new Vector3(0, 0, 0), Quaternion.identity);
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if (myRigidbody != null && Health >0)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
            myRigidbody.velocity = Vector2.zero;
            isTargetable = true;
        }
    }

}
