using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : NpcScr
{
    [Header("Shop")]
    public CreateItem[] itemsInStock = new CreateItem[5];
    public shopInventory ShopInvScr;
    public static int id;
    private int CounterOfItems;
   // private bool talking = false;

    // public static bool CanRemove = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        dialogBox = GameObject.FindGameObjectWithTag("Dialog");
        dialogBoxScr = dialogBox.GetComponent<DialogScr>();
        anim = gameObject.GetComponent<Animator>();
        CounterOfItems = itemsInStock.Length;
    }

    public override void Interact()
    {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //talking = true;
                dialogBoxScr.currentQuestGiver = this.gameObject;
                dialogBoxScr.showDialogShop(dialogTekst, name);
                ShopInvScr.DeleteItems();
                for (int i = 0; i < CounterOfItems; i++)
                {
                    ShopInvScr.AddItem1(itemsInStock[i], i);
                }
            }
    }

 
    void DeleteItem()
    {
        Debug.Log("removed");
        itemsInStock[id] = null;
    }

    protected override void Update()
    {
        if (playerInRange == true)
        {
            Interact();
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            playerInRange = true;
        }
    }


}
