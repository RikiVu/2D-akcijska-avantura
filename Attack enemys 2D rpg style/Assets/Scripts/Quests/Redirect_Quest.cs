using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Redirect_Quest : MonoBehaviour
{
    //private QuestController QuestCont;
    public TypeOfQuest Type;
    public QuestController[] QuestCont = new QuestController[5];
    public int x = 0;
    

    
    // Start is called before the first frame update
 
    public void Killed(string name)
    {
        Debug.Log("killed " + name);
        x = 0;
        for (x = 0; x < QuestCont.Length; x++)
        {
            if (QuestCont[x].activeQuest && QuestCont[x].Type == TypeOfQuest.Kill)
            {
                
                if (QuestCont[x].Target == name )
                {
                   if(QuestCont[x].counter != QuestCont[x].progressionFullCounter)
                    {
                        QuestCont[x].counter++;
                        QuestCont[x].progression.text = QuestCont[x].counter.ToString();
                        Debug.Log(QuestCont[x].counter.ToString());
                        Debug.Log(QuestCont[x].progressionFullCounter);
                    }
                    if (QuestCont[x].counter == QuestCont[x].progressionFullCounter)
                        QuestCont[x].QuestCompleted();
                }
            }
            else
            {
                                                            // napravi da moze rjesit quest prije neg sto ga uzme.
            }
        }
    }
    public void Gathering(string name, int num)
    {
        x = 0;
        for (x = 0; x < QuestCont.Length; x++)
        {
           
            if (QuestCont[x].activeQuest && QuestCont[x].Type == TypeOfQuest.Gathering)
            {
                Debug.Log(name);
                Debug.Log(QuestCont[x].Target);

                if (QuestCont[x].Target == name)
                {
                    Debug.Log("gath");
                   
                    if (QuestCont[x].counter != QuestCont[x].progressionFullCounter)
                        QuestCont[x].counter = num;

                    Debug.Log(QuestCont[x].counter.ToString());
                    Debug.Log(QuestCont[x].progressionFullCounter);
                    QuestCont[x].progression.text = QuestCont[x].counter.ToString();

                    if (QuestCont[x].counter == QuestCont[x].progressionFullCounter)
                        QuestCont[x].QuestCompleted();

          
                }
               
            }
        }
    }

    public void DeleteQuest(string name)
    {
        Debug.Log("deleting quest: " + name);
        int Pun = 0;
        int Prazan = 0;
        for (x = 0; x < QuestCont.Length; x++)
        {
            if (QuestCont[x].activeQuest)
                Pun++;
            else
                Prazan++;
        }
        for (int i = 0; i < QuestCont.Length; i++)
        {
            Debug.Log("i: " + i + " QuestCont.Length: " + QuestCont.Length);
            //Debug.Log("QuestCont[i].completedQuest: " + QuestCont[i].completedQuest + " QuestCont[i].nameOfQuest: " + QuestCont[i].nameOfQuest);

            if (QuestCont[i].completedQuest && QuestCont[i].nameOfQuest.text == name)
            {
                Debug.Log("?3");
                QuestCont[i].gameObject.SetActive(false);
                QuestCont[i].activeQuest = false;
                QuestCont[i].QuestDeletion();
                x--;
                return;
            }
            if (QuestCont[i].activeQuest == false)
                continue;
        }
    }
    public void AddQuest(Create_Quest quest, GameObject gameobjectName)
    {
        Debug.Log("Dodaj quest: "+ quest.name + " Nes: " +  gameobjectName.name);
        int  Pun = 0;
        int Prazan = 0;
        for ( x = 0; x < QuestCont.Length; x++)
        {
            if (QuestCont[x].activeQuest)
                Pun++;

            else
                Prazan++;
        }
        

        for (int i = 0; i < QuestCont.Length; i++)
        {
            if (QuestCont[i].activeQuest)
                continue;
            if (QuestCont[i].activeQuest == false)
            {
                QuestCont[i].gameObject.SetActive(true);
                QuestCont[i].progressionFullCounter = quest.count;
                QuestCont[i].nameOfQuest.text = quest.name;
                QuestCont[i].Type = quest.Type;
                QuestCont[i].description.text = quest.description.ToString();
                QuestCont[i].completedQuest = quest.Finished;
                QuestCont[i].currentQuestGiver = gameobjectName;
                if (QuestCont[i].Type == TypeOfQuest.Kill)
                {
                    QuestCont[i].Target = quest.Target;
                    QuestCont[i].counter = 0;
                }
                else if(QuestCont[i].Type == TypeOfQuest.Gathering)
                    QuestCont[i].Target = quest.Target;

                QuestCont[i].activeQuest = true;
                x++;
                return;
            }
        }
    }


}
