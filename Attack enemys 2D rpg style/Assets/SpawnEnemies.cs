using System.Collections;
using System.Linq;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public static int currentMinionCount = 0;
    public GameObject prefab;
    public Transform Player;
    public GameObject[] placeOfSpawn;
    private bool inRange = false;
    private int maxMinions = 10;

    private float timer = 0f;
    private float interval = 2.5f;

    private float minSpawnDistance = 5f;

    void Update()
    {
        if (!inRange)
            return;

        timer += Time.deltaTime;

        // Check if 2.5 seconds have passed
        if (timer >= interval)
        {
            if (currentMinionCount < maxMinions)
            {
                // Find a random spawn point that is not in close proximity to the player
                GameObject randomSpawn = GetRandomSpawnPointNotCloseToPlayer();

                if (randomSpawn != null)
                {
                    // Instantiate prefab at the selected spawn point
                    Instantiate(prefab, randomSpawn.transform.position, Quaternion.identity);
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
        }
    }
}
