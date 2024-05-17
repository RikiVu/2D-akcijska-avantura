using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public enum EnemyStateR
{
    idle,
    walk,
    attack,
    stagger,
}

public class EnemyR : MonoBehaviour
{
    [SerializeField] protected CreateEnemy enemyScribtableObject;
    [Header("Enemy Basics")]
    [SerializeField] protected float Health;
    public EnemyStateR currentState;
    protected GameObject player;
    protected Rigidbody2D myRigidbody;
    protected Animator anim;
    [SerializeField] protected Animator emoteAnimator;
    // test new 
    protected SpriteRenderer enemySprite;
    
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    //Enemy drop
    public GameObject itemInside;
    public GameObject heartSpawn;
    public GameObject soul;
    public bool isTargetable = true;   
    protected bool currentSelected = false; //ako je selectan
    private Vector3 pos;

    public GameObject healthBar;
    private GameObject HealthTip_Go;

    private TextMeshProUGUI name;

    private CanvasGroup healthGroup;
    public Transform hitBox;
   
    protected Transform target;
    private GameObject enemy;
    private GameObject Current;
    //Flash
    protected bool FlashActive;
    [SerializeField]
    protected float flashLenght = 0f;
    protected float flashCounter = 0f;

    // quest
    private GameObject QuestPanel;
    protected Redirect_Quest Redirect;

    private GameObject EnemyToolTip;

    //knock heart
    public GameObject knockHeart;
    public GameObject knockHeart2;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        enemySprite = this.GetComponent<SpriteRenderer>();
        Health = enemyScribtableObject.MaxHealth;
        target = player.transform;
    }
    void LateUpdate()
    {
        if (currentSelected == true && enemyScribtableObject.isBoss==false)
        {
            if (Vector3.Distance(target.position, Current.transform.position) >= 25)
                DeSelect();
            pos = Camera.main.WorldToScreenPoint(healthBar.transform.position);
            ShowHealth(pos);
        }
        if (FlashActive)
            flash();
    }

    public virtual void DeSelect()
    {
        if (enemyScribtableObject.isBoss == false)
        {
            GameManager.haveTarget = false;
            InitHearts2();
            healthGroup.alpha = 0;
        }
    }
    public virtual Transform Select()
    {
        Debug.Log(enemyScribtableObject.enemyName);
        currentSelected = true;
        Current = this.gameObject;
        if (enemyScribtableObject.isBoss == false)
        {
            name.text = enemyScribtableObject.enemyName;
            healthGroup.alpha = 1;
            InitHearts();
            UpdateHearts();
            //
        }
           
     
        return hitBox;
    }
    void Awake()
    {
        if (enemyScribtableObject.isBoss == false)
        {
            healthGroup = GameObject.FindGameObjectWithTag("EnemyCanvas").GetComponent<CanvasGroup>();
            HealthTip_Go = GameObject.FindGameObjectWithTag("EnemyHealth");
            name = GameObject.FindGameObjectWithTag("EnemyName").GetComponent<TextMeshProUGUI>();
            EnemyToolTip = GameObject.FindGameObjectWithTag("EnemyToolTip");
            hearts = EnemyToolTip.GetComponent<EnemyToolTip>().hearts;
        }
        QuestPanel = GameObject.FindGameObjectWithTag("QuestPanel");
        Redirect = QuestPanel.GetComponent<Redirect_Quest>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

    }
    public void ShowHealth(Vector3 position)
    {
            HealthTip_Go.transform.position = position;
            HealthTip_Go.SetActive(true);
    }
    private void flash()
    {
        if (FlashActive && Health > 0)
        {
            if (flashCounter > flashLenght * .99f)
            {
                enemySprite.color = new Color(1, 0.52f, 0.52f);
            }
            else if (flashCounter > flashLenght * .82f)
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
                enemySprite.color = new Color(255, 255, 255);
                FlashActive = false;

            }
            flashCounter -= Time.deltaTime;
        }

    }


    public void InitHearts()
    {
        for (int i = 0; i < enemyScribtableObject.MaxHealth / 2; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
    }
 
    public void InitHearts2()
    {
        if (enemyScribtableObject.isBoss == false)
        {
            currentSelected = false;
            foreach (Image img in hearts)
            {
                img.gameObject.SetActive(false);
            }
        }
        
    }
    public void UpdateHearts()
    {

        //name.text = enemyScribtableObject.enemyName;
        float tempHealth = Health / 2;
        for (int i = 0; i < enemyScribtableObject.MaxHealth / 2; i++)
        {
            if (i <= tempHealth - 1)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (i >= tempHealth)
            {
                hearts[i].sprite = emptyHeart;
            }
            else
            {
                hearts[i].sprite = halfHeart;
            }
        }
    }
   protected int heartsToSpawn;
    public virtual void Knock(Rigidbody2D myRigidbody, float knockTime, float damage)
    {
        if (isTargetable)
        {
            isTargetable = false;
           
            if (Health - damage <= 0)
            {
                heartsToSpawn = (int)(Health / 2);
                Debug.Log(heartsToSpawn);
            }
             
            else
                heartsToSpawn = (int)(damage / 2);
            Health -= damage;
            UpdateHearts();
            for (int i = 0; i < heartsToSpawn; i++)
            {
                Instantiate(knockHeart, transform.position + new Vector3(0, 1, 0) * (i + 1), Quaternion.identity);
            }
            if (damage % 2 == 1 && Health>=0)
                Instantiate(knockHeart2, transform.position + new Vector3(0, 1, 0) * (heartsToSpawn + 1), Quaternion.identity);
          
            if (Health <= 0)
                Death();
            else
            {
                FlashActive = true;
                flashCounter = flashLenght;
                FindObjectOfType<AudioManager>().Play("LogPain");
                StartCoroutine(KnockCo(myRigidbody, knockTime));
            }
        }
    }
    int counterI;
    Vector3 tempVectorSpawnCoin;
    public virtual void Death()
    {
        DeSelect();
        currentSelected = false;
        FindObjectOfType<AudioManager>().Play("DieLog");
        InitHearts2();
        Redirect.Killed(enemyScribtableObject.enemyName);
        SpawnEnemiesArea.currentMinionCount--;
        //remove restriction
        tempVectorSpawnCoin = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        for (counterI = 0; counterI < enemyScribtableObject.gold; counterI++)
        {
            Instantiate(itemInside, transform.position + tempVectorSpawnCoin, Quaternion.identity);
        }
        counterI = Random.Range(1, 10);
        if(counterI <= 2)
        {
            Instantiate(heartSpawn, transform.position + tempVectorSpawnCoin, Quaternion.identity);
        }
        Instantiate(soul, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        Destroy(this.gameObject);
    }

    protected IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if (myRigidbody != null && Health > 0)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyStateR.idle;
            myRigidbody.velocity = Vector2.zero;
            isTargetable = true;
        }
    }


}
