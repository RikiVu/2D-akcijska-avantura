using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlantState
{
    notPlanted,
    planted,
    finished
}
public class PlantAndHarvest : Interactable
{
    public PlantState currentState;
    public Animator animator;
    public float stageTime = 0;
    public Inventory inventory;
    public item itemPlanted;
    private int[] RandomAmountOfCrops = new int[] { 2, 3, 4 };
    int randomGeneratedNum,i;

    public GameManager gameManager;
    [SerializeField]
    private int assignedId;



    private void Start()
    {
        currentState = PlantState.notPlanted;
        assignedId = gameManager.addInPlantList(this, currentState, stageTime);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.E) && playerInRange == true)
        {
            switch (currentState)
            {
                case PlantState.notPlanted:
                    inventory.openInvToPlant();
                    //show panel to plant
                    break;
                case PlantState.planted:
                    //show dialog that this plant is not yet ripe
                    break;
                case PlantState.finished:
                    animator.SetBool("harvested", true);
                    Debug.Log("you got carrots noice");
                    //animator.SetBool("Carrot", false);
                    animator.SetFloat("Stage", 0);
                    currentState = PlantState.notPlanted;
                    gameManager.addInPlantList(currentState, assignedId);
                    playerInRange = false;
                    sendToInv();
                    break;
                default:
                    Debug.Log("greska");
                    break;
            }
        }
    }

    public void Send(CreateItem item,string name)
    {
        // FindObjectOfType<AudioManager>().Play("");
        Debug.Log("Planting");
        currentState = PlantState.planted;
        animator.SetInteger("TypeOfCrop", item.ID);
        animator.SetBool("harvested", false);
        //animator.SetBool(name, true);
        animator.SetFloat("Stage", 0);
        //assign itemPlanted
        itemPlanted.thisItem = item;
        Debug.Log(itemPlanted.thisItem);
        itemPlanted.name = item.name;
        itemPlanted.description = item.description;
        itemPlanted.img = item.icon;
        itemPlanted.Type = item.Type;
        itemPlanted.haveItem = true;
        gameManager.addInPlantList(currentState, assignedId);
    }
    public void loadPlant(PlantState state, item itm)
    {
        currentState = state;
     
        switch (currentState)
        {
            case PlantState.notPlanted:
                playerInRange = false;
                stageTime = 0;
                animator.SetBool("harvested", true);
                animator.SetFloat("Stage", 0);
                break;
            case PlantState.planted:
                itemPlanted.name = itm.name;
                stageTime = 10;
                itemPlanted.description = itm.description;
                itemPlanted.img = itm.thisItem.icon;
                itemPlanted.Type = itm.Type;
                itemPlanted.haveItem = true;
                animator.SetInteger("TypeOfCrop", itm.thisItem.ID);
                animator.SetFloat("Stage", 0.1f);
                break;
            case PlantState.finished:
                itemPlanted.name = itm.name;
                stageTime = 100;
                itemPlanted.description = itm.description;
                itemPlanted.img = itm.thisItem.icon;
                itemPlanted.Type = itm.Type;
                itemPlanted.haveItem = true;
                animator.SetInteger("TypeOfCrop", itm.thisItem.ID);
                animator.SetFloat("Stage", 1);
                break;
            default:
                Debug.Log("greska");
                break;
        }

    }


    private void Update()
    {
        if (currentState== PlantState.planted)
        {
            stageTime += Time.deltaTime;
            animator.SetFloat("Stage", Mathf.Round((stageTime * 100f) / 100f) /100f);
            if (stageTime >= 100)
            {
                stageTime = 0;
                currentState = PlantState.finished;
                gameManager.addInPlantList(currentState, assignedId);
            }
        }
    }

    public void sendToInv()
    {
        randomGeneratedNum = RandomAmountOfCrops[Random.Range(0, RandomAmountOfCrops.Length)];
        for (i = 0; i < randomGeneratedNum; i++)
        {
            itemPlanted.ChestSend(itemPlanted.thisItem);
        }
        itemPlanted.PlayerInv = false;
        if (itemPlanted.haveItem)
        {
            itemPlanted.Destroy();
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.isTrigger)
        {
            playerInRange = true;
            inventory.plantGm = this.gameObject;
            contextOn.Raise();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.isTrigger)
        {
            playerInRange = false;
            inventory.closeInvToPlant();
            contextOff.Raise();

        }
    }







}
