using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item" )]


public class CreateItem : ScriptableObject
{
    public TypeOfItem Type;
    public TypeOfEquipment TypeOfEquipment;
    public int ID = 0;
    new public string name = "New item";
    new public string description = "Description";
    new public bool pickable;
    new public bool isStackable;
    new public int Price;
    new public int damageBoost;
    new public int HealthBoost;
    public Sprite icon = null;
    public bool isDefaultItem = false;
    

    
        //Debug.Log("Using " + name);
    
    public void RemoveFromInventory()
    {
      //  inventory.instance.Remove(this);
    }
   

   
}


public enum TypeOfItem
{
    HealingPotion,
    Equipment,
    Quest,
    Arrows,
    Plantable
     
}
public enum TypeOfEquipment
{
    None,
    Weapon,
    Helmet,
    Chest,
    Trousers,
    Ring,
    Bow

}
