using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LevelSystem : MonoBehaviour
{

    /*
    public Image img;
    public float lerpSpeed;

    int maxValue = 100;
    int minValue = 0;
    public static int value1 = 0;
   public  float value1float =0;
    public static int level = 1;
    public TextMeshProUGUI levelCount;
    bool levelUp = false;
    int resto = 0;
    public HeartManager hearthManager;
    */

    /*

    void FixedUpdate()
    {
        //value = maxValue;
       // Debug.Log(value1);

       // if(Input.GetKeyDown(KeyCode.B))
       // {
         //   value1 += 110;
       // }
       


       if (value1 >= maxValue)
       {
           levelUp = true;

           resto = value1 - maxValue;
           //level up;
           StartCoroutine(WaitCo());

           maxValue = 100;
           maxValue *= level;







       }
       else if(value1 < maxValue && !levelUp)
       {
           img.fillAmount = Mathf.Lerp(img.fillAmount, (float)value1 / 100 / level, Time.deltaTime * lerpSpeed * 2);
       }

       if(levelUp)
       {
           img.fillAmount = Mathf.Lerp(img.fillAmount, 1f, Time.deltaTime * lerpSpeed * 2);
           FindObjectOfType<AudioManager>().Play("LevelUP");
           if (img.fillAmount>=0.98f)
           {

               levelCount.text = level.ToString();

           }

       }

   }



   private IEnumerator WaitCo()
   {


       level++;
       yield return new WaitForSeconds(3f);
       value1 = resto;
       levelUp = false;
       if (level%3==0)
       {
           hearthManager.playerCurrentHealth.initialValue += 1;
           hearthManager.heartContainers.initialValue += 1 / 2f;
           hearthManager.playerCurrentHealth.RuntimeValue = hearthManager.playerCurrentHealth.initialValue;

           hearthManager.InitHearts(); hearthManager.UpdateHearts();
       }



   }
           */
}
