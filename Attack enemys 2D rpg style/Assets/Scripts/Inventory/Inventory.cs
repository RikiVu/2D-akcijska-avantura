    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using UnityEngine.Rendering.Universal;
using Unity.Loading;


public class Inventory : MonoBehaviour
{
    public GameObject[] slot = new GameObject[25];
    protected item itemScr;
    protected item itemScrTemp;
    public GameManager gM;
    public GameObject ActivateButton;
    public GameObject SellButton;
    //public GameObject PlantButton;
    private PlantAndHarvest plantingScr;
    public GameObject plantGm;
    public GameObject DeactivateButton;
    public GameObject Sort;
    protected int num =0;
    protected int i = 0;
    // public int x= 0;
    public static bool isFull = false;
     public int Pun = 0;
    protected int Prazan = 0;
    public Redirect_Quest Redirect;
    protected bool closedTransfer = false;
    public CreateItem testMrkva;
    public GameObject panelInv;
    private int spaceInInventory;
    public AlertPanelScr alertPanelScr;
    [SerializeField]
    private descriptionPanelScr descriptionPanelScr;
    public TextMeshProUGUI starsCountText;
    public static int starCount = 0;
    [SerializeField]
    private List<CreateItem> allItems = new List<CreateItem>();

    // Start is called before the first frame update
    public  virtual void  Awake()
    {
        closedTransfer = false;
        Pun = 0;
        spaceInInventory = slot.Length;
        starsCountText.text = starCount.ToString();
    }

    public void checkSpaceInInventory(int amount)
    {
        Pun -= amount;
        isFull = false;
    }

  
   public void CloseInv()
    {
        gM.CloseInv();
    }

    public void ActivateDeleteFunc()
    {
      //  ActivateButton.SetActive(false);
        for (int b = 0; b < slot.Length ; b++)
        {
            itemScr = slot[b].GetComponentInChildren<item>();
            itemScr.CantUse = true;
            if (itemScr.haveItem && itemScr.Type != TypeOfItem.Quest && itemScr.Type != TypeOfItem.Star)
            {
                itemScr.DeleteButton.SetActive(true);
                itemScr.SellButton.SetActive(false);
                itemScr.PlantButton.SetActive(false);
            }
         
        }
       // DeactivateButton.SetActive(true);
    }

    public void ActivateSellFunc()
    {
        //SellButton.SetActive(false);
        Debug.Log("shopInventory.InShop : " + shopInventory.InShop);
        if(shopInventory.InShop== true) 
        {
            closedTransfer = true;
            for (int b = 0; b < slot.Length; b++)
            {
                itemScr = slot[b].GetComponentInChildren<item>();
                itemScr.CantUse = true;
                
                if (itemScr.haveItem && itemScr.Type != TypeOfItem.Quest && itemScr.Type != TypeOfItem.Star)
                {
                    itemScr.SellButton.SetActive(true);
                    itemScr.DeleteButton.SetActive(false);
                    itemScr.PlantButton.SetActive(false);
                }
               
            }
        }
        else
        {
           // DeactivateDeleteFunc();
            Debug.Log("you are not at the shop right now so you can't use that feature");
        }

        //DeactivateButton.SetActive(true);
    }

    public void DeactivateDeleteFunc()
    {
        
     //   DeactivateButton.SetActive(false);
        for (int b = 0; b < slot.Length; b++)
        {

            itemScr = slot[b].GetComponentInChildren<item>();
            itemScr.CantUse = false;
            itemScr.DeleteButton.SetActive(false);
            itemScr.SellButton.SetActive(false);
            itemScr.PlantButton.SetActive(false);
        }
        //ActivateButton.SetActive(true);
    }

    public void DeleteItemAfterQuestEnded(Create_Quest quest)
    {
        for (int i = 0; i < slot.Length; i++)
        {

            itemScr = slot[i].GetComponentInChildren<item>();

            if (itemScr.haveItem  && quest.Target == itemScr.name)
            {

                itemScr.Destroy();
                num--;

               Pun -= 1;
        
            }
           
            if (itemScr.haveItem == false)
            {
                continue;

            }
            else
            {

            }

        }
    }

    public void ItemForPlanting()
    { 
        if(plantGm!=null)
        {
            plantingScr = plantGm.GetComponent<PlantAndHarvest>();
            if (plantingScr.playerInRange)
            {
                for (int b = 0; b < slot.Length; b++)
                {
                    itemScr = slot[b].GetComponentInChildren<item>();
                    itemScr.CantUse = true;

                    if (itemScr.haveItem && itemScr.Type == TypeOfItem.Plantable)
                    {
                        itemScr.SellButton.SetActive(false);
                        itemScr.PlantButton.SetActive(true);
                        itemScr.DeleteButton.SetActive(false);
                    }

                }
               
            }
        }
    else
    {
    // DeactivateDeleteFunc();
    Debug.Log("you are not at the plant spot right now so you can't use that feature");
            itemScr.PlantButton.SetActive(false);
        }
    }


