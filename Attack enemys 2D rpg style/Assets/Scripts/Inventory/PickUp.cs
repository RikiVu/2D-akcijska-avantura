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

    public void Awake()
    {
        alertPanelGm = GameObject.FindGameObjectWithTag("alertPanel");
        alertPanelScr = alertPanelGm.GetComponent<AlertPanelScr>();
    }


    public void loadItem(bool state)
    {
        if (state != null)
        {
            picked = state;
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
            invScr.AddItem(item);
            picked = true;
            try
            {
                gameManager.addInPickupList(assignedId, picked);
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


