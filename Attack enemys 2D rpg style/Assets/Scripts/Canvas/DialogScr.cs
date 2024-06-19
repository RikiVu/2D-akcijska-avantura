using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialogScr : MonoBehaviour
{
    public TextMeshProUGUI TmProText;
    public TextMeshProUGUI tmProNameText;
    public GameObject acceptButton;
    public GameObject rejectButton;
    public GameObject currentQuestGiver;
    private NpcQuestScr Scr;
    public GameObject ShopPanel;
    public bool Shop = false;
    public bool coroutineStarted = false;
    public bool talking = false;
    public string placeHolder = "";
    private RectTransform rectTransform;
    public GameObject Inventory;
    [SerializeField]
    private AudioManager audioManager;



    // Start is called before the first frame update
    void Awake()
    {
        TmProText.text = "";
        tmProNameText.text = "";
        acceptButton.SetActive(false);
        rejectButton.SetActive(false);
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(talking)
        {
           if(!coroutineStarted)
                StartCoroutine(FreezeCo());
        }
    }
  

    public void AcceptQuest()
    {
        if(!Shop)
        {
            Scr = currentQuestGiver.GetComponent<NpcQuestScr>();
            // if(Scr.)
            coroutineStarted = false;
            TmProText.text = "";
            tmProNameText.text = "";
            Scr.Decide(true);
            Debug.Log(Scr.name + " test  ");
        }
        else
        {
            Debug.Log("Shop1");
            shopInventory.InShop = true;
            coroutineStarted = false;
            TmProText.text = "";
            tmProNameText.text = "";
            ShopPanel.SetActive(true);
            hideDialog();
            audioManager.Play("ShopSound");
            PlayerScr.playerCanMove = false;
            GameManager.InvOppened = true;
            Inventory.gameObject.SetActive(true);
            //Time.timeScale = 0;
        }
    }
    public void RejectQuest()
    {
        if (!Shop)
        {
            if(currentQuestGiver!=null)
            {
                Scr = currentQuestGiver.GetComponent<NpcQuestScr>();
                Scr.Decide(false);
            }
            coroutineStarted = false;
            TmProText.text = "";
            tmProNameText.text = "";
        }
        else
        {
            Debug.Log("Shop");
            coroutineStarted = false;
            TmProText.text = "";
            tmProNameText.text = "";
            hideDialog();
        }
    }



                                                                                                                    //new 
    public void showDialog(string text, string name)
    {
        talking = true;
        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        Shop = false;
        acceptButton.SetActive(false);
        rejectButton.SetActive(false);
        placeHolder = text.ToString();
        tmProNameText.text = name;
    }
    public void showDialogShop(string text, string name)
    {
        talking = true;
        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        Shop = true;
        acceptButton.SetActive(true);
        rejectButton.SetActive(true);
        placeHolder = text.ToString();
        tmProNameText.text = name;
    }


    public void hideDialog()
    {
        try
        {
            talking = false;
            if (rectTransform != null)
                rectTransform.anchoredPosition = new Vector3(0, -200, 0);
            StopCoroutine(FreezeCo());
            coroutineStarted = false;
            placeHolder = "";
        }
        catch
        {
            Debug.Log("bug");
        }
      
    }

    private IEnumerator FreezeCo()
    {
        coroutineStarted = true;
        int i = 0;
        TmProText.text = "";

        for (i = 0; i < placeHolder.Length; i++)
        {
            audioManager.Play("Typing");
            TmProText.text += placeHolder[i];
            yield return new WaitForSeconds(0.05f);
            if (Input.GetKey(KeyCode.E) && i>4)
            {
                TmProText.text = placeHolder;
                yield return new WaitForSeconds(0.1f);
                yield break;
            }
        }
       
    }
}
