using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class time : MonoBehaviour
{
    public float Day = 120;
    public float Noc = 90;
    private float lifetimeSeconds = -01;
    bool jutro = true;

    public Image image;

    void Start()
    {

        
        /*
         *          0.30f Jutro
         *          0f    Zenit
         *          0.25f Posljepodne
         *          0.50f Navecer
         *          0.60f  Noc
         *          
         *        Krene od  0.30f do 0f i tuu kad dode krene prema 0.60
         * 
         */ 
    }
    private void Update()
    {
        image = GetComponent<Image>();
        var tempColor = image.color;
        
        if (lifetimeSeconds >= 0 && jutro == false)
        {
            lifetimeSeconds -= Time.deltaTime;
            
            tempColor.a = lifetimeSeconds / 100;
            image.color = tempColor;
            Day = 120;
            Noc = 90;

        }
        if (lifetimeSeconds < 0)
        {
            jutro = true;
            Day -= Time.deltaTime;
            
            
        }

        if (jutro && Day <=0)
        {
            
            if (lifetimeSeconds >= 50)
            {
                Noc -= Time.deltaTime;
                if (Noc <= 0)
                jutro = false;
            }
            else
            {
                lifetimeSeconds += Time.deltaTime;
                tempColor.a = lifetimeSeconds / 100;
                image.color = tempColor;
            }
           
            
        }
       





    }
}
