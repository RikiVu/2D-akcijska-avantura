using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HeartManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public float setPlayerMaxHealth = 9;
    //public float setPlayerCurrentHealth;
    public static float playerMaxHealth;
    public static float playerCurrentHealth;
    float tempHealth;
    public TextMeshProUGUI heartsCountText;
    public TextMeshProUGUI heartsCountText2;
    //public FloatValue heartContainers;
    //public FloatValue playerCurrentHealth;

    void Start()
    {
        playerMaxHealth = setPlayerMaxHealth ;
        playerCurrentHealth = playerMaxHealth;
        heartsCountText.text = (playerMaxHealth / 2).ToString();
        heartsCountText2.text = " = " + (playerMaxHealth).ToString() + " hitpoints";
        InitHearts();
    }

    public void constLogic(int constitution)
    {
        if(constitution == 0)
        {
            playerMaxHealth = setPlayerMaxHealth;
        }
        else
        {
            playerMaxHealth = setPlayerMaxHealth + (constitution + constitution);
        }
        heartsCountText.text = (playerMaxHealth / 2).ToString();
        heartsCountText2.text = " = "+ (playerMaxHealth).ToString() + " hitpoints";

        InitHearts();
    }
    // Start is called before the first frame update
    public void InitHearts()
    {
        if(playerCurrentHealth>playerMaxHealth)
        {
            playerCurrentHealth = playerMaxHealth;
        }
        for (int i = 0; i < 10; i++)
        {
            hearts[i].gameObject.SetActive(false);
            if (i <= tempHealth - 1)
            {
                //full health
                hearts[i].sprite = fullHeart;
            }
            else if (i >= tempHealth)
            {
                //emptyHeart
                hearts[i].sprite = emptyHeart;
            }
            else
            {
                //halfHeart
                hearts[i].sprite = halfHeart;
            }
        }
        tempHealth = playerCurrentHealth / 2;
        // Debug.Log((playerMaxHealth / 2.0f));
        for (int i = 0; i < playerMaxHealth / 2.0; i++)
        {
            hearts[i].gameObject.SetActive(true);
            //Debug.Log(i + ": " + (playerCurrentHealth - 1));
            if (i <= tempHealth - 1)
            {
                //full health
                hearts[i].sprite = fullHeart;
            }
            else if (i >= tempHealth)
            {
                //emptyHeart
                hearts[i].sprite = emptyHeart;
            }
            else
            {
                //halfHeart
                hearts[i].sprite = halfHeart;
            }

        }
    }
    public void UpdateHearts()
    {
         tempHealth = playerCurrentHealth / 2;
            for (int i = 0; i < playerMaxHealth / 2; i++)
            {
            if (i <= tempHealth-1)
            {
                //full health
                hearts[i].sprite = fullHeart;
            }
            else if(i >= tempHealth)
                {
                //emptyHeart
                hearts[i].sprite = emptyHeart;
            }
            else
            {
                //halfHeart
                hearts[i].sprite = halfHeart;
            }
        }
    }

}
