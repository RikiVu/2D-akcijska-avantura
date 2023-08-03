using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBack : MonoBehaviour
{
    public GameObject GameManager;
    public Animator trans;
    public string sceneToLoad;
    public Vector2 playerPostition;
    public VectorValue playerStorage;
 //   public GameObject Player;

        // za mjenjanje scena kada triggeramo

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
           
          //  DontDestroyOnLoad(Player.gameObject);
         //   DontDestroyOnLoad(GameManager.gameObject);
           // playerStorage.initialValue = playerPostition;
           StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
 //       trans.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneToLoad);
    }
}
