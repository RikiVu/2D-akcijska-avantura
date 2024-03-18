using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RunningScr : MonoBehaviour
{

    public Image img;
    public float lerpSpeed;
    public static float value = 1;
    float maxValue = 1;
    private Animator animator;
    float minValue = 0;
    bool coroutineStarted =false;
    float currentVel = 0;
    
    public static  float maxSpeed = 16;

    public static float walkSpeed = 10;
    [SerializeField]
    float depletionSpeed = .9f;
    [SerializeField]
    float chargingSpeed = .1f;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
       
         if(Input.GetKey(KeyCode.LeftShift) && PlayerScr.canRun)
         {
            PlayerScr.trci = true;
            value -= depletionSpeed * Time.deltaTime;
            PlayerScr.canRun = true;
            PlayerScr.speed = maxSpeed;
           
            if (value <= minValue)
            {
                if (!coroutineStarted)
                    StartCoroutine(RunningCooldown());
            }
        }
        else
        {
            PlayerScr.trci = false;
            PlayerScr.speed = 10;
            if (value < maxValue && PlayerScr.canRun)
            {
                value += chargingSpeed * Time.deltaTime;
                if (value >= maxValue)
                {
                    value = maxValue;
                }
            }
        }
       // img.fillAmount = Mathf.Lerp(img.fillAmount,value, Time.deltaTime * lerpSpeed *2);
        img.fillAmount = Mathf.SmoothDamp(img.fillAmount, value, ref currentVel, 10 *Time.deltaTime);
    }
    private IEnumerator RunningCooldown()
    {
        coroutineStarted = true;
        value = minValue;
        PlayerScr.canRun = false;
        yield return new WaitForSeconds(2.0f);
        PlayerScr.canRun = true;
        coroutineStarted = false;
    }
}
