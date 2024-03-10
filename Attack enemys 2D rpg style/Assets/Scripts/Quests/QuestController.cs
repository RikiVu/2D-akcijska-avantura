using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public bool completedQuest;
    
    public bool activeQuest = false;
    bool active = false;
    public int counter;
    public int progressionFullCounter;
    public string Target;
    public string CurrentKilledEnemy;
    

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
