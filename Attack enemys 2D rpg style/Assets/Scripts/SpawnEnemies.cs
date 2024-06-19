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
    public static string defaultDifficulty = "Easy";

    //unused
    public static int currentMinionCount = 0;
    private float timer = 0f;
    private float minSpawnDistance = 5f;

    int i;
    GameObject randomSpawn;
    GameObject temp;
    int lenghtOfEnemies;
    int lenghtOfEnemiesMedium;
    int idToSpawn;

    private void Start()
    {
        //maxMinions = target.MaxCount;
        //  Debug.Log("enemies lenght "+ target.Enemies.Length);
        if (target.Enemies[0] != null)
        {
            lenghtOfEnemies = target.Enemies.Length;
        }
        if (target.EnemiesMedium[0] != null)
        {
            lenghtOfEnemiesMedium = target.Enemies.Length;
        }
    }

    public void newGameIntroSpawn()
    {
        StartCoroutine(disablenewGameIntroSpawn(10));
    }
    private IEnumerator disablenewGameIntroSpawn(float knockTime)
    {
        inRange = true;
        yield return new WaitForSeconds(knockTime);
        inRange = false;
        spawned = false;
        for (i = enemies.Count - 1; i >= 0; i--)
        {
            temp = enemies[i];
            enemies.RemoveAt(i);
            Destroy(temp);
        }
        enemies.Clear();
    }



    void Update()
    {
        if (!inRange || spawned)
            return;


        spawned = true;
        if(defaultDifficulty == "Easy")
        {
            for (i = 0; i < placeOfSpawn.Length - 1; i++)
            {
                // Instantiate enemy at the randomly selected spawn point
                idToSpawn = Random.Range(0, lenghtOfEnemies);
                temp = Instantiate(target.Enemies[idToSpawn], placeOfSpawn[i].transform.position, Quaternion.identity);
                enemies.Add(temp);
            }

        }
        else
        {
            for (i = 0; i < placeOfSpawn.Length - 1; i++)
            {
                // Instantiate enemy at the randomly selected spawn point
                idToSpawn = Random.Range(0, lenghtOfEnemiesMedium);
                temp = Instantiate(target.EnemiesMedium[idToSpawn], placeOfSpawn[i].transform.position, Quaternion.identity);
                enemies.Add(temp);
            }
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
                    Instantiate(target.Enemies[0], randomSpawn.transform.position, Quaternion.identity);
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
            Debug.Log("no longer in range");
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
