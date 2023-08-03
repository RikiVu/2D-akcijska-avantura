using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickUp : ShowOutline
{
    private bool inRange = false;
    public CreateItem item;   
    public Inventory invScr;
    // Start is called before the first frame update
    void Awake()
    {
       
       

    }
   

    // Update is called once per frame
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

        if (Input.GetKey(KeyCode.E) && inRange == true && invScr.isFull == false && item.pickable==true)
        {
           
           // Debug.Log("Picked item");       
            inRange = false;
            invScr.AddItem(item);
            Destroy(this.gameObject);
          //  bool isPicked = Inventory
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


