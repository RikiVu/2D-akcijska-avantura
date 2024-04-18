using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.UIElements;

public class NpcQuestScr : NpcScr
{
    [Header("Quest section")]
    public Create_Quest NpcQuest;
    public CreateItem NpcItem;
   
    public bool questTaken = false;
    public bool questEnded = false;
    protected Redirect_Quest redirectScr;
    public Inventory invScr;
    [SerializeField]
    protected SpriteRenderer emoteSprite;

    //save and load 
    public GameManager gameManager;
    [SerializeField]
    private int assignedId;



    protected override void Start()
    {
        base.Start();
        redirectScr = GameObject.FindGameObjectWithTag("QuestPanel").GetComponent<Redirect_Quest>();
        assignedId = gameManager.addInQuestList(this, questTaken, questEnded, NpcQuest);
    }

    public void loadQuestData(bool taken, bool ended)
    {
        if (taken != null || ended !=null)
        {
            questTaken = taken;
            questEnded = ended;
            if (questTaken && !questEnded)
            {
                emoteSprite.color = new Color(0.4f, 0.74f, 0.95f);
                if (NpcItem!=null)
                    NpcItem.pickable = true;
            }
            else if(questTaken && questEnded)
            {
                emoteSprite.enabled = false;
            }
            else
            {
                emoteSprite.color = new Color(1f, 0.9195983f, 0.5707547f);
                if (NpcItem != null)
                    NpcItem.pickable = false;
            }
        }
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
            if (!questTaken)
            {
                questTaken = true;
                gameManager.addInQuestList(assignedId, questTaken, questEnded);
                redirectScr.AddQuest(NpcQuest, this.gameObject);
                if (NpcItem)
                    NpcItem.pickable = true;

                dialogBoxScr.hideDialog();
                emoteSprite.color = new Color(0.4f, 0.74f, 0.95f);
            }
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
            emoteSprite.enabled = false;
            redirectScr.DeleteQuest(NpcQuest.name);
            gameManager.addInQuestList(assignedId, questTaken, questEnded);
            Debug.Log("jel obriso ?" + NpcQuest.name);
            if (NpcQuest.Type == TypeOfQuest.Gathering)
                invScr.DeleteItemAfterQuestEnded(NpcQuest);
        }
    }
}
