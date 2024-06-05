using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransfer : MonoBehaviour
{
    public GameObject OptionPanel;
    public GameObject CreditsPanel;
    public GameObject mainMenuPanel;
    public AudioManager audioManager;
    public CreateSettings createSettings;
    public GameObject loadPanel;
    public savesButtonScr savesButtonScr;

    private void Awake()
    {
        startUp();
    }
    private void startUp()
    {
        CreditsPanel.gameObject.SetActive(false);
        OptionPanel.gameObject.SetActive(false);
        loadPanel.SetActive(false);
        mainMenuPanel.gameObject.SetActive(true);
      
    }

    public void NewGame()
    {
        audioManager.Play("Click");
        createSettings.newGame = true;
        SceneManager.LoadScene("test");

    }
    public void ShowLoadPanel()
    {
        audioManager.Play("Click");
        loadPanel.SetActive(true);
        savesButtonScr.ShowLoadRecordsMenu();
    }
    public void LoadGame()
    {
        audioManager.Play("Click");
        createSettings.newGame = false;
        SceneManager.LoadScene("test");


    }

    public void GotoOptions()
    {
        audioManager.Play("Click");
        loadPanel.SetActive(false);
        mainMenuPanel.gameObject.SetActive(false);
        CreditsPanel.gameObject.SetActive(false);
        OptionPanel.SetActive(true);
    }
    public void GotoCredits()
    {
        audioManager.Play("Click");
        loadPanel.SetActive(false);
        mainMenuPanel.gameObject.SetActive(false);
        OptionPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }
    public void Quit()
    {
        audioManager.Play("Click");
        Application.Quit();
    }
   
    public void GotoMain() 
    {
        audioManager.Play("Click");
        OptionPanel.SetActive(false);
        CreditsPanel.gameObject.SetActive(false);
        mainMenuPanel.gameObject.SetActive(true);
    }
   

}

