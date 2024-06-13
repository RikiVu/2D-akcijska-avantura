using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertPanelScr : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI textPanel;
    private bool coroutineStarted = false;
    void Start()
    {
        textPanel.text ="no text";
        panel.SetActive(false);
    }

    public void showAlertPanel(string text)
    {
        textPanel.text = text;
        if(!coroutineStarted) {
            StartCoroutine(PanelShowed());
        }
     
    }
    public void showAlertPanel(string text, float time)
    {
        textPanel.text = text;
        if (!coroutineStarted)
        {
            StartCoroutine(PanelShowed(time));
        }

    }

    private IEnumerator PanelShowed()
    {
        coroutineStarted = true;
        panel.SetActive(true);
        yield return new WaitForSeconds(2f);
        panel.SetActive(false);
        coroutineStarted = false;
    }
    private IEnumerator PanelShowed(float time)
    {
        coroutineStarted = true;
        panel.SetActive(true);
        yield return new WaitForSeconds(time);
        panel.SetActive(false);
        coroutineStarted = false;
    }


}
