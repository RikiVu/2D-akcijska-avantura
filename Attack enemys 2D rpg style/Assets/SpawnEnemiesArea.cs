using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Progress;

public class SpawnEnemiesArea : MonoBehaviour
{
    public static int currentMinionCount=0;
    public GameObject prefab;
    public Transform Player;
    public GameObject[] placeOfSpawn;
    private bool inRange = false;
    List<Vector3> vec = new List<Vector3>();
    private int maxMinions = 5;
    
    int i = 0;
    private bool temp = false;
    Vector3 temp1;
    // Update is called once per frame
    void Update()
    {
        if (!inRange)
            return;
       
        if (!temp)
        {

            foreach (GameObject gm in placeOfSpawn)
            {
                Debug.Log(gm.transform.position - Player.position  + " ;;;" + i );
                Instantiate(prefab, placeOfSpawn[i].transform.position, Quaternion.identity);
                //temp1 = gm.transform.position - Player.position;
                i++;
                currentMinionCount++;
            }
            
            temp = true;
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
