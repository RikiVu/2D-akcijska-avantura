using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class SaveOrLoad : MonoBehaviour
{
    private PlayerScr player;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private bool saved = false;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScr>();
    }
    public void SavetoJson(Vector3 playerPosition, bool godmode, float health, float gold, int arrows, int stars)
    {
        GameData data = new GameData();
        data.spawnPosition = playerPosition;
        data.godMode = godmode;
        data.currentHealth = health;
        data.gold = gold;
        data.arrows = arrows;
        data.stars = stars;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/gameData.json", json);
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/gameData.json");
        GameData data = JsonUtility.FromJson<GameData>(json);
        player.transform.position = data.spawnPosition;
        PlayerScr.GodMode = data.godMode;
        HeartManager.playerCurrentHealth = data.currentHealth;
        PlayerScr.Gold= data.gold;
        PlayerScr.Arrows = data.arrows;
        //player.loadPlayer();
        Inventory.starCount = data.stars;
        inventory.loadInventory();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!saved)
        {
            SavetoJson(player.transform.position, PlayerScr.GodMode, HeartManager.playerCurrentHealth, PlayerScr.Gold, PlayerScr.Arrows, Inventory.starCount);
            saved = true;
        }
    }

}
