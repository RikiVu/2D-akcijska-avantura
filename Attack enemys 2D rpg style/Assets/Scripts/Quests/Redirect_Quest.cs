using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEngine.EventSystems.EventTrigger;

public class Redirect_Quest : MonoBehaviour
{
    public List<QuestObject> questObjects = new List<QuestObject>();
    private QuestObject questObjectTemp = new QuestObject();
    public GameObject parentGameObject;
    public GameObject panelPrefab;
    private float sizeOfContainerBottom = 0;
    private float sizeOfContainerBottomInital = 0;
    private RectTransform rectTransform;
    public GameManager gameManager;
    private void Awake()
    {
       rectTransform = parentGameObject.gameObject.GetComponent<RectTransform>();
       sizeOfContainerBottomInital = rectTransform.offsetMin.y;
    }

    //test 
    public void saveToManager()
    {
        foreach (QuestObject obj in questObjects) {
            gameManager.addInQuestList(obj.counter, obj.quest);
            Debug.Log("ovvvvvvvvvvo "+ obj.counter);
        }
     
    }
    public void loadQuests(QuestObjectLog obj)
    {
        if (obj.questTaken == true && obj.questEnded == false)
            AddQuest(obj.quest,obj.npcQuestScr, obj.count);
    }
    public void Killed(string name)
    {
        Debug.Log("killed " + name);
        questObjectTemp = questObjects.Find(p => p.quest.Target == name);
        if( questObjectTemp!=null)
        {
            if (questObjectTemp.quest.count > questObjectTemp.counter)
            {
                questObjectTemp.counter++;
                questObjectTemp.panelScr.progression.text = questObjectTemp.counter.ToString();
                if(questObjectTemp.counter >= questObjectTemp.quest.count )
                {
                    Debug.Log("finished quest");
                    questObjectTemp.quest.Finished = true;
                    questObjectTemp.Scr.NpcQuest.Finished = true;
                    questObjectTemp.completedQuest = true;
                    questObjectTemp.panelScr.QuestCompleted();
                }
            }
        }
                       // QuestCont[x].saveToManager();
                         // napravi da moze rjesit quest prije neg sto ga uzme.
    }

    public void Gathering(string name, int num)
    {
        Debug.Log("Got " + name);
        questObjectTemp = questObjects.Find(p => p.quest.Target == name);
        if (questObjectTemp != null)
        {
            if (questObjectTemp.quest.count > questObjectTemp.counter)
            {
                questObjectTemp.counter++;
                questObjectTemp.panelScr.progression.text = questObjectTemp.counter.ToString();
                if (questObjectTemp.counter >= questObjectTemp.quest.count)
                {
                    Debug.Log("finished quest");
                    questObjectTemp.quest.Finished = true;
                    questObjectTemp.Scr.NpcQuest.Finished = true;
                    questObjectTemp.completedQuest = true;
                    questObjectTemp.panelScr.QuestCompleted();
                }
            }
        }
            //QuestCont[x].saveToManager();
    }

    public void AddQuest(Create_Quest quest, NpcQuestScr npcScr, int currentCount)
    {
        QuestObject questObjectTemp1 = new QuestObject();
        Debug.Log("Dodaj quest: " + quest.name);
        GameObject panel = Instantiate(panelPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        panel.transform.SetParent(parentGameObject.transform); 
        panel.transform.localScale = new Vector3(0.75f, 0.75f, 1);
        questObjectTemp1.quest = quest;
        questObjectTemp1.Scr = npcScr;
        questObjectTemp1.completedQuest = false;
        questObjectTemp1.activeQuest = true;
        QuestController questController = panel.gameObject.GetComponent<QuestController>();
        questObjectTemp1.panelScr = questController;
        Debug.Log(currentCount);
        questObjectTemp1.counter = currentCount;
        questObjects.Add(questObjectTemp1);
        //panel texts
        panel.SetActive(true);
        questController.nameOfQuest.text = quest.name;
        questController.description.text = quest.description.ToString();
        questController.progression.text = questObjectTemp1.counter.ToString();
        questController.activeQuest = true;
        questController.progressionFull.text = quest.count.ToString();
        if (questObjects.Count > 5)
        {
            sizeOfContainerBottom = rectTransform.offsetMin.y - 70f;
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, sizeOfContainerBottom);
        }
        if (questObjectTemp1.counter >= questObjectTemp1.quest.count)
        {
            Debug.Log("finished quest");
            questObjectTemp1.quest.Finished = true;
            questObjectTemp1.Scr.NpcQuest.Finished = true;
            questObjectTemp1.completedQuest = true;
            questObjectTemp1.panelScr.QuestCompleted();
        }
    }

    public void DeleteQuest(string name)
    {
       Debug.Log("deleting quest: " + name);
       questObjectTemp = questObjects.Find(p => p.quest.name == name);
        if (questObjectTemp != null)
        {
            if (questObjectTemp.completedQuest)
            {
                Destroy(questObjectTemp.panelScr.gameObject);
                questObjects.Remove(questObjectTemp);
                if (questObjects.Count > 5)
                {
                    sizeOfContainerBottom = rectTransform.offsetMin.y + 70f;
                    rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, sizeOfContainerBottom);
                }
            }
        }
    }
    public void DeleteAll()
    {
        foreach (QuestObject qObject in questObjects)
        {
            Destroy(qObject.panelScr.gameObject);
           
        }
        questObjects.Clear();
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, sizeOfContainerBottomInital);

    }
}
