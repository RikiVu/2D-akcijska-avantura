using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Equipment : MonoBehaviour
{
    public GameObject[] slot = new GameObject[6];
    
    private item itemScr;
   [SerializeField]  private TextMeshProUGUI StrenghtText;
    [SerializeField] private TextMeshProUGUI ConstitutionText;
    [SerializeField] private TextMeshProUGUI DexterityText;
    [SerializeField] private TextMeshProUGUI walkText;
    [SerializeField] private TextMeshProUGUI runText;
    //public FloatValue currentHealth;
    // public FloatValue heartContainers;
    public ArmorManager armorManager;
    public Inventory Inventory;
    public int Pun = 0;
    protected int Prazan = 0;
    public int x = 0;
    private float i = 0;
    protected Redirect_Quest Redirect;
    private GameObject QuestPanel;

    private void Start()
    {
        QuestPanel = GameObject.FindGameObjectWithTag("QuestPanel");
        Redirect = QuestPanel.GetComponent<Redirect_Quest>();
    }

    public void AddItem(CreateItem item)
    {
        switch (item.TypeOfEquipment)
        {
            case TypeOfEquipment.None:
                break;
            case TypeOfEquipment.Weapon:
                itemScr = slot[5].GetComponentInChildren<item>();
                if (itemScr.haveItem == false)
                {
                    Knockback.damageBoost += item.damageBoost;
                    PlayerScr.haveSword = true;
                    AllowPassage.CanPass = true;
                    itemScr.equiped = true;
                    itemScr.thisItem = item;
                    itemScr.name = item.name;
                    itemScr.description = item.description;
                    itemScr.img = item.icon;
                    itemScr.Type = item.Type;
                    itemScr.haveItem = true;

                    i = 0 + Knockback.damageBoost;
                    StrenghtText.text = i.ToString();
                    ConstitutionText.text = armorManager.armorContainers.ToString();
                    
                    //Debug.Log("Equipo");
                }
                else
                {
                    Inventory.AddItem(itemScr.thisItem);
                    Knockback.damageBoost -= itemScr.thisItem.damageBoost;

                    Knockback.damageBoost += item.damageBoost;
                    PlayerScr.haveSword = true;
                    AllowPassage.CanPass = true;
                    itemScr.equiped = true;
                    itemScr.thisItem = item;
                    itemScr.name = item.name;
                    itemScr.description = item.description;
                    itemScr.img = item.icon;
                    itemScr.Type = item.Type;
                    itemScr.haveItem = true;
                    Debug.Log("Swap Equipo");
                }
               
                break;
            case TypeOfEquipment.Helmet:
                itemScr = slot[0].GetComponentInChildren<item>();
                if (itemScr.haveItem == false)
                {
                    itemScr.equiped = true;
                    itemScr.thisItem = item;
                    itemScr.name = item.name;
                    itemScr.description = item.description;
                    itemScr.img = item.icon;
                    itemScr.Type = item.Type;
                    itemScr.haveItem = true;
                    Debug.Log("Equipo");
                }
                else
                {
                    Debug.Log("Nije Equipo");
                }
                break;
            case TypeOfEquipment.Chest:
                itemScr = slot[1].GetComponentInChildren<item>();
                if (itemScr.haveItem == false)
                {
                    armorManager.cooldown = false;
                    //  armorManager.playerCurrentArmor += item.HealthBoost;
                    armorManager.armorContainers += item.HealthBoost;
                  

                    armorManager.InitArmor(); armorManager.UpdateHearts  ();
                    armorManager.Changed(item.HealthBoost);
                    itemScr.equiped = true;
                    itemScr.thisItem = item;
                    itemScr.name = item.name;
                    itemScr.description = item.description;
                    itemScr.img = item.icon;
                    itemScr.Type = item.Type;
                    itemScr.haveItem = true;
                    Debug.Log("Equipo");
                }
                else
                {
                    Inventory.AddItem(itemScr.thisItem);
                    Knockback.damageBoost -= itemScr.thisItem.damageBoost;
                    //armorManager.playerCurrentArmor -= itemScr.thisItem.HealthBoost;
                    armorManager.cooldown = false;
                    armorManager.armorContainers -= itemScr.thisItem.HealthBoost;
                    // Debug.Log(armorManager.armorContainers);
                    //   armorManager.playerCurrentArmor += item.HealthBoost;
                    armorManager.armorContainers += item.HealthBoost;
                    armorManager.Changed(item.HealthBoost);
                    Debug.Log("obuko "+item.HealthBoost);


                    armorManager.InitArmor(); armorManager.UpdateHearts();
                    itemScr.equiped = true;
                    itemScr.thisItem = item;
                    itemScr.name = item.name;
                    itemScr.description = item.description;
                    itemScr.img = item.icon;
                    itemScr.Type = item.Type;
                    itemScr.haveItem = true;
                    Debug.Log("Swap Equipo");

                }
                break;
            case TypeOfEquipment.Boots:
                itemScr = slot[2].GetComponentInChildren<item>();
                if (itemScr.haveItem == false)
                {
                    RunningScr.maxSpeed += 4;
                    RunningScr.walkSpeed += 2;
                    itemScr.equiped = true;
                    itemScr.thisItem = item;
                    itemScr.name = item.name;
                    itemScr.description = item.description;
                    itemScr.img = item.icon;
                    itemScr.Type = item.Type;
                    itemScr.haveItem = true;
                    Debug.Log("Equipo");

                    DexterityText.text = 2.ToString() ;
                    runText.text = RunningScr.maxSpeed.ToString();
                    walkText.text = RunningScr.walkSpeed.ToString();
                }
                else
                {
                    Inventory.AddItem(itemScr.thisItem);
                    RunningScr.maxSpeed = 16;
                    RunningScr.walkSpeed =10;
                    Knockback.damageBoost -= itemScr.thisItem.damageBoost;
                    Debug.Log("obuko " + item.HealthBoost);
                    itemScr.equiped = true;
                    itemScr.thisItem = item;
                    itemScr.name = item.name;
                    itemScr.description = item.description;
                    itemScr.img = item.icon;
                    itemScr.Type = item.Type;
                    itemScr.haveItem = true;
                    RunningScr.maxSpeed += 4;
                    RunningScr.walkSpeed += 2;
                    Debug.Log("Swap Equipo");
                    DexterityText.text = 2.ToString();
                    runText.text = RunningScr.maxSpeed.ToString();
                    walkText.text = RunningScr.walkSpeed.ToString();

                }
                break;
            case TypeOfEquipment.Ring:
                itemScr = slot[3].GetComponentInChildren<item>();
                if (itemScr.haveItem == false)
                {
                    itemScr.equiped = true;
                    itemScr.thisItem = item;
                    itemScr.name = item.name;
                    itemScr.description = item.description;
                    itemScr.img = item.icon;
                    itemScr.Type = item.Type;
                    itemScr.haveItem = true;
                    Debug.Log("Equipo");
                }
                else
                {
                    Debug.Log("Nije Equipo");
                }
                break;
            case TypeOfEquipment.Bow:
                itemScr = slot[4].GetComponentInChildren<item>();
                if (itemScr.haveItem == false)
                {
                    PlayerScr.haveBow = true;
                    itemScr.equiped = true;
                    itemScr.thisItem = item;
                    itemScr.name = item.name;
                    itemScr.description = item.description;
                    itemScr.img = item.icon;
                    itemScr.Type = item.Type;
                    itemScr.haveItem = true;
                    Debug.Log("Equipo");
                }           
                else
                {
                    Inventory.AddItem(itemScr.thisItem);
                   // Knockback.damageBoost -= itemScr.thisItem.damageBoost;

                   // Knockback.damageBoost += item.damageBoost;
                   // PlayerScr.haveSword = true;
                    itemScr.equiped = true;
                    itemScr.thisItem = item;
                    itemScr.name = item.name;
                    itemScr.description = item.description;
                    itemScr.img = item.icon;
                    itemScr.Type = item.Type;
                    itemScr.haveItem = true;
                    Debug.Log("Swap Equipo");
                }





                break;
            default:
                Debug.Log("NO");
                break;
        }

    }
    /*
    public void AddItemInHand(CreateItem item)
    {



        switch (item.Type)
        {


            case TypeOfItem.Veggies:
                itemScr = slot[6].GetComponentInChildren<item>();
                if (itemScr.haveItem == false)
                {
                    itemScr.equiped = true;
                    itemScr.thisItem = item;
                    itemScr.name = item.name;
                    itemScr.counter1 = item.;
                    itemScr.description = item.description;
                    itemScr.img = item.icon;
                    itemScr.Type = item.Type;
                    itemScr.haveItem = true;
                    //Debug.Log("Equipo");
                }
                else
                {
                    Inventory.AddItem(itemScr.thisItem);
                    for (int i = 0; i < length; i++)
                    {

                    }
                    itemScr.thisItem = item;
                    itemScr.name = item.name;
                    itemScr.description = item.description;
                    itemScr.img = item.icon;
                    itemScr.Type = item.Type;
                    itemScr.haveItem = true;
                    Debug.Log("Swap Equipo");
                }

                break;
            
            default:
                Debug.Log("NO use");
                break;
        }

    }
    */












}
