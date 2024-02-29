using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NpcQuestScr : NpcScr
{
    [Header("Quest section")]
    public Create_Quest NpcQuest;
    public CreateItem NpcItem;
    [SerializeField]
    protected bool questTaken = false;
    [SerializeField]
    protected bool questEnded = false;
    protected Redirect_Quest redirectScr;
    public Inventory invScr;
    [SerializeField]
    protected GameObject emoteGameObject;

    protected override void Start()
    {
        base.Start();
        redirectScr = GameObject.FindGameObjectWithTag("QuestPanel").GetComponent<Redirect_Quest>();
    }

    void Awake()
    {
        NpcQuest.Finished = false;
        if (NpcItem)
            NpcItem.pickable = false;
    }

    public override void Interact()
    {
        if (playerInRange == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogBoxScr.showDialog(dialogTekst);
                QuestConversation();
            }
        }
          
        
    }

    public void Decide(bool decide)
    {
        if (decide)  //prihvacen quest
        {
            questTaken = true;
            redirectScr.AddQuest(NpcQuest, this.gameObject);
            if (NpcItem)
                NpcItem.pickable = true;
           
            dialogBoxScr.hideDialog();
        }
        else if (!decide)
        {
            // questTaken = false;
            dialogBoxScr.hideDialog();
        }
    }

    public void QuestConversation()
    {
        if (!questTaken)
        {
            dialogBoxScr.currentQuestGiver = this.gameObject;
            dialogBoxScr.placeHolder = this.NpcQuest.description.ToString();
            dialogBoxScr.acceptButton.SetActive(true);
            dialogBoxScr.rejectButton.SetActive(true);
        }
        else if (questTaken && !NpcQuest.Finished && questEnded == false)
            dialogBoxScr.placeHolder = this.NpcQuest.alreadyTakenQuest.ToString();

        else if (questTaken && this.NpcQuest.Finished && !questEnded)
        {
            dialogBoxScr.placeHolder = NpcQuest.completedQuest.ToString();
            PlayerScr.Gold += NpcQuest.money;
            questEnded = true;
            emoteGameObject.SetActive(false);
            redirectScr.DeleteQuest(NpcQuest.name);
            Debug.Log("jel obriso ?" + NpcQuest.name);
            if (NpcQuest.Type == TypeOfQuest.Gathering)
                invScr.DeleteItemAfterQuestEnded(NpcQuest);
        }
    }
}
