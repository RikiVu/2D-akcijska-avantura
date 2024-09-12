using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class item : MonoBehaviour
{
    public TypeOfItem Type;
    public int CurrentNum = 0;
    public Inventory invScr;
    public Inventory invScr2;
    public GameObject InventoryGM;
    public string name;
    public string description;
    public Sprite img;
    private Image sprite;
    public bool haveItem = false;
    public GameObject Player;
    private PlayerScr plyScr;
    public GameObject DeleteButton;
    public GameObject SellButton;
    public GameObject PlantButton;
    [SerializeField]
    protected GameObject Tooltip;
    public bool CantUse = false;
    public GameObject stackCount;
    public TextMeshProUGUI broj;
    public int counter1 = 1;
    [SerializeField]
    toolTipScr tooltipSCR;
    public bool PlayerInv;
    public CreateItem thisItem;
    public GameObject priceGM;
    public TextMeshProUGUI priceText;
    public int id;
    int reducedPrice = 0;
    public GameObject EquipmentGM;
    public Equipment EquipmentScr;
    public bool equiped = false;

    public Transform TipLocation;
    private GameObject alertPanelGm;
    private AlertPanelScr alertPanelScr;
    [SerializeField]
    private Image SlotImg;
    private Sprite originalSlot;
    [SerializeField]
    private Sprite SlotImgHighlight;

    public void Awake()
    {
        alertPanelGm = GameObject.FindGameObjectWithTag("alertPanel");
        alertPanelScr = alertPanelGm.GetComponent<AlertPanelScr>();
        sprite = this.gameObject.GetComponent<Image>();
        originalSlot = SlotImg.sprite;
        if(tooltipSCR== null)
        {
            Tooltip = GameObject.FindGameObjectWithTag("ToolTip");

            if (Tooltip.GetComponent<toolTipScr>() != null)
            {
                //Debug.Log("tooltip nije null");
                tooltipSCR = Tooltip.GetComponent<toolTipScr>();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        plyScr = Player.GetComponent<PlayerScr>();
        if (img != null)
        {
            //ImageObject.;

            sprite.sprite = img;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (haveItem)
        {
            priceGM.SetActive(true);
            if (!PlayerInv && !equiped)
            {

                stackCount.gameObject.SetActive(false);
                priceText.text = thisItem.Price.ToString();


            }
            else if (!PlayerInv && equiped)
            {
                stackCount.gameObject.SetActive(false);
                priceGM.SetActive(false);
            }

            else if (PlayerInv)
            {
                reducedPrice = thisItem.Price / 2;
                priceText.text = reducedPrice.ToString();
                stackCount.gameObject.SetActive(true);
            }


            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);

            broj.text = counter1.ToString();

            if (img != null)
            {
                //ImageObject.;

                sprite.sprite = img;
            }
        }

        else
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);

            stackCount.gameObject.SetActive(false);
            priceGM.SetActive(false);
            counter1 = 1;
        }

    }


    public void Use()
    {
        if (CantUse == false)
        {
            if (Type == TypeOfItem.HealingPotion)
            {
                if (HeartManager.playerCurrentHealth < HeartManager.playerMaxHealth)
                {
                    if (HeartManager.playerCurrentHealth == HeartManager.playerMaxHealth - 1)
                        HeartManager.playerCurrentHealth += 1;
                    else
                        HeartManager.playerCurrentHealth += 2;

                    FindObjectOfType<AudioManager>().Play("HealHeart");
                    plyScr.PlayerHealthSignal.Raise();
                    if (counter1 == 1)
                    {
                        Tooltip.SetActive(false);
                        name = null;
                        description = null;
                        img = null;
                        //  sprite.enabled = false;
                        sprite.sprite = null;
                        haveItem = false;
                        if (PlayerInv)
                            invScr.checkSpaceInInventory(1);
                    }
                    else if (counter1 > 1 && counter1 < 6)
                        counter1--;
              
                }
            }
            else if (Type == TypeOfItem.Equipment)
                Equip();
        }
    }


    public void Destroy()
    {
        if (haveItem != false)
        {
            if (thisItem.isStackable && counter1 > 1 && thisItem.Type != TypeOfItem.Quest)
            {
                name = null;
                description = null;
                img = null;
                haveItem = false;
                DeleteButton.SetActive(false);
                counter1 = 1;
            }
            else if (thisItem.isStackable && counter1 >= 1 && thisItem.Type == TypeOfItem.Quest)
            {
                name = null;
                description = null;
                img = null;
                haveItem = false;
                DeleteButton.SetActive(false);
                counter1 = 1;
            }
            else
            {
                name = null;
                description = null;
                img = null;
                if (PlayerInv)
                    invScr.checkSpaceInInventory(1);
                haveItem = false;
                DeleteButton.SetActive(false);
            }
        }
    }
    public void Sell()
    {
        if (haveItem != false && shopInventory.InShop)
        {
            if (thisItem.isStackable && counter1 > 1)
            {
                counter1--;
            }
            else
            {
                invScr.checkSpaceInInventory(1); 
                name = null;
                description = null;
                img = null;
                haveItem = false;
                SellButton.SetActive(false);
            }
            PlayerScr.Gold += thisItem.Price / 2;
        }
    }
    bool temp = false;

    public void Plant()
    {
        if (haveItem)
        {
            temp = invScr.transferFromItem(thisItem, thisItem.name);
            if (temp)
            {
                if (thisItem.isStackable && counter1 > 1)
                    counter1--;
                else
                {
                    invScr.checkSpaceInInventory(1);
                    name = null;
                    description = null;
                    img = null;
                    haveItem = false;
                    SellButton.SetActive(false);
                }
            }
        }
    }
    
    public void Equip()
    {
        name = null;
        description = null;
        img = null;
        sprite.sprite = null;
        haveItem = false;
        EquipmentScr.AddItem(thisItem);
        Tooltip.SetActive(false);
       
        
    }
    public void UnEquip()
    {
        if (haveItem)
        {
            if (Inventory.isFull == false)
            {
                EquipmentScr.UnequipItem(this);

                Debug.Log("Unequip");
                Tooltip.SetActive(false);
                name = null;
                description = null;
                img = null;
                sprite.sprite = null;
                haveItem = false;
            }
            else
            {
                alertPanelScr.showAlertPanel("No space in inventory!");
            }
        }
        else
        {
            Debug.Log("Its empty");
        }

    }
    public void UnEquipForLoad()
    {
        Tooltip.SetActive(false);
        name = null;
        description = null;
        img = null;
        sprite.sprite = null;
        haveItem = false;
    }


    public void PointerEnter()
    {
        if (haveItem) 
        {
            SlotImg.sprite = SlotImgHighlight;
            Tooltip.SetActive(true);
            tooltipSCR.ChangeText(name, description);
            Tooltip.transform.position = TipLocation.position;
        }
    }

    public void PointerClick()
    {
        if (haveItem && PlayerInv)
            Use();
        else if (haveItem && !PlayerInv && equiped == false)
        {
            if (Inventory.isFull == true)
            {
                alertPanelScr.showAlertPanel("No space in inventory!");
                Debug.Log("No space in inventory!");
                return;
            }
            if (PlayerScr.Gold >= thisItem.Price)
            {
                if (thisItem.Type != TypeOfItem.Arrows)
                {
                    invScr2.AddItem(thisItem, false);
                    PlayerScr.Gold -= thisItem.Price;
                }
                if (thisItem.Type == TypeOfItem.Arrows && PlayerScr.Arrows < PlayerScr.MaxArrows)
                {
                    PlayerScr.Arrows += 1;
                    PlayerScr.Gold -= thisItem.Price;
                }
            }
            else
            {
                alertPanelScr.showAlertPanel("Insufficient gold");
                Debug.Log("Insufficient gold");
            }


        }
    }

    public void ChestSend(CreateItem item)
    {
        invScr2.AddItem(item,false);
    }

    public void PointerExit()
    {
        SlotImg.sprite = originalSlot;
        Tooltip.SetActive(false);
    }
}
