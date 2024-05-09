    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using static UnityEditor.Progress;

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
     public int x= 0;
    public static bool isFull = false;
     public int Pun = 0;
    protected int Prazan = 0;
    public Redirect_Quest Redirect;
    protected bool closedTransfer = false;

    public CreateItem testMrkva;
    public GameObject panelInv;


    private int spaceInInventory;
    public AlertPanelScr alertPanelScr;
    public TextMeshProUGUI starsCountText;

    public static int starCount = 0;

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
                x++;
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
        for (int i = 0; i < slot.Length; i++)
        {
            itemScr = slot[i].GetComponentInChildren<item>();
            if (itemScr.haveItem)
            {
                itemScr.Destroy();
                num--;
                Pun -= 1;
                x++;
            }
        }
    }
    public List<CreateItem> SaveInventory()
    {
        List<CreateItem> items = new List<CreateItem>();
        for (int i = 0; i < slot.Length; i++)
        {
            itemScr = slot[i].GetComponentInChildren<item>();
            if (itemScr.haveItem)
                items.Add(itemScr.thisItem);
            continue;
        }
      
        return items;
    }

    public void LoadInventory(List<CreateItem> items)
    {
        starsCountText.text = starCount.ToString();
        wipeInvenory();
        foreach (CreateItem item1 in items)
        {
            if(item1 != null)
                AddItem(item1);
        }
    }



    public void AddItem(CreateItem item)
        {
        if (item.Type == TypeOfItem.Star)
        {
            ++starCount;
            starsCountText.text = starCount.ToString();
            return;
        }

        if (isFull)
        {
            Debug.Log("ovdi?");
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
                    Redirect.Gathering(item.name, num);
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
                x++;
                if (item.Type == TypeOfItem.Quest)
                {
                    num++;
                    Redirect.Gathering(item.name,num);
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
