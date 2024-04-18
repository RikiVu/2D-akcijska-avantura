using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class SaveOrLoad : MonoBehaviour
{
    private PlayerScr player;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private Equipment equipment;
    [SerializeField]
    private bool saved = false;
    List<CreateItem> itemsTemp;
    List<CreateItem> equipmentTemp;
    string filePath = Application.dataPath + "/gameData.json";
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AlertPanelScr alertPanelScr;
    public static bool loading = false;
    private bool waitingCoro = false;
    [SerializeField]
    private CreateMap mapScrObject;
    private CameraMovement cam;
    public GameManager manager;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScr>();
        cam = Camera.main.GetComponent<CameraMovement>();
    }
    public void SavetoJson(Vector3 playerPosition, bool godmode, float health, float gold, int arrows, int stars, List<CreateItem> items, List<CreateItem> equipment,
        List<ChestObject> chestList, List<PlantAndHarvestObject> plantListPar, List<PotObject> potlist, bool passage, List<ItemsOnGroundObject> pickUpItemList, List<QuestObjectLog> questObject)
    {
        GameData data = new GameData();
        data.spawnPosition = playerPosition;
        data.godMode = godmode;
        data.currentHealth = health;
        data.gold = gold;
        data.arrows = arrows;
        data.stars = stars;
        data.items = items;
        data.equipment = equipment;
        data.camMaxPosition= mapScrObject.maxPosition;
        data.camMinPosition = mapScrObject.minPosition;
        data.chests = chestList;
        data.plantList = plantListPar;
        data.pots = potlist;
        data.canPass = passage;
        data.pickUpItems = pickUpItemList;
        data.quests = questObject;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/gameData.json", json);
    }

    public void LoadFromJson()
    {
        if (!loading)
            StartCoroutine(Loading());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!saved && !loading)
        {
            SavetoJson(player.transform.position, PlayerScr.GodMode, HeartManager.playerCurrentHealth, PlayerScr.Gold,
                PlayerScr.Arrows, Inventory.starCount, inventory.SaveInventory(), equipment.SaveEquipment(),
                manager.chestList, manager.plantList, manager.potList, AllowPassage.CanPass, manager.pickupList ,manager.questObjectLogList) ;
            StartCoroutine(Saving());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (loading)
        {
            if(!waitingCoro)
                StartCoroutine(waitToLeave());
        }
    }

    private IEnumerator Saving()
    {
        saved = true;
        animator.SetBool("canSave", false);
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("saved");
        alertPanelScr.showAlertPanel("Saved");
        yield return new WaitForSeconds(30f);
        animator.SetBool("canSave", true);
        saved = false;
    }
    private IEnumerator Loading()
    {
        loading = true;
        player.triggerBox.enabled = false;
        yield return Frames(2);
        player.triggerBox.enabled = true;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(Application.dataPath + "/gameData.json");
            GameData data = JsonUtility.FromJson<GameData>(json);
            player.transform.position = data.spawnPosition;
            PlayerScr.GodMode = data.godMode;
            HeartManager.playerCurrentHealth = data.currentHealth;
            PlayerScr.Gold = data.gold;
            PlayerScr.Arrows = data.arrows;
            Inventory.starCount = data.stars;
            inventory.LoadInventory(data.items);
            equipment.LoadEquipment(data.equipment);
            cam.MapTransfer(data.camMinPosition, data.camMaxPosition);
            manager.loadChests(data.chests);
            manager.loadPlant(data.plantList);
            manager.loadPots(data.pots);
            manager.Passage(data.canPass);
            manager.loadPickUpItems(data.pickUpItems);
            manager.loadQuests(data.quests);
            alertPanelScr.showAlertPanel("Loaded");
        }
        else
        {
            Debug.LogError("There are no save files");
        }
  
       
    }
    public static IEnumerator Frames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }
    private IEnumerator waitToLeave()
    {
        waitingCoro = true;
        yield return new WaitForSeconds(3f);
        loading = false;
        waitingCoro = false;
    }

}
