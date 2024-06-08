using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemObject
{
    public int id;
    public int count;

    public void assign(int newId, int newCount)
    {
        id = newId;
        count = newCount;
    }

      
}

/*
    public void LoadInventory(List<ItemObject> items)
    {
        starsCountText.text = starCount.ToString();
        wipeInvenory();
        for (int i = 0; i < allItems.Count; i++)
        {
            foreach (ItemObject item1 in items)
            {
                if (item1 != null)
                {
                    if (item1.getId() == allItems[i].ID)
                    {
                        AddItem(allItems[i]);
                    }
                }
            }
        }
    }
*/