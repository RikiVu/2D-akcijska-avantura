using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Sign : Interactable
{
    public GameObject SignBox;
    public TextMeshProUGUI SignText;
    public string signtext;
    private bool oneActive = false;

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            contextOn.Raise();
           
        }
        else if(oneActive && !playerInRange) {
            SignBox.SetActive(false);
            contextOff.Raise();
            oneActive = false;
        }
     

        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
         {
                SignBox.SetActive(true);
                SignText.text = signtext;
                oneActive = true;
          }
        
       
    }
    }

