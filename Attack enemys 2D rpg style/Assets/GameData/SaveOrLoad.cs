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


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScr>();
        cam = Camera.main.GetComponent<CameraMovement>();
    }
    public void SavetoJson(Vector3 playerPosition, bool godmode, float health, float gold, int arrows, int stars, List<CreateItem> items, List<CreateItem> equipment)
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
        data.map= mapScrObject;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/gameData.json", json);
    }

    public void LoadFromJson()
    {
        loading = true;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(Application.dataPath + "/gameData.json");
            GameData data = JsonUtility.FromJson<GameData>(json);
            player.transform.position = data.spawnPosition;
            PlayerScr.GodMode = data.godMode;
            HeartManager.playerCurrentHealth = data.currentHealth;
            PlayerScr.Gold = data.gold;
            PlayerScr.Arrows = data.arrows;
            //player.loadPlayer();
            Inventory.starCount = data.stars;
            inventory.LoadInventory(data.items);
            equipment.LoadEquipment(data.equipment);
            cam.MapTransfer(data.map.minPosition, data.map.maxPosition);
            alertPanelScr.showAlertPanel("Loaded");
        }
        else
        {
            Debug.LogError("There are no save files");
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!saved && !loading)
        {
            SavetoJson(player.transform.position, PlayerScr.GodMode, HeartManager.playerCurrentHealth, PlayerScr.Gold, PlayerScr.Arrows, Inventory.starCount, inventory.SaveInventory(), equipment.SaveEquipment());
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
    private IEnumerator waitToLeave()
    {
        waitingCoro = true;
        yield return new WaitForSeconds(3f);
        loading = false;
        waitingCoro = false;
    }

}
