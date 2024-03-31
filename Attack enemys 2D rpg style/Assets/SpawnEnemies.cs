using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{

    public CreateMap target;
    private int maxMinions;
    public Transform Player;
    public GameObject[] placeOfSpawn;
    [SerializeField]
    private bool inRange = false;
    [SerializeField]
    private bool spawned = false;
    public List<GameObject> enemies = new List<GameObject>();

    //unused
    public static int currentMinionCount = 0;
    private float timer = 0f;
    private float minSpawnDistance = 5f;

    int i;
    GameObject randomSpawn;
    GameObject temp;

    private void Start()
    {
        //maxMinions = target.MaxCount;
    }

    void Update()
    {
        if (!inRange || spawned)
            return;

        spawned = true;

        for ( i = 0; i<  placeOfSpawn.Length-1; i++)
        {
            // Instantiate enemy at the randomly selected spawn point
            temp = Instantiate(target.Enemy, placeOfSpawn[i].transform.position, Quaternion.identity);
            enemies.Add(temp);
        }
          
    }
    

    public void testWithTime()
    {
        timer += Time.deltaTime;

        // Check if 2 seconds have passed
        if (timer >= target.RespawnTime)
        {
            if (currentMinionCount < maxMinions)
            {
                // Find a random spawn point that is not in close proximity to the player
                GameObject randomSpawn = GetRandomSpawnPointNotCloseToPlayer();

                if (randomSpawn != null)
                {
                    // Instantiate prefab at the selected spawn point
                    Instantiate(target.Enemy, randomSpawn.transform.position, Quaternion.identity);
                    currentMinionCount++;
                    timer = 0f;
                    Debug.Log(currentMinionCount);
                }
            }
        }
    }

    private GameObject GetRandomSpawnPointNotCloseToPlayer()
    {
        // Filter spawn points that are not in close proximity to the player
        var validSpawnPoints = placeOfSpawn.Where(spawn =>
        {
            float distance = Vector2.Distance(Player.position, spawn.transform.position);
            return distance > minSpawnDistance;
        }).ToArray();

        // If there are valid spawn points, return a random one; otherwise, return null
        if (validSpawnPoints.Length > 0)
        {
            return validSpawnPoints[UnityEngine.Random.Range(0, validSpawnPoints.Length)];
        }
        else
        {
            return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger && !inRange)
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger && inRange)
        {
            inRange = false;
            spawned = false;
            for(i = enemies.Count -1; i >=0 ; i--)
            {
                temp = enemies[i];
                enemies.RemoveAt(i);
                Destroy(temp);
            }
            enemies.Clear();
            }
        }
}
