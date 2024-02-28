using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : NPC01
{
    public CreateItem[] itemsInStock = new CreateItem[5];
    public shopInventory ShopInvScr;
    public static int id;
    private int CounterOfItems;
    
    // public static bool CanRemove = false;
    // Start is called before the first frame update
    private void Start()
    {
        CounterOfItems = itemsInStock.Length;
    }


    public override void Interact()
    {
       
        //  base.Interact();
        if (inRangeforTalk == true)

            if (Input.GetKey(KeyCode.E) && !talking)
            {
                
                if (!haveQuest == true)
                {
                    talking = true;
                    dialogScr.currentQuestGiver = this.gameObject;
                    DialogBox.SetActive(true);
                    dialogScr.Shop = true;
                    dialogScr.placeHolder = Talk.ToString();
                    dialogScr.acceptButton.SetActive(true);
                    dialogScr.rejectButton.SetActive(true);
                    ShopInvScr.DeleteItems();
                    for (int i = 0; i < CounterOfItems; i++)
                    {
                       // if(itemsInStock[i] != null)
                        ShopInvScr.AddItem1(itemsInStock[i],i);
                    }

                }

            }
         



    }
    void DeleteItem()
    {
        Debug.Log("removed");
        itemsInStock[id] = null;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(CanRemove)
        {
            DeleteItem();
            CanRemove = false;
                
        }
    */


        Interact();
    }
    

}
