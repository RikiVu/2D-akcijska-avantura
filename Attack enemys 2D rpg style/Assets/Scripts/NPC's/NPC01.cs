using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC01 : MonoBehaviour
{

   
    // public GameObject dialogBox;
    public bool inRangeforTalk;
    public GameObject DialogBox;
    public DialogScr dialogScr;
    public bool talking = false;
    public string Talk;
   public GameObject ContextClue;
    public bool haveQuest;
    public QuestNPC quest;

    // Start is called before the first frame update
    void Start()
    {
        dialogScr = DialogBox.GetComponent<DialogScr>();
       
        //movespots.Length; 
       
        inRangeforTalk = false;

    }
//sf

     public  void Update()
    {


        Interact();


    }

    public virtual void Interact()
    {



        if (inRangeforTalk == true)

            if (Input.GetKeyDown(KeyCode.E)) 
            {
             //   PlayerScr.speed = 0;
                
                if(haveQuest == true)
                quest.NpcQuest.AtNpcPlace = true;

                DialogBox.SetActive(true);
                dialogScr.Shop = false;
                dialogScr.acceptButton.SetActive(false);
                dialogScr.rejectButton.SetActive(false);

                if (haveQuest == true)
                    quest.QuestConversation();
               else if (!haveQuest)
                {
                    dialogScr.TmProText.text = "";
                    dialogScr.placeHolder = "";
                    dialogScr.placeHolder = Talk.ToString();
                    dialogScr.Shop = false;
                }
               


                // otvori buttone i imas izbor da uzmes ga
                //kad stisne yes onda bih trebalo 
                //javit npc u da moze upisat u quest log..




            }
    }
   
  
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(haveQuest)
        {
            if (!quest.questTaken && !quest.questEnded)
                this.ContextClue.SetActive(true);
        }
       

        inRangeforTalk = true;
        

    }
    public virtual void OnTriggerExit2D(Collider2D collision)
    {
     dialogScr.currentQuestGiver = null;
        dialogScr.RejectQuest();
        inRangeforTalk = false;
        dialogScr.radi = false;
        dialogScr.TmProText.text = "";
        DialogBox.SetActive(false);
        if (haveQuest)
            quest.NpcQuest.AtNpcPlace = false;
        ContextClue.SetActive(false);
        talking = false;
    }

}
