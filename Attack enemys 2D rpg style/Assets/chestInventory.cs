using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestInventory : MonoBehaviour
{
  public GameObject slot;
    public Inventory Inventory;
    private item itemScr;
    public bool Collected = false;

    private int maxSize = 10;


    private void Awake()
    {
        slot.SetActive(true);
    }
    public void AddItem1(CreateItem item)
     {   
        itemScr = slot.GetComponentInChildren<item>();
        // itemScr.PlayerInv = false;
        itemScr.thisItem = item;
        itemScr.name = item.name;
        itemScr.description = item.description;
        itemScr.img = item.icon;
        itemScr.Type = item.Type;
        itemScr.haveItem = true;
    }
 
    public void sendToInv()
    {
            itemScr.ChestSend(itemScr.thisItem);
          
    }
 
}

