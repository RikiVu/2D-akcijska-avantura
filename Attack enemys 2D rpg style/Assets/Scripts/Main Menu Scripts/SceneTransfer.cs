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

    private void Awake()
    {
        startUp();
    }
    private void startUp()
    {
        mainMenuPanel.gameObject.SetActive(true);
        CreditsPanel.gameObject.SetActive(false);
        OptionPanel.gameObject.SetActive(false);
    }

    public void GotoStart()
    {
        audioManager.Play("Click");
        SceneManager.LoadScene("test");
    }

    public void GotoOptions()
    {
        audioManager.Play("Click");
        mainMenuPanel.gameObject.SetActive(false);
        CreditsPanel.gameObject.SetActive(false);
        OptionPanel.SetActive(true);
    }
    public void GotoCredits()
    {
        audioManager.Play("Click");
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

