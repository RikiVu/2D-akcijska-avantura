using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ArmorManager : MonoBehaviour
{
    public Image[] armors;
    public Sprite armor;
    float startTime = 0;
   public float TimeLeft = 20f;
    public Animator[] animator;
    public TextMeshProUGUI[] tmText;
    public int armorContainers =0;
    public int armorCurrent = 0;
    float lerpSpeed = 2;
   public bool cooldown = false;
    int count = 0;

    void Start()
    {
   

        armorContainers = 0;
      //  playerCurrentArmor = 0;
        InitArmor();
    }
    // Start is called before the first frame update
    public void InitArmor()
    {
        for (int x = 0; x < armors.Length; x++)
        {
            armors[x].gameObject.SetActive(false);
           // armors[x].sprite = armor;

        
        }
       // armorCurrent = armorContainers;
        for (int i = 0; i < armorContainers; i++)
        {
            armors[i].gameObject.SetActive(true);
            armors[i].sprite = armor;
        }

    }
    public void UpdateHearts()
    {
       // armorCurrent = armorContainers;
        //  float tempHealth = playerCurrentArmor;
        for (int i = 0; i < armorContainers; i++)
        {
            if (i <= armorContainers - 1)
            {
                //full health
                armors[i].sprite = armor;
            }
            else if (i >= armorContainers)
            {
                //emptyHeart
             //   hearts[i].sprite = emptyHeart;
            }
            else
            {
                //halfHeart
            //    hearts[i].sprite = halfHeart;
            }
        }
    }

    public  void ArmorHit()
    {
        armorCurrent -= 1;
        Debug.Log(armorCurrent);
        StartCoroutine(WaitCo());
    }


    private void ArmorCooldown(int element)
    {
      //  startTime += 0.1f * Time.deltaTime;
       // armors[armorContainers - armorCurrent+1].fillAmount = Mathf.Lerp(armors[armorContainers -armorCurrent+1].fillAmount, startTime, Time.deltaTime * lerpSpeed);
    }

    private void LateUpdate()
 {
        

        if(cooldown)
        {
          //  Debug.Log("container "+armorContainers);
          //  Debug.Log("armor current "+armorCurrent);
          //  Debug.Log("cout -- "+count);


            startTime += 0.05f * Time.deltaTime;
            TimeLeft -= 0.98f * Time.deltaTime;
            if (startTime <= 1.05f )
                {
                if(count > 0)
                {
                    if(count>armorContainers)
                    {
                        count--;
                    }
                    int time =(int)TimeLeft;
                   // Debug.Log(armorContainers - count);
                    armors[armorContainers - count].fillAmount = Mathf.Lerp(armors[armorContainers - count].fillAmount, startTime, Time.deltaTime * lerpSpeed);
                    tmText[armorContainers - count].text = time.ToString();
                }
                 

               
               
            }


            else if(startTime>=1.06)
                {
                animator[armorContainers - count].SetBool("Recharged", true);
                startTime = 0;
                
                TimeLeft = 20f;
                tmText[armorContainers - count].text="";
                armorCurrent += 1;
                      count--;
                WaitCo2();


            }
                if(count==0)
                  {
                cooldown = false;
                 }
           

        }
        if (count > 0)
        {
            for (int i = 1; i <= count; i++)
            {
                //armors[armorCurrent +1].fillAmount = 0;
                armors[armorCurrent + 1].fillAmount = Mathf.Lerp(armors[armorCurrent + 1].fillAmount, 0, Time.deltaTime * lerpSpeed);
                tmText[armorContainers + 1].text = "20";
            }
        }
    }


    private IEnumerator WaitCo()
    {

        armors[armorCurrent].fillAmount = 0;
        yield return new WaitForSeconds(2f);
        count++;
        cooldown = true;


   
        


    }
    private IEnumerator WaitCo2()
    {

        animator[armorContainers - count].SetBool("Recharged", false);
        yield return new WaitForSeconds(1f);
       
    }
    public void Changed(int boost)
    {
        cooldown = false;      
        armorCurrent = 0;
        for (int i =0; i<boost; i++)
        {
            armors[i].fillAmount = 0;
            tmText[i].text = "20";
        }
        count = boost;
        cooldown = true;

    }


}
