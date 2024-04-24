using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AdrenalinScr : MonoBehaviour
{

    public Image img;
    public float lerpSpeed;
  public static float value = 0;
    float maxValue = 1;
    private Animator animator;
    float minValue = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {

         if(Input.GetKeyDown(KeyCode.B))
       {
            value += 1;
       }
       

        //value = maxValue;
        img.fillAmount = Mathf.Lerp(img.fillAmount,value, Time.deltaTime * lerpSpeed *2);
      
        if (value >= maxValue)
        {
            value = maxValue;
            animator.SetBool("fullRage", true);
            PlayerScr.CanDash = true;
            
        }
         if(value < maxValue)
        {
            animator.SetBool("fullRage", false);
        }
        if(value < minValue)
        {
            value = minValue;
        }
       

    }
}
