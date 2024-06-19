using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransfer : MonoBehaviour
{
    public GameObject OptionPanel;
    public GameObject CreditsPanel;
    public GameObject NewGamePanel;
    public GameObject mainMenuPanel;
    public AudioManager audioManager;
    public CreateSettings createSettings;
    public GameObject loadPanel;
    public savesButtonScr savesButtonScr;
    public GameObject inputField;
    public Button easyBtn;
    public Button mediumBtn;
    public Toggle godmodeToggle;
    private ColorBlock theColorYellow;
    private ColorBlock theColorWhite;


    //private string valueOfInput ="";

    private void Awake()
    {
        startUp();
    }
    private void startUp()
    {
        CreditsPanel.gameObject.SetActive(false);
        NewGamePanel.gameObject.SetActive(false);
        OptionPanel.gameObject.SetActive(false);
        loadPanel.SetActive(false);
        mainMenuPanel.gameObject.SetActive(true);
        //easyBtn.onClick.Invoke();
        //godmodeToggle.isOn = createSettings.godmode;
        theColorYellow = easyBtn.colors;
        theColorWhite = easyBtn.colors;
        theColorYellow.normalColor = Color.yellow;
        theColorWhite.normalColor = Color.white;
        easyBtn.colors = theColorYellow;
        mediumBtn.colors = theColorWhite;
        createSettings.recordSelected = "";
    }

    public void NewGame()
    {
        audioManager.Play("Click");
        createSettings.newGame = true;
        //filter stuff 
       
        createSettings.recordSelected = FilterToValidJsonFileName(inputField.GetComponent<TMP_InputField>().text);
     
      //  Debug.Log(createSettings.recordName);
         SceneManager.LoadScene("Act1");
    }

    public void ChangeStateEasy()
    {
        audioManager.Play("Click");
        createSettings.diff = Diff.Easy;
        easyBtn.colors = theColorYellow;
        mediumBtn.colors = theColorWhite;
    }
    public void ChangeStateMedium()
    {
        audioManager.Play("Click");
        createSettings.diff = Diff.Medium;
        easyBtn.colors = theColorWhite;
        mediumBtn.colors = theColorYellow;
    }
    public void ChangeStateRecord()
    {
        audioManager.Play("Click");
        createSettings.diff = Diff.Medium;
    }
    public void ChangeStateGodmode()
    {
        audioManager.Play("Click");
        createSettings.godmode = !createSettings.godmode;

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
        SceneManager.LoadScene("Act1");


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
    public void GotoNewGamePanel()
    {
        createSettings.recordSelected = "";
        inputField.GetComponent<TMP_InputField>().text = "";
        godmodeToggle.isOn = false;
        createSettings.godmode = false;
        easyBtn.colors = theColorYellow;
        mediumBtn.colors = theColorWhite;
        //easyBtn.onClick.Invoke();

        //toogle to no
        createSettings.diff = Diff.Easy;
        audioManager.Play("Click");
        loadPanel.SetActive(false);
        mainMenuPanel.gameObject.SetActive(false);
        OptionPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        NewGamePanel.gameObject.SetActive(true);
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
        NewGamePanel.gameObject.SetActive(false);
        mainMenuPanel.gameObject.SetActive(true);
    }
    public string FilterToValidJsonFileName(string input)
    {
     
        string validFileName = Regex.Replace(input, @"[^\w\-]", "_");
        return validFileName;
    }


}

