using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 spawnPosition;
    public bool godMode;
    public float currentHealth=4;
    public float gold=0;
    public int arrows=0;
    public int stars =0;
    public int strenght = 0;
    public int dexterity = 0;
    public int constitution = 0;
    public List<CreateItem> items = new List<CreateItem>();
    public List<CreateItem> equipment = new List<CreateItem>();
    public Vector2 camMaxPosition;
    public Vector2 camMinPosition;
    public List<ChestObject> chests = new List<ChestObject>();

}
