using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class Equipment : MonoBehaviour
{
    public GameObject[] slot = new GameObject[6];
    
    private item itemScr;

    public ArmorManager armorManager;
    public Inventory Inventory;
    public int Pun = 0;
    protected int Prazan = 0;
    public int x = 0;
    //private float i = 0;
    protected Redirect_Quest Redirect;
    private GameObject QuestPanel;
    public GameManager GameManager;
    [SerializeField]
    private PlayerScr playerScr;

    private void Start()
    {
        QuestPanel = GameObject.FindGameObjectWithTag("QuestPanel");
        Redirect = QuestPanel.GetComponent<Redirect_Quest>();
    }
    private void ItemLogic(item itemScr, CreateItem item)
    {
        itemScr.equiped = true;
        itemScr.thisItem = item;
        itemScr.name = item.name;
        itemScr.description = item.description;
        itemScr.img = item.icon;
        itemScr.Type = item.Type;
        itemScr.haveItem = true;
        itemScr.DeleteButton.SetActive(true);
        CoinScript.bonusPerc = item.MonsterGoldBonus;
        GameManager.updateStats(item.StrenghtBoost, item.DexterityBoost, item.ConstitutionBoost);
     
    }
    private void conditionFunc(item itemScr, CreateItem item)
    {
        if (itemScr.haveItem == false)
        {
            ItemLogic(itemScr, item);
            if (item.nullify)
            {
                armorManager.hasProtection = true;
                armorManager.cooldown = false;
          
                armorManager.armorContainers = 1;
                armorManager.InitArmor();
               
            }
            Inventory.checkSpaceInInventory(1);
        }
            
        else
        {
            ItemLogicUnequip(itemScr.thisItem);
            Inventory.checkSpaceInInventory(1);
            Inventory.AddItem(itemScr.thisItem);
            ItemLogic(itemScr, item);
            Debug.Log("Swap Equipo");
            if (item.nullify)
            {
                armorManager.hasProtection = true;
                armorManager.cooldown = false;
            
                armorManager.armorContainers = 1;
                armorManager.InitArmor(); 
          
            }
        }
    }
    private void ItemLogicUnequip(CreateItem item)
    {
        if(item.TypeOfEquipment == TypeOfEquipment.Weapon)
        {
            playerScr.changeHaveSwordState(false);
        }
        if (item.nullify)
        {
            armorManager.hasProtection = false;
            armorManager.cooldown = false;
     
            armorManager.armorContainers = 0;
            armorManager.InitArmor();
         
        }
        GameManager.updateStats(-item.StrenghtBoost, -item.DexterityBoost, -item.ConstitutionBoost);
    }
    public void UnequipItem(item item)
    {
        ItemLogicUnequip(item.thisItem);
        item.DeleteButton.SetActive(false);
        Inventory.AddItem(item.thisItem);
    }

    public void AddItem(CreateItem item)
    {
            switch (item.TypeOfEquipment)
            {
                case TypeOfEquipment.None:
                    break;
                case TypeOfEquipment.Weapon:
                    itemScr = slot[5].GetComponentInChildren<item>();
                    conditionFunc(itemScr, item);
                    playerScr.changeHaveSwordState(true);
                    AllowPassage.CanPass = true;
                    break;
                case TypeOfEquipment.Helmet:
                    itemScr = slot[0].GetComponentInChildren<item>();
                    conditionFunc(itemScr, item);
                    break;
                case TypeOfEquipment.Chest:
                    itemScr = slot[1].GetComponentInChildren<item>();
                    conditionFunc(itemScr, item);
                    break;
                case TypeOfEquipment.Boots:
                    itemScr = slot[2].GetComponentInChildren<item>();
                    conditionFunc(itemScr, item);
                    break;
                case TypeOfEquipment.Ring:
                    itemScr = slot[3].GetComponentInChildren<item>();
                    conditionFunc(itemScr, item);
                    break;
                case TypeOfEquipment.Bow:
                    itemScr = slot[4].GetComponentInChildren<item>();
                    if (itemScr.haveItem == false)
                    {
                        PlayerScr.haveBow = true;
                        ItemLogic(itemScr, item);
                    }           
                    else
                    {
                        Inventory.AddItem(itemScr.thisItem);
                        ItemLogic(itemScr, item);
                    }
                    break;
                default:
                    Debug.Log("NO");
                    break;
            }
    }

    public void WipeEquipment()
    {
        foreach (GameObject gm in slot)
        {
            itemScr = gm.GetComponentInChildren<item>();
            if (itemScr.haveItem)
            {
                itemScr.UnEquipForLoad();
                ItemLogicUnequip(itemScr.thisItem);
                itemScr.DeleteButton.SetActive(false);
            }
        }
    }
    public List<CreateItem> SaveEquipment()
    {
        List<CreateItem> items = new List<CreateItem>();
        foreach (GameObject gm in slot)
        {
            itemScr = gm.GetComponentInChildren<item>();
            if (itemScr.haveItem)
            {
                items.Add(itemScr.thisItem);
            }
        }
        return items;
    }


    public void LoadEquipment(List<CreateItem> items)
    {
        WipeEquipment();
        foreach (CreateItem item1 in items)
        {
            if (item1 != null)
                AddItem(item1);
        }
    }

}

