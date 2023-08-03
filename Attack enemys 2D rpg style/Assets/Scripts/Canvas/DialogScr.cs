using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialogScr : MonoBehaviour
{
    public TextMeshProUGUI TmProText;
    public GameObject acceptButton;
    public GameObject rejectButton;
    public GameObject currentQuestGiver;
    private QuestNPC Scr;
    public GameObject ShopPanel;
    public bool Shop = false;
    public bool radi = false;
    public string placeHolder = "";
   

    // Start is called before the first frame update
    void Awake()
    {
        TmProText.text = "";
        acceptButton.SetActive(false);
        rejectButton.SetActive(false);
    }

    private void FixedUpdate()
    {
        
        if(this.gameObject.activeInHierarchy && !radi)
        {
            

            
                radi = true;
                StartCoroutine(FreezeCo());
           
 
      
        }
       
       
        
    }
  

    public void AcceptQuest()
    {
        if(!Shop)
        {
            Scr = currentQuestGiver.GetComponent<QuestNPC>();
            // if(Scr.)
            radi = false;
            TmProText.text = "";
            Scr.Decide(true);
            Debug.Log(Scr.name + " test  ");
        }
        else
        {
            Debug.Log("Shop1");
            radi = false;
            TmProText.text = "";
            ShopPanel.SetActive(true);
            this.gameObject.SetActive(false);
            Time.timeScale = 0;
        }
      
        
    }
    public void RejectQuest()
    {
        if (!Shop)
        {
            if(currentQuestGiver!=null)
            {
                Scr = currentQuestGiver.GetComponent<QuestNPC>();
                Scr.Decide(false);
            }
          
            radi = false;
            TmProText.text = "";
           
        }
        else
        {
          
            Debug.Log("Shop");
            radi = false;
            TmProText.text = "";
            this.gameObject.SetActive(false);
        }

    }

    private IEnumerator FreezeCo()
    {
       

        int i = 0;
       
        for (i = 0; i < placeHolder.Length; i++)
        {
           
            TmProText.text += placeHolder[i];


            yield return new WaitForSeconds(0.05f);
            if (Input.GetKey(KeyCode.E) && i>4)
            {

                TmProText.text = placeHolder;
                yield return new WaitForSeconds(0.1f);
                yield break;

            }
        }
        
            
       
        // TmProText.text = b + Time.deltaTime;


    }


}
