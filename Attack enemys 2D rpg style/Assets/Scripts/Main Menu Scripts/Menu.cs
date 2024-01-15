using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;



public class Menu : MonoBehaviour
{
    public PlayerScr player;
    //  public CoinCounter Coins;
    public GameManager gM;
    public GameObject OptionPanel;
    public GameObject EscPanel;
    bool escOppened;
    bool optionsOppened;
    public bool StopSong;
    public int Coinvalue;


  //  public GameObject inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
       escOppened = false;
        optionsOppened = false;
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape) && GameManager.InvOppened || Input.GetKeyDown(KeyCode.Escape) && gM.QuestListOppened)
        {
            gM.CloseInv();
            gM.CloseQuests();
        }

       else if (Input.GetKeyDown(KeyCode.Escape) && escOppened == false && optionsOppened==false && GameManager.InvOppened== false)
        {
           

            EscPanel.SetActive(true);
            escOppened = true;

           // inventoryUI.SetActive(false);
           Time.timeScale = 0;

            

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && escOppened == true && optionsOppened == false)
        {
            EscPanel.SetActive(false);
            escOppened = false;
            Time.timeScale = 1;

        }
        if(optionsOppened == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape)  && optionsOppened == true)
            {

                OptionPanel.SetActive(false);
                EscPanel.SetActive(true);
                optionsOppened = false;
                escOppened = true;
                Time.timeScale = 0;
            }
        }
      


    }

    public void OnPointer()
    {
        // Do something.
        Debug.Log("<color=red>Event:</color> Completed mouse highlight.");
    }
    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
      //  FindObjectOfType<AudioManager>().Play("Menu");

    }

    public void GotoOptions()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        OptionPanel.SetActive(true);
        EscPanel.SetActive(false);
        optionsOppened = true;
        escOppened = false;
        Time.timeScale = 0;


    }
    public void Save()
    {
        FindObjectOfType<AudioManager>().Play("Click");
      //  SaveSystem.SavePlayer(player);
    }
    public void Load()
    {
        FindObjectOfType<AudioManager>().Play("Click");
       // PlayerData data = SaveSystem.LoadPlayer();

        //Health
       // player.MaxHealth = data.MaxHealth;
      //  player.currentHealth = data.currentHealth;
        
        //Gold
      //  PlayerScr.Gold = data.Gold;

        //Pozicija 
       // Vector3 position;
      //  position.x = data.position[0];
      //  position.y = data.position[1];
       // position.z = data.position[2];
      //  player.transform.position = position;

      

    }
    public void ReturnFromOptions()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        OptionPanel.SetActive(false);
        EscPanel.SetActive(true);
        optionsOppened = false;
        escOppened = true;
        Time.timeScale = 0;
    }

    public void ReturnToGame()
    {
        FindObjectOfType<AudioManager>().Play("Click");

        if (escOppened == true)
        {
            EscPanel.SetActive(false);
            escOppened = false;
            Time.timeScale = 1;
        }
    }
    public void GotoMainMenu()
    {
        FindObjectOfType<AudioManager>().Play("Click");

        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }
  
}
