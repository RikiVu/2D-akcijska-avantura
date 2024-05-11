using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
// ovo gore su libary 
public class ChestSCR : MonoBehaviour
{
    //public item[] itemScr;
    public CreateItem ChestItem;
    private Vector3 pos;
    public GameObject ItemsInsideObject;
    
    public bool playerInRange;
    public SignalSender contextOn;
    public SignalSender contextOff;

    public GameObject ChestPanel;
    public bool chestCollected;
    public Transform SpawnPosition;
    private Animator anim;
    public chestInventory chestInvScr;
    bool chestOppened = false;



    private GameObject alertPanelGm;
    private AlertPanelScr alertPanelScr;

    public GameManager gameManager;
    public int assignedId;

    void Start() // ovo se pokrece samo jednom
    {
        alertPanelGm = GameObject.FindGameObjectWithTag("alertPanel");
        alertPanelScr = alertPanelGm.GetComponent<AlertPanelScr>();
        ChestPanel.SetActive(false);
        anim = GetComponent<Animator>(); // anim na foru dohvaca Animator u Unityu
        chestCollected = false; // bool moze bit samo true i false na pocetku "Start je false"
    }
void Update()
{
        // ovo se pokrece svaki frame ( Update)
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && !chestOppened && !chestCollected)
        {
            // kad stisne slovo E i player je u rangeu tek onda ulazi u statement        
            anim.SetBool("Oppened", true);
            chestOppened = true;

            pos = Camera.main.WorldToScreenPoint(ItemsInsideObject.transform.position);
            FindObjectOfType<AudioManager>().Play("ChestOppened");
            ChestPanel.SetActive(true);
            if (chestCollected == false)
            {
                ShowItems(pos);
                chestInvScr.AddItem1(ChestItem);
                //   PlayerScr.Gold += 50;              
                Debug.Log("otvorio si chest");
            }
            else if (chestCollected)
            {
                Debug.Log("You already looted this chest");
            }
        }
       else if (Input.GetKeyDown(KeyCode.E) && !chestCollected  && chestOppened)
        {
            if(Inventory.isFull == false || ChestItem.Type == TypeOfItem.Star)
            {
                chestInvScr.sendToInv();
                chestCollected = true;
                playerInRange = false;
                chestOppened = false;
                //anim.SetBool("Oppened", false);
                ChestPanel.SetActive(false);
                contextOff.Raise();
                gameManager.addInChestList(chestCollected, assignedId);
            }
            else
            {
                alertPanelScr.showAlertPanel("No space in inventory!");
                Debug.Log("No space in inventory!");
            }
        }
    }

    public void loadChest(bool state)
    {
        Debug.Log("chest load");
        chestCollected = state;
        if (!state)
            anim.SetBool("Oppened", false);
        else
            anim.SetBool("Oppened", true);
 
    }

    // ako uzme itemee     chestCollected = true;
    public void ShowItems(Vector3 position)
    {
        ChestPanel.transform.position = position;
        ChestPanel.SetActive(true);
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
          //  Debug.Log("in range");
            playerInRange = true;
            if (playerInRange && chestCollected == false)
            {
                contextOn.Raise();
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
     
            if (other.CompareTag("Player") && other.isTrigger)
            {
               // Debug.Log("out of range");
                playerInRange = false;
                chestOppened = false;
                //anim.SetBool("Oppened", false);
                ChestPanel.SetActive(false);
                contextOff.Raise();
                if (!chestCollected)
                    anim.SetBool("Oppened", false);

            }
    }



}
