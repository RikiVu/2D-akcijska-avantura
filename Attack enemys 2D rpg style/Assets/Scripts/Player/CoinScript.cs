using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
public class CoinScript : MonoBehaviour
{
    private bool collected = false;


 
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && !collected)
        {
            collected = true;
              FindObjectOfType<AudioManager>().Play("coin");
            StartCoroutine(ChangeSize());
            PlayerScr.Gold += 10;
            Debug.Log("something");
            Destroy(this.gameObject);
        }
    }




    public IEnumerator ChangeSize()
    {
        transform.localScale += new Vector3(-0.2F, -0.2f, -0.2f);
        yield return new WaitForSeconds(30f); // 
        transform.localScale += new Vector3(0.0F, 0.0f, 0.0f);
       
        
     
    }

   


}
