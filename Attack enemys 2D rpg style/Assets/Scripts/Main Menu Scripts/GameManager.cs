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
    public GameObject QuestTitle;
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
    public List<PotObject> potList = new List<PotObject>();
    public List<ItemsOnGroundObject> pickupList = new List<ItemsOnGroundObject>();
    public List<ChestObject> chestList = new List<ChestObject>();
    public List<QuestObjectLog> questObjectLogList = new List<QuestObjectLog>();
    public List<PlantAndHarvestObject> plantList = new List<PlantAndHarvestObject>();
    public AllowPassage allowPassage;
    private QuestObjectLog questObject;
    //  public List<ChestObject> itemsOnGroundList = new List<ChestObject>();
    int i = 0;

    //cica 
    public void Passage(bool value)
    {
        allowPassage.load(value);
    }
    //quest log load and save
    public int addInQuestList(NpcQuestScr npcQuestScr, bool taken, bool ended, Create_Quest whichQuest)
    {
        QuestObjectLog questObjectLog = new QuestObjectLog();
        questObjectLog.npcQuestScr = npcQuestScr;
        questObjectLog.questTaken = taken;
        questObjectLog.questEnded = ended;
        questObjectLog.quest = whichQuest;
        questObjectLog.id = i;
        ++i;
        questObjectLogList.Add(questObjectLog);
        return questObjectLog.id;
    }
    public void addInQuestList(int Id, bool taken, bool ended)
    {
        questObjectLogList.Find(p => p.id == Id).questTaken = taken;
        questObjectLogList.Find(p => p.id == Id).questEnded = ended;
    }
   
    public int addInQuestList(QuestController controllerScr, int counter, Create_Quest quest)
    {
            questObject = questObjectLogList.Find(p => p.quest == quest);
        if (questObject != null)
        {
            questObject.controllerScr = controllerScr;
            questObject.count = counter;
            return questObject.id;
        }
        return -1;
        //  questObjectLogList.Find(p => p.quest == quest).controllerScr = controllerScr;
        // questObjectLogList.Find(p => p.quest == quest).count = counter;
    }
    public void addInQuestList(int Id, int counter)
    {
        questObjectLogList.Find(p => p.id == Id).count = counter;
    }
    public void loadQuests(List<QuestObjectLog> list)
    {
        if (list != null)
        {
            questObjectLogList = list;
            foreach (QuestObjectLog c in questObjectLogList)
            {
                c.npcQuestScr.loadQuestData(c.questTaken, c.questEnded);
              //  c.controllerScr.(c.questTaken,c.questEnded);
            }
        }

    }
    //pickUp load and save
    public int addInPickupList(PickUp pickUpScr, bool state)
    {
        ItemsOnGroundObject tempPickUpObject = new ItemsOnGroundObject();
        tempPickUpObject.pickUpScr = pickUpScr;
        tempPickUpObject.picked = state;
        tempPickUpObject.id = i;
        ++i;
        pickupList.Add(tempPickUpObject);
        return tempPickUpObject.id;
    }
    public void addInPickupList(int Id, bool state)
    {
        pickupList.Find(p => p.id == Id).picked = state;
    }
    public void loadPickUpItems(List<ItemsOnGroundObject> list)
    {
        if (list != null)
        {
            pickupList = list;
            foreach (ItemsOnGroundObject c in pickupList)
            {
                c.pickUpScr.loadItem(c.picked);
            }
        }

    }

    //pot load and save
    public int addInPotList(pot potScr, bool state)
    {
        PotObject tempPotObject = new PotObject();
        tempPotObject.potScr = potScr;
        tempPotObject.broken = state;
        tempPotObject.id = i;
        ++i;
        potList.Add(tempPotObject);
        return tempPotObject.id;
    }
    public void addInPotList( int Id,bool state)
    {
        potList.Find(p => p.id == Id).broken = state;
    }
    public void loadPots(List<PotObject> list)
    {
        if (list != null)
        {
            potList = list;
            foreach (PotObject c in potList)
            {
                c.potScr.loadPot(c.broken);
            }
        }
      
    }


    //chest load and save
    public int addInChestList(ChestSCR chestScr, bool state)
    {
        ChestObject tempChestObject = new ChestObject();
        tempChestObject.chestScr = chestScr;
        tempChestObject.collected = state;
        tempChestObject.id = i;
        ++i;
        chestList.Add(tempChestObject);
        return tempChestObject.id;
    }
    public void addInChestList(bool state, int Id)
    {
        chestList.Find(p => p.id == Id).collected = state;
    }
    public void loadChests(List<ChestObject> list)
    {
        chestList = list;
        foreach (ChestObject c in chestList)
        {
            c.chestScr.loadChest(c.collected);
        }
    }

    //plant load and save
    public int addInPlantList(PlantAndHarvest plantScr, PlantState state, float time)
    {
        PlantAndHarvestObject tempPlantObject = new PlantAndHarvestObject();
        tempPlantObject.plantAndHarvestScr = plantScr;
        tempPlantObject.isPlanted = state;
        tempPlantObject.stageTime = time;
        tempPlantObject.id = i;
        ++i;
        plantList.Add(tempPlantObject);
        return tempPlantObject.id;
    }
    public void addInPlantList(PlantState state, int Id)
    {
        plantList.Find(p => p.id == Id).isPlanted = state;
    }
  
    public void loadPlant(List<PlantAndHarvestObject> list)
    {
        if (list != null)
        {
            plantList = list;
            foreach (PlantAndHarvestObject c in plantList)
            {
                c.plantAndHarvestScr.loadPlant(c.isPlanted, c.plantAndHarvestScr.itemPlanted);
            }
        }
        else
        {

            Debug.Log("empty plant list");
        }
    }



    private void Start()
    {
 
        Inventory.gameObject.SetActive(true);
        //ShopInventory.gameObject.SetActive(true);
        //  InvScr = Inventory.GetComponent<Inventory>();
        Inventory.gameObject.SetActive(false);
        //ShopInventory.gameObject.SetActive(false);
        QuestList.gameObject.SetActive(false);
        QuestTitle.gameObject.SetActive(false);
        updateStats(stats.Strenght, stats.Dexterity, stats.Constitution);
    }

    public void updateStats(int Str, int Dex, int Const)
    {
        strenght += Str;
        dexterity += Dex;
        constitution += Const;
        if(strenght < 0)
            strenght = 0;
        if (dexterity < 0)
            dexterity = 0;
        if (constitution < 0)
            constitution = 0;
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
            QuestTitle.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.L) && QuestListOppened == true)
        {
            QuestListOppened = false;
            QuestList.gameObject.SetActive(false);
            QuestTitle.gameObject.SetActive(false);

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
        QuestTitle.gameObject.SetActive(false);

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


