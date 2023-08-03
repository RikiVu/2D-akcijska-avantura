using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;



public class item : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TypeOfItem Type;
    public int CurrentNum = 0; 
    public Inventory invScr;
    public Inventory invScr2;
    public GameObject InventoryGM;
    public string name;
    public string description;
    public Sprite img;
    private  Image sprite;
    public bool haveItem =false;
    public GameObject Player;
    private PlayerScr plyScr;
    public  GameObject DeleteButton;
    public GameObject SellButton;
    public GameObject PlantButton;
    private GameObject Tooltip;
    public bool CantUse = false;
    public GameObject stackCount;
    public TextMeshProUGUI broj;
    public int counter1 =1;
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

    public void Awake()
    {

        Tooltip = GameObject.FindGameObjectWithTag("ToolTip");
        sprite = this.gameObject.GetComponent<Image>();
        tooltipSCR = Tooltip.GetComponent<toolTipScr>();
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
            if ( !PlayerInv && !equiped)
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
                reducedPrice = thisItem.Price /2;
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


    public  void Use()
    {
        if (CantUse == false)
        {
            

            if (Type == TypeOfItem.HealingPotion)
            {

                if (plyScr.currentHealth.RuntimeValue < plyScr.currentHealth.initialValue)
                {

                    if (plyScr.currentHealth.RuntimeValue == plyScr.currentHealth.initialValue - 1)
                    {
                        plyScr.currentHealth.RuntimeValue += 1;
                    }
                    else
                    {
                        plyScr.currentHealth.RuntimeValue += 2;
                    }
                    FindObjectOfType<AudioManager>().Play("HealHeart");

                    plyScr.PlayerHealthSignal.Raise();

                    if(counter1 == 1)
                    {
                        Tooltip.SetActive(false);
                        name = null;
                        description = null;
                        img = null;
                        //  sprite.enabled = false;
                        sprite.sprite = null;

                        haveItem = false;
                        if(PlayerInv)
                        invScr.Pun -= 1;
                    }
                    else if(counter1>1 && counter1<6)
                    {
                        counter1--;
                    }

                   



                }
            }

            else if(Type == TypeOfItem.Equipment)
            {
                Equip();
            }
            /*
            else if (Type == TypeOfItem.Veggies)
            {
                EquipInHand();  
            }
            */
        }
    }
   

    public void Destroy()
    {
        if(haveItem != false)
        {
            if(thisItem.isStackable && counter1 >1 && thisItem.Type != TypeOfItem.Quest)
            {
                
            }
            else if(thisItem.isStackable && counter1 >= 1 && thisItem.Type == TypeOfItem.Quest)
            {
                name = null;
                description = null;

                img = null;
                //  sprite.enabled = false;

                //sprite.sprite = null;



                haveItem = false;
                DeleteButton.SetActive(false);
                counter1 = 1;
            }
            else
            {
                name = null;
                description = null;

                img = null;
                //  sprite.enabled = false;

                //sprite.sprite = null;

                if (PlayerInv)
                    invScr.Pun -= 1;


                haveItem = false;
                DeleteButton.SetActive(false);
            }
           

        }
    }
 public  void Sell()
    {
        if (haveItem != false)
        {
            if (thisItem.isStackable && counter1 > 1)
            {
                counter1--;
            }
            else
            {
                invScr.Pun -= 1;
                name = null;
                description = null;
                img = null;

                haveItem = false;
                SellButton.SetActive(false);
               
            }
            PlayerScr.Gold += thisItem.Price / 4;

        }
    }


    public void Plant()
    {
      
        if (haveItem)
        {
            
            if (thisItem.isStackable && counter1 > 1)
            {
                counter1--;
            }
            else
            {
                invScr.Pun -= 1;
                name = null;
                description = null;
                img = null;

                haveItem = false;
                SellButton.SetActive(false);

            }
        

            invScr.transferFromItem(thisItem, thisItem.name);
           

        }
    }

    public void Equip()
    {
        EquipmentScr.AddItem(thisItem);
       
            Tooltip.SetActive(false);
            name = null;
            description = null;
            img = null;
            //  sprite.enabled = false;
            sprite.sprite = null;

            haveItem = false;
            if (PlayerInv == true)
                invScr.Pun -= 1;
        
        
       
    }
  
   
    public void OnPointerEnter(PointerEventData eventData)
        {

      
            if (haveItem)
            {
           
                Tooltip.SetActive(true);
           
                Tooltip.transform.position = this.transform.position + new Vector3(0,55,0);
                tooltipSCR.ChangeText(name, description);
                //Debug.Log("Uso");
            }
       
         }

    public void OnPointerClick(PointerEventData eventData)
    {
        
      //  Debug.Log("click");
        if (haveItem && PlayerInv)
        {
           
            Use();
        }
        else if(haveItem && !PlayerInv && equiped == false)
        {
         
            if (PlayerScr.Gold >= thisItem.Price)
            {
                
                if(thisItem.Type != TypeOfItem.Arrows)
                {
                     invScr2.AddItem(thisItem);
                    PlayerScr.Gold -= thisItem.Price;
                }
               
                if (thisItem.Type == TypeOfItem.Arrows && PlayerScr.Arrows <5)
                {
                    PlayerScr.Arrows += 1;
                    PlayerScr.Gold -= thisItem.Price;
                }
                
                //  ShopNPC.id = id;
                // ShopNPC.CanRemove = true;
                // Destroy();
            }
            else
            {
                Debug.Log("You can't buy shit with that amount money");
            }
           
    
        }
       
    }

   public void ChestSend()
    {
        invScr2.AddItem(thisItem);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.SetActive(false);
      //  Debug.Log("Izaso");
    }
}
