using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class descriptionPanelScr : MonoBehaviour
{
    //[SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI textPanel;
    [SerializeField]
    private AudioManager audioManager;
    private List<string> list = new List<string>();

    public void showDescriptionPanel()
    {
       // textPanel.text = text;
        this.gameObject.SetActive(true);
        Time.timeScale = 0;
        audioManager.Play("Victory");
    }
    public void showDescriptionPanel(List<string> arrayOfString)
    {
            list= arrayOfString;
            textPanel.text = arrayOfString[0];
            this.gameObject.SetActive(true);
            Time.timeScale = 0;
    }
    public void hideDescriptionPanel()
    {
        if(list.Count <= 1) {
            this.gameObject.SetActive(false);
            Time.timeScale = 1;
            list.Clear();
            return;
        }
        list.Remove(list[0]);
        showDescriptionPanel(list);
    }
  
}
