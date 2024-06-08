using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestObjectLog 
{
    public int id;
    //public NpcQuestScr npcQuestScr;
    public bool questTaken = false;
    public bool questEnded = false;
    public Create_Quest quest;
    //public string name = "";
    public int count = 0;
}
