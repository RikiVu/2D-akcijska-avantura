using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirect_Quest : MonoBehaviour
{
    private QuestController QuestCont;
    public TypeOfQuest Type;
    public GameObject objekt;
    public GameObject[] slot = new GameObject[5];
    public int x = 0;
    

    
    // Start is called before the first frame update
 
    public void Killed(string name)
    {
        Debug.Log("killed " + name);
        x = 0;
        for (x = 0; x < slot.Length; x++)
        {
            QuestCont = slot[x].GetComponent<QuestController>();
            if (QuestCont.activeQuest)
            {
                if (QuestCont.Target == name )
                {
                   if(QuestCont.counter.ToString() != QuestCont.progressionFull.text)
                    QuestCont.counter++;
                }
                if (QuestCont.counter.ToString() == QuestCont.progressionFull.text)
                        QuestCont.QuestCompleted();
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
        for (x = 0; x < slot.Length; x++)
        {
            QuestCont = slot[x].GetComponent<QuestController>();
            if (QuestCont.activeQuest)
            {
                if (QuestCont.Target == name)
                {
                    if (QuestCont.counter.ToString() != QuestCont.progressionFull.text)
                         QuestCont.counter = num;
                }
                if (QuestCont.counter.ToString() == QuestCont.progressionFull.text)
                    QuestCont.QuestCompleted();
            }
            else if(QuestCont.Type == TypeOfQuest.Gathering)
            {
                if (QuestCont.Target == name)
                {
                    if (QuestCont.counter.ToString() != QuestCont.progressionFull.text)
                        QuestCont.counter = num;
                }
            }
        }
    }
    public void FirstQuest(string name,bool canPass)
    {
        if (canPass)
        {
         
            x = 0;
            for (x = 0; x < slot.Length; x++)
            {
                QuestCont = slot[x].GetComponent<QuestController>();
                if (QuestCont.activeQuest)
                {
                    Debug.Log("Proso");
                    if (QuestCont.Type == TypeOfQuest.First)
                    {
                            QuestCont.QuestCompleted();

                    }
                }
                else
                {
                    // napravi da moze rjesit quest prije neg sto ga uzme.

                }
            }
        }
    }
    public void DeleteQuest(string name)
    {
        Debug.Log("deleting quest: " + name);
        int Pun = 0;
        int Prazan = 0;
        for (x = 0; x < slot.Length; x++)
        {
            QuestCont = slot[x].GetComponent<QuestController>();
            if (QuestCont.activeQuest)
                Pun++;
            else
                Prazan++;
        }
     

        for (int i = 0; i < slot.Length; i++)
        {
            QuestCont = slot[i].GetComponent<QuestController>();
            if (QuestCont.completedQuest && QuestCont.nameOfQuest.text == name)
            {
                QuestCont.gameObject.SetActive(false);
                QuestCont.activeQuest = false;
                QuestCont.QuestDeletion();
                x--;
                return;
            }
            if (QuestCont.activeQuest == false)
                continue;
        }
    }
    public void AddQuest(Create_Quest quest, GameObject gameobjectName)
    {
        int  Pun = 0;
        int Prazan = 0;
        for ( x = 0; x < slot.Length; x++)
        {
            QuestCont = slot[x].GetComponent<QuestController>();
            if (QuestCont.activeQuest)
                Pun++;

            else
                Prazan++;
        }
        

        for (int i = 0; i < slot.Length; i++)
        {
            QuestCont = slot[i].GetComponent<QuestController>();
            if (QuestCont.activeQuest)
                continue;
            if (QuestCont.activeQuest == false)
            {
                QuestCont.gameObject.SetActive(true);
                QuestCont.progressionFull.text = quest.count.ToString();
                QuestCont.nameOfQuest.text = quest.name;
                QuestCont.Type = quest.Type;
                QuestCont.description.text = quest.description.ToString();
                QuestCont.completedQuest = quest.Finished;
                QuestCont.currentQuestGiver = gameobjectName;
                if (QuestCont.Type == TypeOfQuest.Kill)
                {
                    QuestCont.Target = quest.Target;
                    QuestCont.counter = 0;
                }
                else if(QuestCont.Type == TypeOfQuest.Gathering)
                    QuestCont.Target = quest.Target;
               
                QuestCont.activeQuest = true;
                x++;
                return;
            }
        }
    }


}
