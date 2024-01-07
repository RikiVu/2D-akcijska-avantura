using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private bool chestCollected;
    public Transform SpawnPosition;
    private Animator anim;
    public chestInventory chestInvScr;
    bool chestOppened = false;
    float startTime = 0;
    SpriteRenderer renderer;
    private float HideCounter = 0f;
    private float HideLenght = 0f;


    void Start() // ovo se pokrece samo jednom
    {

        ChestPanel.SetActive(false);
        anim = GetComponent<Animator>(); // anim na foru dohvaca Animator u Unityu
        chestCollected = false; // bool moze bit samo true i false na pocetku "Start je false"
        renderer = GetComponent<SpriteRenderer>();

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
            chestInvScr.sendToInv();
            chestCollected = true;
            playerInRange = false;
            chestOppened = false;
            anim.SetBool("Oppened", false);
            ChestPanel.SetActive(false);
            contextOff.Raise();
            anim.SetBool("Hiding", true);
        }


    }
   

  

    // ako uzme itemee     chestCollected = true;
    public void ShowItems(Vector3 position)
    {
        ChestPanel.transform.position = position;
        // ChestPanel.transform.position = pos2    ;
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
            //Debug.Log("out of range");
            //hestInvScr.DeleteItems();
            playerInRange = false;
            chestOppened = false;
            anim.SetBool("Oppened", false);
            ChestPanel.SetActive(false);
            contextOff.Raise();
            if(chestCollected)
            this.gameObject.SetActive(false);

        }
    }



}
