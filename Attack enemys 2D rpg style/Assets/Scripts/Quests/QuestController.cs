using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Sprites;

public class QuestController : MonoBehaviour
{
    public TypeOfQuest Type;
    public TextMeshProUGUI nameOfQuest;
    public TextMeshProUGUI description;
    public TextMeshProUGUI progression;
    public TextMeshProUGUI progressionFull;
    public TextMeshProUGUI completedText;
    public GameObject currentQuestGiver;
    private NpcQuestScr Scr;
    public Create_Quest quest;

    public bool completedQuest;
    
    public bool activeQuest = false;
    bool active = false;
    public int counter = 0;
    public int progressionFullCounter;
    public string Target;
    public string CurrentKilledEnemy;

    public GameManager gameManager;
    [SerializeField]
    private int assignedId;

    private void Start()
    {
        assignedId = gameManager.addInQuestList(this, counter, quest);
    }


    public void saveToManager()
    {
        gameManager.addInQuestList(assignedId,counter);
    }

    public void loadItem(int count, bool activeQuest, Create_Quest quest)
    {
        if (count != null || activeQuest != null)
        {
            if (!activeQuest)
            {
                QuestDeletion();
            }
            else
            {
                counter = count;
                progression.text = counter.ToString();
                progressionFullCounter = quest.count;
                nameOfQuest.text = quest.name;
                Type = quest.Type;
                description.text = quest.description.ToString();
                completedQuest = quest.Finished;
                //currentQuestGiver = gameobjectName;
                quest = quest;
                this.gameObject.SetActive(true);
            }
    
           
        }
    }

    private void Awake()
    {
        if (!activeQuest)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            progression.text = counter.ToString();
            progressionFull.text = progressionFullCounter.ToString();
        }
    }

    public void QuestCompleted()
    {
        Image img = this.GetComponent<Image>();
        img.color = UnityEngine.Color.green;
        completedText.gameObject.SetActive(true);
        
        completedQuest = true;
        Scr = currentQuestGiver.GetComponent<NpcQuestScr>();
        Scr.NpcQuest.Finished = true;
        if (Scr.NpcItem)
            Scr.NpcItem.pickable = false;

        Debug.Log("completed");
        // 
    }
    public void QuestDeletion()
    {
        Image img = this.GetComponent<Image>();
        img.color = UnityEngine.Color.white;
        completedText.gameObject.SetActive(false);
        Debug.Log("obrisan quest");
        completedQuest = false;
        progression.text = "0";
        counter = 0;
        Target = "Null";
        activeQuest = false;
        if (!activeQuest)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void onClick()
    {
        if(active == false)
        {
            nameOfQuest.gameObject.SetActive(false);
            description.gameObject.SetActive(true);
            active = true;
        }
        else
        {
            description.gameObject.SetActive(false);
            nameOfQuest.gameObject.SetActive(true);          
            active = false;
        }
    }
}
