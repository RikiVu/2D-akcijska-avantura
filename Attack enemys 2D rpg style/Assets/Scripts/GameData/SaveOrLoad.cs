using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Globalization;

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
    string filePath; //= Application.dataPath + "/saves/gameData.json";
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
    public BossAi Boss;
    public bool canSave = true;
    private string timestamp = "";
    private string currentGameRecord = "";

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScr>();
        cam = Camera.main.GetComponent<CameraMovement>();
    }
    public static string ConvertToFileName(string dateTimeString)
    {
        
        DateTime dateTime = DateTime.ParseExact(dateTimeString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

        string fileName = dateTime.ToString("yyyy-MM-dd_HH-mm-ss");

        return fileName;
    }
    public void SavetoJson(Vector3 playerPosition, bool godmode, float health, float gold, int arrows, int stars, List<ItemObject> items, List<CreateItem> equipment,
        List<ChestObject> chestList,  List<PotObject> potlist, bool passage, List<ItemsOnGroundObject> pickUpItemList, List<QuestObjectLog> questObject, BossAi boss)
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
        //data.plantList = plantListPar;
        data.pots = potlist;
        data.canPass = passage;
        data.pickUpItems = pickUpItemList;
        data.quests = questObject;
        data.bossDefeated= boss.bossDefeated;
        string json = JsonUtility.ToJson(data, true);
        timestamp = System.DateTime.Now.ToString();
        timestamp = ConvertToFileName(timestamp);
        Debug.Log(currentGameRecord);
        if (currentGameRecord != "new_game.json")
        {
            filePath = Application.dataPath + "/saves/" + currentGameRecord;
            if (File.Exists(filePath))
            {
                // Delete the previous record
                File.Delete(filePath);
                Debug.Log("Deleted file: " + filePath);
            }
            else
            {
                Debug.LogWarning("File not found: " + filePath);
            }
        }
        currentGameRecord = Application.dataPath + "/saves/" + timestamp+".json";
        
        Debug.Log(timestamp);
        File.WriteAllText(Application.dataPath + "/saves/"+timestamp+".json", json);
    }
    /*
    public void SavetoJson(Vector3 playerPosition, bool godmode, float health, float gold, int arrows, int stars, List<CreateItem> items, List<CreateItem> equipment,
    List<ChestObject> chestList, List<PotObject> potlist, bool passage, List<ItemsOnGroundObject> pickUpItemList, List<QuestObjectLog> questObject, BossAi boss,string name)
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
        data.camMaxPosition = mapScrObject.maxPosition;
        data.camMinPosition = mapScrObject.minPosition;
        data.chests = chestList;
        //data.plantList = plantListPar;
        data.pots = potlist;
        data.canPass = passage;
        data.pickUpItems = pickUpItemList;
        data.quests = questObject;
        data.bossDefeated = boss.bossDefeated;
        string json = JsonUtility.ToJson(data, true);
        currentGameRecord = Application.dataPath + "/saves/" + name + ".json";
        File.WriteAllText(Application.dataPath + "/saves/" + name + ".json", json);
    }
    */
    /* //old
    public void NewGame()
    {
        if (!saved && !loading)
        {
            try
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScr>();
                cam = Camera.main.GetComponent<CameraMovement>();
                manager.redirect_Quest.saveToManager();
                SavetoJson(player.transform.position, PlayerScr.GodMode, HeartManager.playerCurrentHealth, PlayerScr.Gold,
                    PlayerScr.Arrows, Inventory.starCount, inventory.SaveInventory(), equipment.SaveEquipment(),
                    manager.chestList, manager.potList, AllowPassage.CanPass, manager.pickupList, manager.questObjectLogList, Boss, "new_game");
                StartCoroutine(Saving2());
            }
            catch (Exception e)
            {
                Debug.Log("failed to save!: " + e);
                alertPanelScr.showAlertPanel("Failed to save!");
            }
        }
    }
    */
    public void NewGame()
    {
        if (!saved && !loading)
        {
            try
            {
                if (!loading)
                    StartCoroutine(Loading("new_game.json",true));
            }
            catch
            {
                Debug.Log("failed to new game!");
                alertPanelScr.showAlertPanel("Failed to start a new game!");
            }
        }
    }


    public void LoadFromJson(string name)
    {
        try
        {
            if (!loading)
                StartCoroutine(Loading(name,false));
        }
        catch
        {
            Debug.Log("failed to load!");
            alertPanelScr.showAlertPanel("Failed to load!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!saved && !loading && canSave)
        {
            try
            {
                manager.redirect_Quest.saveToManager();
                SavetoJson(player.transform.position, PlayerScr.GodMode, HeartManager.playerCurrentHealth, PlayerScr.Gold,
                    PlayerScr.Arrows, Inventory.starCount, inventory.SaveInventory(), equipment.SaveEquipment(),
                    manager.chestList, manager.potList, AllowPassage.CanPass, manager.pickupList, manager.questObjectLogList, Boss);
                StartCoroutine(Saving());
            }
            catch
            {
                Debug.Log("failed to save!");
                alertPanelScr.showAlertPanel("Failed to save!");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (loading)
        {
            try
            {
                if (!waitingCoro)
                    StartCoroutine(waitToLeave());
            }
            catch
            {
                Debug.Log("Coroutine couldn't be started because the the game object 'spawn_0(1)' is inactive!");
            }
        
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
    private IEnumerator Saving2()
    {
        saved = true;
        animator.SetBool("canSave", false);
        yield return new WaitForSeconds(1f);
        //animator.SetTrigger("saved");
        //alertPanelScr.showAlertPanel("Saved");
        yield return new WaitForSeconds(10f);
        animator.SetBool("canSave", true);
        saved = false;
    }
    private IEnumerator Loading(string name,bool newgame)
    {
        loading = true;
        player.triggerBox.enabled = false;
        yield return Frames(2);
        player.triggerBox.enabled = true;
        string json;
        if (!newgame)
        {
            alertPanelScr.showAlertPanel("Loaded");
            json = File.ReadAllText(Application.dataPath + "/saves/" + name);
            filePath = Application.dataPath + "/saves/" + name;
        }
        else
        {
            json = File.ReadAllText(Application.dataPath + "/" + name);
            filePath = Application.dataPath + "/" + name;
        }
            currentGameRecord = name;
        //treba ak je novi napravit
        if (File.Exists(filePath))
        {
            GameData data = JsonUtility.FromJson<GameData>(json);
            player.transform.position = data.spawnPosition;
            PlayerScr.GodMode = data.godMode;
            equipment.LoadEquipment(data.equipment);
            HeartManager.playerCurrentHealth = data.currentHealth;
            PlayerScr.Gold = data.gold;
            PlayerScr.Arrows = data.arrows;
            Inventory.starCount = data.stars;
            inventory.LoadInventory(data.items);
                
            cam.MapTransfer(data.camMinPosition, data.camMaxPosition);
            manager.loadChests(data.chests);
            //manager.loadPlant(data.plantList);
            manager.loadPots(data.pots);
            manager.Passage(data.canPass);
            manager.loadPickUpItems(data.pickUpItems);
            manager.loadQuests(data.quests);
            player.loadPlayer();
            Boss.Load(data.bossDefeated);
               
                   
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
        yield return new WaitForSeconds(2f);
        loading = false;
        waitingCoro = false;
    }

}
