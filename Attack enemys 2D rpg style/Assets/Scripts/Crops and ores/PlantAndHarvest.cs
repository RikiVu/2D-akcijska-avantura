using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantAndHarvest : MonoBehaviour
{

    //public CreateItem PlantedItem;
    public  bool inRange = false;
    private bool collected = false;
    public Animator animator;
    public float stageTime = 0;
    private bool planted = false;
    public bool Sended = false;
    bool finished = false;
    public chestInventory chestInvScr;
    public Inventory inventory;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.E) && inRange == true)
        {
            if (finished)
            {
                inRange = false;
                animator.SetBool("harvested", true);
                Debug.Log("you got carrots noice");
                animator.SetBool("Carrot", false);
                animator.SetFloat("Stage", 0);
                planted = false;
                finished = false;
                chestInvScr.sendToInv();
                chestInvScr.sendToInv();
            }

           





        }
        if (Input.GetKey(KeyCode.R) && inRange == true && planted && !finished)
        {
            animator.SetBool("harvested", true);
            animator.SetBool("Carrot", false);
            animator.SetFloat("Stage", 0);
            Debug.Log("destroyed");
            inRange = false;
            planted = false;
            stageTime = 0;
        }
    }

    public void Send(CreateItem itm,string name)
        {
         if (!planted)
            {
            Sended = true;
            planted = true;
             // open inventory then chose item to plant 
             inRange = false;
                Debug.Log("Planting");
                animator.SetBool("harvested", false);
                animator.SetBool(name, true);
                chestInvScr.AddItem1(itm);


                // FindObjectOfType<AudioManager>().Play("");

                animator.SetFloat("Stage", 0);
                
            }
}


    private void Update()
    {
        if (planted)
        {

            stageTime += Time.deltaTime;
            
            
            
            animator.SetFloat("Stage", Mathf.Round((stageTime * 100f) / 100f) /100f);
            if (stageTime >= 100)
            {
                planted = false;
                finished = true;
                stageTime = 0;


            }

        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {


        
        if (collision.gameObject.CompareTag("Player") && collision.isTrigger)
        {
            inRange = true;
            inventory.plantGm = this.gameObject;
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
