using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
public class CoinScript : MonoBehaviour
{
    private bool collected = false;
    public static float bonusPerc;
    private float bonus;
    int randomGeneratedNum;
    private float lifetime = 15f;
    private float lifetimeSeconds;
    private void Start()
    {
        lifetimeSeconds = lifetime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && !collected)
        {
            collected = true;
              FindObjectOfType<AudioManager>().Play("coin");
            StartCoroutine(ChangeSize());
            randomGeneratedNum = Random.Range(5, 10);
            bonus = randomGeneratedNum * bonusPerc;
            PlayerScr.Gold += randomGeneratedNum + bonus;
            Destroy(this.gameObject);
        }
    }




    public IEnumerator ChangeSize()
    {
        transform.localScale += new Vector3(-0.2F, -0.2f, -0.2f);
        yield return new WaitForSeconds(30f); // 
        transform.localScale += new Vector3(0.0F, 0.0f, 0.0f);
    }
    private void Update()
    {
        lifetimeSeconds -= Time.deltaTime;
        if (lifetimeSeconds <= 0)
            Destroy(this.gameObject);
    }




}