    public void openInvToPlant()
    {
        if (GameManager.InvOppened == false)
        {
            GameManager.InvOppened = true;
            panelInv.SetActive(true);
            ItemForPlanting();
           
        }
    }
    public void closeInvToPlant()
    {
        if (GameManager.InvOppened == true)
        {
            GameManager.InvOppened = false;
            panelInv.SetActive(false);
        }
    }
    public bool transferFromItem(CreateItem itm,string name)
    {
        if (plantGm != null)
        {
            plantingScr = plantGm.GetComponent<PlantAndHarvest>();
            if (plantingScr.playerInRange && plantingScr.currentState == PlantState.notPlanted)
            {
                plantingScr.Send(itm, name);
                DeactivateDeleteFunc();
                panelInv.SetActive(false);
                GameManager.InvOppened = false;
                Time.timeScale = 1;
                return true;
            }
        }
        return false;
    }

    public void SortByName()
    {
        /*
        char temp, temp2;
        for (int i = 0; i < Pun; i++)
        {
          

              for(int y=0;y<Pun-1;y++)
                        {
                itemScr = slot[y].GetComponentInChildren<item>();
                itemScrTemp = slot[y+1].GetComponentInChildren<item>();
                temp = itemScr.name[0];
                temp2 = itemScrTemp.name[0];
                Debug.Log(temp);

                if(temp>temp2)
                {
                    itemScr = itemScrTemp;
                    Debug.Log("Proslo");
                    Debug.Log(itemScr.name);

                }

              }
                                                                                    // TREBA FIXAT ZIVOT
               
           
        }
        */
    }
    public void wipeInvenory()
    {
        for (i = 0; i < slot.Length; i++)
        {
            itemScr = slot[i].GetComponentInChildren<item>();
            if (itemScr.haveItem)
            {
                Debug.Log("obrisao: " + itemScr.name);
                itemScr.Destroy();
            }
        }
        num = 0;
        Pun = 0;
    }

    public List<ItemObject> SaveInventory()
    {
        List<ItemObject> items = new List<ItemObject>();
        for ( i = 0; i < slot.Length; i++)
        {
            itemScr = slot[i].GetComponentInChildren<item>();
            if (itemScr.haveItem)
            {
                ItemObject itemObject = new ItemObject();
                itemObject.assign(itemScr.thisItem.ID, itemScr.counter1);
                items.Add(itemObject);
            }
        }
        return items;
    }
 public void checkInvContent() {
    for (i = 0; i < slot.Length; i++)
        {
            itemScr = slot[i].GetComponentInChildren<item>();
            if (itemScr.haveItem)
            {
                Debug.Log("pun na : " + i + 1 + " , " + itemScr.name);
            }
            else
            {
                Debug.Log("empty at place : " + i + 1);
            }
        }
 }

    public void LoadInventory(List<ItemObject> items)
    {
        starsCountText.text = starCount.ToString();
        wipeInvenory();
        foreach (ItemObject item1 in items)
        {
            for (i = 0; i < allItems.Count; i++)
            {
                //Debug.Log(item1.id + " , " + item1.count);
                    if (item1.id == allItems[i].ID)
                    {
                        for(int j=0;j< item1.count; j++)
                        {
                            AddItem(allItems[i], true);
                         }
                        break;
                    }
            }
        }
    }



    public void AddItem(CreateItem item,    bool loading )
        {
        if (item.Type == TypeOfItem.Star)
        {
            ++starCount;
            starsCountText.text = starCount.ToString();
            Redirect.Gathering(item.name, num, loading);
            if(starCount >= 10)
            {
                descriptionPanelScr.showDescriptionPanel();
              //  alertPanelScr.showAlertPanel("Game finished, demo completed!");
            }
            return;
        }

        if (isFull)
        {
            alertPanelScr.showAlertPanel("No space in inventory");
            return;
        }
        //Ako inv nije pun...

        for (int  i = 0; i < slot.Length; i++)
        {     
            itemScr = slot[i].GetComponentInChildren<item>();
            if (itemScr.haveItem && item.isStackable == false)
            {
                continue;
            }
            else if(itemScr.haveItem && item.isStackable && itemScr.counter1 < 5 && item.name == itemScr.name)
            {
                Debug.Log("gather");
                itemScr.counter1++;
                if (item.Type == TypeOfItem.Quest)
                {
                    Debug.Log("salje");
                    num++;
                    Redirect.Gathering(item.name, num, loading);
                }
                return;
            }
            else if (itemScr.haveItem == false)
            {
                itemScr.thisItem = item;
                itemScr.CurrentNum = i;
                itemScr.name = item.name;
                itemScr.description = item.description;
                itemScr.img = item.icon;
                itemScr.Type = item.Type;
                itemScr.haveItem = true;
                Pun++;
                if (item.Type == TypeOfItem.Quest)
                {
                    num++;
                    Redirect.Gathering(item.name,num, loading);
                }
                if (Pun == spaceInInventory)
                {
                    isFull = true;
                    Debug.Log("pun");
                }
                return ;
            }
        }
    }
}
