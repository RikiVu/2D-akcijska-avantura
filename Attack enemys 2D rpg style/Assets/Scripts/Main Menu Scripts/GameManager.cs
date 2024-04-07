using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.PlayerLoop.PostLateUpdate;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerScr player;
    private EnemyR currentTarget;
    private pot currentBreakeble;
    private Transform tr;
    public GameObject Inventory;
    public GameObject QuestList;
    public Inventory InvScr;
    public GameObject ShopInventory;
    public GameObject ToolTip;
    public GameObject sprite1;
    GameObject current;
    public static bool turnOff=false;
   public static bool InvOppened = false;
   public bool QuestListOppened = false;
   public static bool haveTarget = false;
    public RunningScr RunningScript;

    public static bool bossAlive = true;
    public GameObject boss;

    public Stats stats;
    private int strenght=0;
    private int dexterity=0;
    private int constitution=0;
    public FloatValue playerCurrentHealth;
    [SerializeField] private TextMeshProUGUI StrenghtText;
    [SerializeField] private TextMeshProUGUI ConstitutionText;
    [SerializeField] private TextMeshProUGUI DexterityText;
    [SerializeField] private TextMeshProUGUI walkText;
    [SerializeField] private TextMeshProUGUI runText;
    public HeartManager heartManager;
    public static bool gameOver = false;




    private void Start()
    {
        Inventory.gameObject.SetActive(true);
        //ShopInventory.gameObject.SetActive(true);
        //  InvScr = Inventory.GetComponent<Inventory>();
        Inventory.gameObject.SetActive(false);
        //ShopInventory.gameObject.SetActive(false);
        QuestList.gameObject.SetActive(false);
        updateStats(stats.Strenght, stats.Dexterity, stats.Constitution);

    }

    public void updateStats(int Str, int Dex, int Const)
    {
        strenght += Str;
        dexterity += Dex;
        constitution += Const;
        StrenghtText.text = strenght.ToString();
        DexterityText.text = dexterity.ToString();
        ConstitutionText.text = constitution.ToString();

        RunningScript.dexUpdate(dexterity);
        RunningScript.constUpdateRunning(constitution);
        runText.text = RunningScr.maxSpeed.ToString();
        walkText.text = RunningScr.walkSpeed.ToString();
        Knockback.damageBoost = strenght;
        heartManager.constLogic(constitution);
        //playerCurrentHealth.initialValue += (float)constitution; 

    }

    void Update()
    {

        if(turnOff)
        {
            Destroy(current);
            turnOff = false;
        }

         ClickTarget();
     //ArrowShot();
        //Kodovi za otvaranje Inventory i Quest List
        if (Input.GetKeyDown(KeyCode.I) && InvOppened == false)
        {
            InvOppened = true;
            Inventory.gameObject.SetActive(true);
            //Time.timeScale = 0;
        }
       else  if(Input.GetKeyDown(KeyCode.I) && InvOppened == true )
        {
            InvOppened = false;
            Inventory.gameObject.SetActive(false);
            if (ShopInventory.activeInHierarchy == false)
            {
               // Time.timeScale = 1;
            }
            ToolTip.SetActive(false);

            //InvScr.DeactivateButton.SetActive(false);
            // InvScr.ActivateButton.SetActive(true);
        }
       

        if (Input.GetKeyDown(KeyCode.L) && QuestListOppened == false)
        {
            QuestListOppened = true;
            QuestList.gameObject.SetActive(true);           
        }
        else if (Input.GetKeyDown(KeyCode.L) && QuestListOppened == true)
        {
            QuestListOppened = false;
            QuestList.gameObject.SetActive(false);
            
            //InvScr.DeactivateButton.SetActive(false);
            // InvScr.ActivateButton.SetActive(true);
        }


    }





  public  void CloseInv()
    {
        InvOppened = false;
        Inventory.gameObject.SetActive(false);
        InvScr.DeactivateDeleteFunc();
        if (ShopInventory.activeInHierarchy == false)
        {
           // Time.timeScale = 1;
        }
        ToolTip.SetActive(false);
    }
    public void CloseQuests()
    {
        QuestListOppened = false;
        QuestList.gameObject.SetActive(false);

    }


    private void ClickTarget() 
    {
        if (Input.GetMouseButtonDown(0)) // Ako kliknemo s lijevim clickom na "targetable" mjesto
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 256);
           
            if(hit.collider != null) // ako smo pogodili collider
            {
               // Debug.Log("hit");
                haveTarget = true;
                PlayerScr.CantAtt = false;
                if ( currentTarget != null )
                {
                    currentTarget.DeSelect();                 
                }

                if(currentBreakeble != null)
                {
                    currentBreakeble.DeSelect();
                }

                currentTarget = hit.collider.GetComponent<EnemyR>();
                currentBreakeble = hit.collider.GetComponent<pot>();

                if(currentTarget != null)
                {                  
                    player.MyTarget = currentTarget.Select();
                }
                else
                {
                    player.MyTarget = currentBreakeble.Select();
                }

            }
            else 
            {
                PlayerScr.CantAtt = false;
                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }
                currentTarget = null;
                player.MyTarget = null;
            } 
        }
        else if(!haveTarget)
        {
            PlayerScr.CantAtt = false;
            if (currentTarget != null)
            {
                currentTarget.DeSelect();
            }
            currentTarget = null;
            player.MyTarget = null;
        }
    }


    public void ChangeTarget()
    {
        try
        {
            if (bossAlive)
            {
                haveTarget = true;
                PlayerScr.CantAtt = false;
                if (currentTarget != null)
                    currentTarget.DeSelect();

                currentTarget = boss.GetComponent<EnemyR>();

                if (currentTarget != null)
                    player.MyTarget = currentTarget.Select();
            }
        }
        catch
        {
            Debug.Log("error Gamemanager");
        }
        //if boss alive 
    
       
               
    }

    private void ArrowShot()
    {


        if (Input.GetMouseButtonDown(0)) // Ako kliknemo s lijevim clickom na "targetable" mjesto
        {
            if(current!=null)
            Destroy(current);

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 256);
           
            Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            temp.z = 0;
             current = Instantiate(sprite1, temp, Quaternion.identity);
            
           
           // tr.position = hit;
            
                PlayerScr.CantAtt = false;
                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }

                if (currentBreakeble != null)
                {
                    currentBreakeble.DeSelect();
                }

               // currentTarget = hit.collider.GetComponent<Enemy>();
             //   currentBreakeble = hit.collider.GetComponent<pot>();

                
                player.Location = temp;
              
               
            
         
        }
    }

}
