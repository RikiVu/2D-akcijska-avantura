    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


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
    bool closedTransfer = true;

    public CreateItem testMrkva;
    public GameObject panelInv;


    private int spaceInInventory;
    public AlertPanelScr alertPanelScr;

    // Start is called before the first frame update
    public  virtual void  Awake()
    {
        Pun = 0;
        spaceInInventory = slot.Length;
    }

    private void Update()
    {
        if (shopInventory.InShop == false && closedTransfer)
            CloseShop();
    }

    public void checkSpaceInInventory(int amount)
    {
        Pun -= amount;
        isFull = false;
    }

    void  CloseShop()
    {
        DeactivateDeleteFunc();
        itemScr.priceGM.SetActive(false);
        closedTransfer = false;
        Debug.Log("closed");
        


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
            if (itemScr.haveItem && itemScr.Type != TypeOfItem.Quest)
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
        if(shopInventory.InShop== true)
        {
            closedTransfer = true;
            for (int b = 0; b < slot.Length; b++)
            {
                itemScr = slot[b].GetComponentInChildren<item>();
                itemScr.CantUse = true;
                
                if (itemScr.haveItem && itemScr.Type != TypeOfItem.Quest)
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
            Time.timeScale = 0;
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

        public void AddItem(CreateItem item)
        {
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
                
                itemScr.counter1++;
                if (item.Type == TypeOfItem.Quest)
                {
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
