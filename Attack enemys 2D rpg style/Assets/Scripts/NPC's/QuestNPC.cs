using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : MonoBehaviour
{
      public Create_Quest NpcQuest;
    public CreateItem NpcItem;
    public bool questTaken = false;
    public bool questEnded = false;
    public Redirect_Quest Redirect;
    public string name;
    public GameObject DialogBox;
    public DialogScr dialogScr;
    public Inventory invScr;
    public string Talk;



     void Awake()
    {
        if (NpcItem)
        {
          NpcItem.pickable = false;
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        dialogScr = DialogBox.GetComponent<DialogScr>();
    }

    // Update is called once per frame
    public void Decide(bool decide)
    {
        if (decide)
        {
            questTaken = true;
           Redirect.AddQuest(NpcQuest, this.gameObject);
            if (NpcItem)
            {
                NpcItem.pickable = true;
            }

            //prihvacen quest
            DialogBox.SetActive(false);

        }
        else if (!decide)
        {
            // questTaken = false;
            
            DialogBox.SetActive(false);
        }
    }


    public void QuestConversation()
    {
        if (!questTaken)
        {
            
            dialogScr.currentQuestGiver = this.gameObject;
            dialogScr.placeHolder = this.NpcQuest.description.ToString();
           
            //sad bih trebo izbacit buttone hoces li prihvatit poso
            dialogScr.acceptButton.SetActive(true);
            dialogScr.rejectButton.SetActive(true);

        }


        else if (questTaken && !NpcQuest.Finished && questEnded == false)
        {
            dialogScr.placeHolder = this.NpcQuest.alreadyTakenQuest.ToString();
            //neki razgovor ne bitan   
        }

        else if (NpcQuest.AtNpcPlace && this.NpcQuest.Finished)
        {
            dialogScr.placeHolder = NpcQuest.completedQuest.ToString();
            PlayerScr.Gold += NpcQuest.money;
            //LevelSystem.value1 += NpcQuest.XPgain;
            questEnded = true;
            NpcQuest.Finished = false;
            Redirect.DeleteQuest(NpcQuest.name);
            if (NpcQuest.Type == TypeOfQuest.Gathering)
            {
                invScr.DeleteItemAfterQuestEnded(NpcQuest);
                //mora sve popravit treba
            }
           
            //neki razgovor ne bitan   
        }
        else if (questEnded)
        {
            dialogScr.placeHolder = Talk.ToString();
        }




    }
}



