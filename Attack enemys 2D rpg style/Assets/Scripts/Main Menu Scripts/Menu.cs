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
    public savesButtonScr savesButtonScr;
    [SerializeField] shopInventory shopInventory;
    [SerializeField]
    private CameraMovement cameraMovement;
    public GameObject howToPlayPanel;

    //  public GameObject inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
       escOppened = false;
        optionsOppened = false;
        howToPlayPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape) && GameManager.InvOppened || Input.GetKeyDown(KeyCode.Escape) && gM.QuestListOppened)
        {
            gM.CloseInv();
            gM.CloseQuests();
            shopInventory.ClosePanel();
        }

       else if (Input.GetKeyDown(KeyCode.Escape) && escOppened == false && optionsOppened==false && GameManager.InvOppened== false)
        {
         
            if (cameraMovement.cameraAnimDone == false)
            {
                cameraMovement.skipIntro();
                return;
            }
            EscPanel.SetActive(true);
            escOppened = true;
            Time.timeScale = 0;

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && escOppened == true && optionsOppened == false)
        {
            EscPanel.SetActive(false);
            savesButtonScr.backBtn();
            escOppened = false;
            if(GameManager.gameOver==false)
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

    public void GotoControls()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        howToPlayPanel.SetActive(true);
        EscPanel.SetActive(false);
        Time.timeScale = 0;
    }
    public void ReturnFromControls()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        howToPlayPanel.SetActive(false);
        EscPanel.SetActive(true);
        escOppened = true;
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
    public void ReturnFromDescriptionPanel()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        Time.timeScale = 0;
    }

    public void ReturnToGame()
    {
        FindObjectOfType<AudioManager>().Play("Click");

        if (escOppened == true)
        {
            EscPanel.SetActive(false);
            escOppened = false;
            if (GameManager.gameOver == false)
                Time.timeScale = 1;
        }
    }
    public void GotoMainMenu()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        SaveOrLoad.loading = false;
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }
  
}
