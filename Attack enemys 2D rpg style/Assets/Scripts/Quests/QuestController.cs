using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Sprites;

public class QuestController : MonoBehaviour
{
    public TextMeshProUGUI nameOfQuest;
    public TextMeshProUGUI description;
    public TextMeshProUGUI progression;
    public TextMeshProUGUI progressionFull;
    public TextMeshProUGUI completedText;
    //private NpcQuestScr Scr;
    //public Create_Quest quest;

    //public bool completedQuest;
    
    public bool activeQuest = false;
    bool active = false;
    /*
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
              // QuestDeletion();
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
*/
    private void Awake()
    {
        if (!activeQuest)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
          //  progression.text = "0";
          //  progressionFull.text = "0";
        }
    }

    public void QuestCompleted()
    {
        Image img = this.GetComponent<Image>();
        img.color = UnityEngine.Color.green;
        completedText.gameObject.SetActive(true);
        //completedQuest = true;
        /*
         if (Scr.NpcItem)
             Scr.NpcItem.pickable = false;
        */
    }


    //ne diraj

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
