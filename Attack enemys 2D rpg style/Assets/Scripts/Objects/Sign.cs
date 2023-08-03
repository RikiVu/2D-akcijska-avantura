using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Sign : Interactable
{
    public GameObject SignBox;
    public TextMeshProUGUI SignText;
    public string signtext;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            if(SignBox.activeInHierarchy)
            {
                SignBox.SetActive(false);
            }
            else
            {
                SignBox.SetActive(true);
                SignText.text = signtext;
            }
        }
         else if(playerInRange==false)
        {
            contextOff.Raise();
            SignBox.SetActive(false);
        }
         if(playerInRange)
        {
            contextOn.Raise();
        }
    }
    }

