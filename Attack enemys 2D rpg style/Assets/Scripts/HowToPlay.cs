using Pathfinding.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    public GameObject ThisGm;
    private float startTime = 0;
    bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(isActive==false)
        {
            startTime += 1f * Time.deltaTime;
            if(startTime >3f)
            {
                isActive = true;
                ThisGm.SetActive(true);
                Time.timeScale = 0;
            }
        }
        */
    }
  public  void TurnOffScreen()
    {
        //Debug.Log("duvug");
        ThisGm.SetActive(false);
        Time.timeScale = 1;
    }
}
