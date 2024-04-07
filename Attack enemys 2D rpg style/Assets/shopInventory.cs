using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopInventory : Inventory
{

    public GameObject ParentGM;
    public Inventory Inventory;
    public static bool InShop = false;

    // Start is called before the first frame update

    public override void Awake()
    {
        Pun = 0;
    }


    public void ClosePanel()
    {
        PlayerScr.playerCanMove = true;
        //Time.timeScale = 1;
        DeleteItems();
        ParentGM.SetActive(false);
        InShop = false;
        
    }

    public void DeleteItems()
    {
        Pun = 0;
        Prazan = 0;
        for (x = 0; x < slot.Length; x++)
        {

            itemScr = slot[x].GetComponentInChildren<item>();
            if (itemScr.haveItem)
            {
                Pun++;

            }
            else
            {
                Prazan++;
            }
        }

        for (int i = 0; i < slot.Length; i++)
        {


            itemScr = slot[i].GetComponentInChildren<item>();
            if (itemScr.haveItem)
            {

                itemScr.Destroy();
                num--;

                x++;
            }
            if (itemScr.haveItem == false)
            {
                continue;

            }

        }
    }

    private void Update()
    {
        if(this.gameObject.activeInHierarchy)
        {
            InShop = true; 
        }
    }


    public  void AddItem1(CreateItem item, int id)
    {
        
        Pun = 0;
        Prazan = 0;
        for (x = 0; x < slot.Length; x++)
        {

            itemScr = slot[x].GetComponentInChildren<item>();
            itemScr.PlayerInv = false;
            if (itemScr.haveItem)
            {
                Pun++;
            }
            else
            {
                Prazan++;
            }
        }

        for (int i = 0; i < slot.Length; i++)
        {

            itemScr = slot[i].GetComponentInChildren<item>();
         
            if (itemScr.haveItem && item.isStackable == false)
            {
              
                continue;
            }
            else if (itemScr.haveItem && item.isStackable && itemScr.counter1 < 5 && item.name == itemScr.name)
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
                itemScr.id = id;
                itemScr.CurrentNum = i;
                itemScr.name = item.name;
                itemScr.description = item.description;
                itemScr.img = item.icon;
                itemScr.Type = item.Type;
                itemScr.haveItem = true;
                x++;
                if (item.Type == TypeOfItem.Quest)
                {
                    num++;
                    Redirect.Gathering(item.name, num);

                }

                return;
            }



        }



    }

}
