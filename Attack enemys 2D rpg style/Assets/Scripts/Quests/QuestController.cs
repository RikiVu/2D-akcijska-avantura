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
    private QuestNPC Scr;

    public bool completedQuest;
    
    public bool activeQuest = false;
    bool active = false;
    public int counter;
    public string Target;
    public string CurrentKilledEnemy;
    

    private void Awake()
    {
        completedQuest=false;
        if (!activeQuest)
            this.gameObject.SetActive(false);
    }

    public void QuestCompleted()
    {
        Image img = this.GetComponent<Image>();
        img.color = UnityEngine.Color.green;
        completedText.gameObject.SetActive(true);
        
        completedQuest = true;
        Scr = currentQuestGiver.GetComponent<QuestNPC>();
        Scr.NpcQuest.Finished = true;
        if (Scr.NpcItem)
            Scr.NpcItem.pickable = false;

        // 
    }
    public void QuestDeletion()
    {
        Image img = this.GetComponent<Image>();
        img.color = UnityEngine.Color.white;
        completedText.gameObject.SetActive(false);

        completedQuest = false;
    }

    private void Update()
    {
       progression.text = counter.ToString();
        
       
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
