using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOutline : MonoBehaviour
{
    public SpriteRenderer rend;
    bool FlashActive = true;
    private float flashLenght = 1f;
    private float flashCounter = 0f;
    float time = 0;


    public void ShowOutlineOnItems()
    {
        
            flash();
        
        rend.material.EnableKeyword("TURN_ON");
       // flash();
          //  rend.material.SetColor("_Outline_color", new Color(233f / 255f, 79f / 255f, 55f / 255f));      

    }

    public void HideOutlineOnItems()
    {

        rend.material.DisableKeyword("TURN_ON");
    }
    private void flash() // kad ga udari da mjenja kao boju 
    {
        if (FlashActive)
        {
            if (flashCounter > flashLenght * .99f)
            {
                rend.material.SetColor("_Outline_color", new Color(0.749707f, 1f, 0.4862745f));
            }
            else if (flashCounter > flashLenght * .87f)
            {
                rend.material.SetColor("_Outline_color", new Color(0.4862745f , 1f, 0.5479895f));

            }
            else if (flashCounter > flashLenght * .75f)
            {
                rend.material.SetColor("_Outline_color", new Color(0.4862745f, 1f, 0.870891f));

            }
            else if (flashCounter > flashLenght * .63f)
            {
                rend.material.SetColor("_Outline_color", new Color(0.4862745f, 0.8133345f, 1f));

            }
            else if (flashCounter  > flashLenght * .51f)
            {
                rend.material.SetColor("_Outline_color", new Color(0.4862745f, 0.5493107f, 1f));

            }
            else if (flashCounter  > flashLenght * .39f)
            {
                rend.material.SetColor("_Outline_color", new Color(0.69573f, 0.4862745f, 1f));

            }
            else if (flashCounter > flashLenght * .27f)
            {
                rend.material.SetColor("_Outline_color", new Color(1f, 0.4862745f, 0.7056287f));

            }
            else if (flashCounter > flashLenght * .15f)
            {
                rend.material.SetColor("_Outline_color", new Color(1f, 0.6022147f, 0.4862745f));

            }
           
            else if (flashCounter  > 0f)
            {
                rend.material.SetColor("_Outline_color", new Color(1f, 0.4862745f, 1f));

            }
            else
            {


                // rend.material.SetColor("_Outline_color", new Color(0.9797369f, 0.8845241f, 0.4862745f));
               
                flashCounter = flashLenght;
             //    FlashActive = false;

            }
            flashCounter -= Time.deltaTime;
            
        }
        else
        {
            rend.material.SetColor("_Outline_color", new Color(1f, 1f, 0.6f));
            time += 0.1f;
            if(time>=2f)
            {
                FlashActive = true;
                time = 0;
            }
        }
    }

}