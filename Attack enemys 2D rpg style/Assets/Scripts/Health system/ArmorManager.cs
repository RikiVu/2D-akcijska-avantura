using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

public class ArmorManager : MonoBehaviour
{
    public GameObject armor;
    public Image armorImage;
    public GameObject background;
    float startTime = 0;
   public float TimeLeft = 20f;
    public Animator animator;
    public TextMeshProUGUI tmText;
    public int armorContainers =0;
   // public int armorCurrent = 0;
    float lerpSpeed = 2;
   public bool cooldown = false;
   // int count = 0;
    public bool hasProtection = false;
    public bool activeProtection = false;

    void Start()
    {
        InitArmor();
    }
    // Start is called before the first frame update
    public void InitArmor()
    {
        armor.SetActive(false);
        background.SetActive(false);
        if (hasProtection)
        {
            Debug.Log("init protection");
            TimeLeft = 20f;
            armor.SetActive(true);
            cooldown = true;
            background.SetActive(true);
           
        }
    }
    public  void ArmorHit()
    {
        TimeLeft = 20f;
        activeProtection = false;
        StartCoroutine(WaitCo());
    }

    private void LateUpdate()
    {
        if(cooldown)
        {
            startTime += 0.05f * Time.deltaTime;
            TimeLeft -= 0.98f * Time.deltaTime;
            if (startTime <= 1.05f )
                {
                if(!activeProtection)
                {
                    int time =(int)TimeLeft;
                    armorImage.fillAmount = Mathf.Lerp(armorImage.fillAmount, startTime, Time.deltaTime * lerpSpeed);
                    tmText.text = time.ToString();
                }
            }
            else if(startTime>=1.06)
                {
                animator.SetTrigger("Recharged2");
                startTime = 0;
                tmText.text="";
                activeProtection = true;
                cooldown = false;
            }
        }
    }


    private IEnumerator WaitCo()
    {
        armorImage.fillAmount = 0;
        yield return new WaitForSeconds(2f);
        activeProtection = false;
        armorImage.fillAmount = Mathf.Lerp(armorImage.fillAmount, 0, Time.deltaTime * lerpSpeed);
        //tmText.text = "20";
        cooldown = true;
    }

}

