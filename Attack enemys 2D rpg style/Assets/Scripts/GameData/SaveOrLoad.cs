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
    public AudioSource audioSource;
    public AudioClip songToPlay;

    public static string currentGameRecord = "";

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
        try
        {
            GameData data = new GameData();
            filePath = currentGameRecord;
            string json ="";
            json = File.ReadAllText(filePath);
            if (File.Exists(filePath))
            {
                data = JsonUtility.FromJson<GameData>(json);
                File.Delete(filePath);
            }
            else
            {
                Debug.LogWarning("File not found: " + filePath);
            }
            Debug.Log(currentGameRecord);
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
            data.pots = potlist;
            data.canPass = passage;
            data.pickUpItems = pickUpItemList;
            data.quests = questObject;
            data.bossDefeated = boss.bossDefeated;
            json = JsonUtility.ToJson(data, true);
            File.WriteAllText(currentGameRecord, json);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error while trying to save! " + e);
        }
       
    }
    public void SavetoJson(Vector3 playerPosition, bool godmode, float health, float gold, int arrows, int stars, List<ItemObject> items, List<CreateItem> equipment,
        List<ChestObject> chestList, List<PotObject> potlist, bool passage, List<ItemsOnGroundObject> pickUpItemList, List<QuestObjectLog> questObject, BossAi boss, string recordName, string diff)
    {
        try
        {
            GameData data = new GameData();
            data.spawnPosition = playerPosition;
            data.godMode = godmode;
            data.difficulty = diff;
            //add to adjust to diff
            data.currentHealth = health;
            data.gold = gold;
            data.arrows = arrows;
            data.stars = stars;
            data.items = items;
            data.equipment = equipment;
            data.camMaxPosition = mapScrObject.maxPosition;
            data.camMinPosition = mapScrObject.minPosition;
            data.chests = chestList;
            data.pots = potlist;
            data.canPass = passage;
            data.pickUpItems = pickUpItemList;
            data.quests = questObject;
            data.bossDefeated = boss.bossDefeated;
            timestamp = System.DateTime.Now.ToString();
            timestamp = ConvertToFileName(timestamp);
            if (recordName != "")
            {
                data.recordName = recordName;
                string json = JsonUtility.ToJson(data, true);
                currentGameRecord = Application.dataPath + "/saves/" + data.recordName + ".json";
                File.WriteAllText(Application.dataPath + "/saves/" + data.recordName + ".json", json);
            }
            else
            {
                data.recordName = timestamp;
                string json = JsonUtility.ToJson(data, true);
                currentGameRecord = Application.dataPath + "/saves/" + timestamp + ".json";
                File.WriteAllText(Application.dataPath + "/saves/" + timestamp + ".json", json);
            }
           
        }
        catch (Exception e)
        {
            alertPanelScr.showAlertPanel("Failed to start a new game!");
        }
    }

    public void NewGame(string recordname,bool godmode, Diff dif)
    {
        if (!saved && !loading)
        {
            try
            {
                audioSource.clip = songToPlay;
                audioSource.Play();
                manager.redirect_Quest.saveToManager();
                SavetoJson(player.transform.position, godmode, HeartManager.playerCurrentHealth, PlayerScr.Gold,
                    PlayerScr.Arrows, Inventory.starCount, inventory.SaveInventory(), equipment.SaveEquipment(),
                    manager.chestList, manager.potList, AllowPassage.CanPass, manager.pickupList, manager.questObjectLogList, Boss, recordname, dif.ToString());
                StartCoroutine(Saving2());
                //StartCoroutine(Loading(recordname, godmode, dif.ToString()));
            }
            catch (Exception e)
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
                StartCoroutine(Loading(name));
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
                {
                    StartCoroutine(waitToLeave());
                    audioSource.clip = songToPlay;
                    audioSource.Play();
                }
              
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
    private IEnumerator Loading(string name)
    {
        loading = true;
       
        player.triggerBox.enabled = false;
        yield return Frames(2);
        player.triggerBox.enabled = true;
        string json;
        alertPanelScr.showAlertPanel("Loaded");
        json = File.ReadAllText(Application.dataPath + "/saves/" + name);
        filePath = Application.dataPath + "/saves/" + name;
        currentGameRecord = filePath;
        
        if (File.Exists(filePath))
        {
            GameData data = JsonUtility.FromJson<GameData>(json);
            player.transform.position = data.spawnPosition;
            PlayerScr.GodMode = data.godMode;
            //record name 
            // Diff
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
            SpawnEnemies.defaultDifficulty = data.difficulty;
            
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
