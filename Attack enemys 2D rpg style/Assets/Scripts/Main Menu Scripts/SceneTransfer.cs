using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransfer : MonoBehaviour
{
    public GameObject OptionPanel;
    public GameObject CreditsPanel;

    public void GotoStart()
    {
        SceneManager.LoadScene("test");
    }

    public void GotoOptions()
    {
        OptionPanel.SetActive(true);
    }
    public void GotoCredits()
    {
        CreditsPanel.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
   
    public void ReturnFromOptions() 
    {
        OptionPanel.SetActive(false);
    }
   
    public void ReturnFromCredits()
    {
        CreditsPanel.SetActive(false);
    }
}

