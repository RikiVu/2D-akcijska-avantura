using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;


public class PickUp : ShowOutline
{
    private bool inRange = false;
    public CreateItem item;   
    public Inventory invScr;
    // Start is called before the first frame update
    private GameObject alertPanelGm;
    private AlertPanelScr alertPanelScr;

    public GameManager gameManager;

    public int assignedId;
    private bool picked = false;
    private bool pickable = false;

    public void Awake()
    {
        alertPanelGm = GameObject.FindGameObjectWithTag("alertPanel");
        alertPanelScr = alertPanelGm.GetComponent<AlertPanelScr>();
        pickable = item.pickable;
    }


    public void loadItem(bool state)
    {
        if (state != null)
        {
            picked = state;
           // Debug.Log(item.name + " , picked?: " + picked);
            this.gameObject.SetActive(!state);
       
        }
    }

    void FixedUpdate()
    {
        if(item.pickable)
        {
            ShowOutlineOnItems();
        }
        else
        {
            HideOutlineOnItems();
        }

        if (Input.GetKey(KeyCode.E) && inRange == true  && item.pickable==true)
        {
           if(Inventory.isFull == true)
            {
                alertPanelScr.showAlertPanel("No space in inventory!");
                return;
            }
            inRange = false;
            Debug.Log("pokupio?");
            invScr.AddItem(item);
            picked = true;
            try
            {
                gameManager.changePickupList(assignedId, picked);
            }
            catch
            {
                Debug.Log("error sa saveanjem ");
            }
            this.gameObject.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.isTrigger)
        {
            inRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.isTrigger)
        {
            inRange = false;
        }
    }
}


