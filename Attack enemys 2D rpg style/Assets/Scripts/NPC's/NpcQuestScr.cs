
using UnityEngine;

public class NpcQuestScr : NpcScr
{
    [Header("Quest section")]
    public Create_Quest NpcQuest;
    public CreateItem NpcItem;
    private bool npcItemState;
    public bool questTaken = false;
    public bool questEnded = false;
    protected Redirect_Quest redirectScr;
    public Inventory invScr;
    [SerializeField]
    protected SpriteRenderer emoteSprite;

    //save and load 
    public GameManager gameManager;

    public int assignedId;

    

    protected override void Start()
    {
        base.Start();
        redirectScr = GameObject.FindGameObjectWithTag("QuestPanel").GetComponent<Redirect_Quest>();
    }

    public void loadQuestData(bool taken, bool ended, int count)
    {
        
        if (taken != null || ended !=null)
        {
            questTaken = taken;
            questEnded = ended;
           
            if (NpcQuest.Type == TypeOfQuest.Gathering)
                    NpcItem.pickable = false;

            if (questTaken && !questEnded)
            {
                emoteSprite.enabled = true;
                emoteSprite.color = new Color(0.4f, 0.74f, 0.95f);
                if (NpcQuest.Type == TypeOfQuest.Gathering)
                {
                   // Debug.Log(NpcQuest.name + " , " + questTaken + " , " + questEnded);
                    this.NpcItem.pickable = true;
                    
                }
                       
            }
            else if(questTaken && questEnded)
            {
                emoteSprite.enabled = false;
                NpcItem.pickable = false;
            }
            else
            {
                emoteSprite.enabled = true;
                emoteSprite.color = new Color(1f, 0.9195983f, 0.5707547f);
            }
       
        }
    }

    void Awake()
    {
        NpcQuest.Finished = false;
        if (NpcItem && NpcItem.name != "Star")
        {
            NpcItem.pickable = false;
            npcItemState = NpcItem.pickable;
        }
    }

    public override void Interact()
    {
        if (playerInRange == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogBoxScr.showDialog(dialogTekst, name);
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
                redirectScr.AddQuest(NpcQuest, this,0);
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
            Debug.Log("ugasio");
            emoteSprite.enabled = false;
            redirectScr.DeleteQuest(NpcQuest.name);
            gameManager.addInQuestList(assignedId, questTaken, questEnded);
            Debug.Log("jel obriso ?" + NpcQuest.name);
            if (NpcQuest.Type == TypeOfQuest.Gathering)
                invScr.DeleteItemAfterQuestEnded(NpcQuest);
        }
    }
}
