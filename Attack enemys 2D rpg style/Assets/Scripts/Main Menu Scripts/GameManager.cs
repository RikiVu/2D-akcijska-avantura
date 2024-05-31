using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
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
    public Redirect_Quest redirect_Quest;

    public pot[] potScr;
    public PickUp[] pickupScr;
    public ChestSCR[] chestSCRScr;
    public NpcQuestScr[] NpcQuestScr;

    public void giveIds()
    {
       for(i=0; i<potScr.Length; i++)
        {
            potScr[i].assignedId = i;
            addInPotList(potScr[i], false, i);
        }
        for (i = 0; i < pickupScr.Length; i++)
        {
            pickupScr[i].assignedId = i;
            addInPickupList(pickupScr[i], false, i);
        }
        for (i = 0; i < chestSCRScr.Length; i++)
        {
            chestSCRScr[i].assignedId = i;
            addInChestList(chestSCRScr[i], false, i);
        }
        for (i = 0; i < NpcQuestScr.Length; i++)
        {
            NpcQuestScr[i].assignedId = i;
            addInQuestList(NpcQuestScr[i], NpcQuestScr[i].questTaken, NpcQuestScr[i].questEnded, NpcQuestScr[i].NpcQuest, i);
        }
    }
    //cica 
    public void Passage(bool value)
    {
        allowPassage.load(value);
    }
    //quest log load and save
  public void  addInQuestList(NpcQuestScr npcQuestScr, bool taken, bool ended, Create_Quest whichQuest, int id)
    {
        QuestObjectLog questObjectLog = new QuestObjectLog();
        questObjectLog.npcQuestScr = npcQuestScr;
        questObjectLog.questTaken = taken;
        questObjectLog.questEnded = ended;
        questObjectLog.quest = whichQuest;
        questObjectLog.id = id;
        questObjectLogList.Add(questObjectLog);
    }
    public void addInQuestList(int Id, bool taken, bool ended)
    {
        questObjectLogList.Find(p => p.id == Id).questTaken = taken;
        questObjectLogList.Find(p => p.id == Id).questEnded = ended;
    }


    public void addInQuestList(int counter, Create_Quest quest)
    {
            questObject = questObjectLogList.Find(p => p.quest == quest);
        if (questObject != null)
        {
            questObject.count = counter;
            Debug.Log(counter);
        }
    }
    public void addInQuestList(int Id, int counter)
    {
        questObjectLogList.Find(p => p.id == Id).count = counter;
    }
    public void loadQuests(List<QuestObjectLog> list)
    {
        if (list != null)
        {
            redirect_Quest.DeleteAll();
   
           
            foreach (QuestObjectLog c in list)
            {
                Debug.Log(c.quest.name + " , "+ c.questTaken + " , "+ c.questEnded);
                c.npcQuestScr.loadQuestData(c.questTaken, c.questEnded, c.count);
                redirect_Quest.loadQuests(c);
                questObject =  questObjectLogList.Find(p => p.quest.name == c.quest.name);
                if (questObject != null)
                {
                    questObject.questTaken = c.questTaken;
                    questObject.questEnded = c.questEnded;
                    questObject.count = c.count;
                    questObject.quest = c.quest;

                }

            }
        }

    }
    //pickUp load and save
    public void addInPickupList(PickUp pickUpScr, bool state, int id)
    {
        Debug.Log(pickUpScr.item.name + " , picked ? : " + state);
        ItemsOnGroundObject tempPickUpObject = new ItemsOnGroundObject();
        tempPickUpObject.pickUpScr = pickUpScr;
        tempPickUpObject.picked = state;
        tempPickUpObject.id = id;
        pickupList.Add(tempPickUpObject);
    }
    public void addInPickupList(int Id, bool state)
    {
        Debug.Log(Id + " , " + state);
        pickupList.Find(p => p.id == Id).picked = state;
    }
    public void loadPickUpItems(List<ItemsOnGroundObject> list)
    {
        if (list != null)
        {
            //pickupList = list;
            for (int i = 0; i < pickupList.Count; i++)
            {
                pickupList[i].pickUpScr.loadItem(list[i].picked);
            }
         
        }

    }

    //pot load and save
    public void addInPotList(pot potScr, bool state,int id)
    {
        PotObject tempPotObject = new PotObject();
        tempPotObject.potScr = potScr;
        tempPotObject.broken = state;
        tempPotObject.id = id;
        potList.Add(tempPotObject);
    }
    public void addInPotList( int Id,bool state)
    {
        potList.Find(p => p.id == Id).broken = state;
    }
    public void loadPots(List<PotObject> list)
    {
        if (list != null)
        {
            //potList = list;
            /*
            foreach (PotObject c in potList)
            {
                c.potScr.loadPot(c.broken);
            }
            */

            //chestList = list;
            for (int i = 0; i < potList.Count; i++)
            {
                potList[i].potScr.loadPot(list[i].broken);
            }
        }
      
    }


    //chest load and save
    public void addInChestList(ChestSCR chestScr, bool state, int id)
    {
        ChestObject tempChestObject = new ChestObject();
        tempChestObject.chestScr = chestScr;
        tempChestObject.collected = state;
        tempChestObject.id = id;
        chestList.Add(tempChestObject);
    }
    public void addInChestList(bool state, int Id)
    {
        chestList.Find(p => p.id == Id).collected = state;
    }
    public void loadChests(List<ChestObject> list)
    {
        if (list != null)
        {
            //chestList = list;
            for (int i = 0; i< chestList.Count; i++)
            {
                chestList[i].chestScr.loadChest(list[i].collected);
            }
          
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
        giveIds();
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


