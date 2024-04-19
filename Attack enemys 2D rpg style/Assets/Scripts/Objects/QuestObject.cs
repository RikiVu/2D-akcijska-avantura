using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestObject 
{
    public Create_Quest quest;
    public NpcQuestScr Scr;
    public bool completedQuest =false;
    public bool activeQuest = false;
    public int counter = 0;
    //public int progressionFullCounter;
    //public string Target ="";
    public string CurrentKilledEnemy ="";
    public QuestController panelScr;
}
