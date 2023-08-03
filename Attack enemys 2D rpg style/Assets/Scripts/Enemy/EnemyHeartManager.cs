using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHeartManager : MonoBehaviour
{
    /*
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    // public FloatValue heartContainers;
   //  public FloatValue enemyCurrentHealth;
      Enemy enemy;

 
    void Start()
    {

        InitHearts();
    }
    // Start is called before the first frame update
    public void InitHearts()
    {
        enemy.GetComponent<Enemy>();
        Debug.Log(enemy.dam);
        for (int i = 0; i < enemy.dam; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
    }
    public void UpdateHearts()
    {
        
        float tempHealth = enemy.bum / 2;
        for (int i = 0; i < enemy.dam; i++)
        {
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
    */
}
