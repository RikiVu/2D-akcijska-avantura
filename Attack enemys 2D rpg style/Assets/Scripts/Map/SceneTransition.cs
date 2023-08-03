using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Canvas can;
    public Canvas can2;
    public Canvas can3;
    public GameObject GameManager;
    public Animator trans;
    public string sceneToLoad;
    public Vector2 playerPostition;
    public VectorValue playerStorage;
  //  public GameObject Player;


    // za mjenjanje scena kada triggeramo

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            DontDestroyOnLoad(can.gameObject);
            DontDestroyOnLoad(can2.gameObject);
            DontDestroyOnLoad(can3.gameObject);
           // DontDestroyOnLoad(Player.gameObject);
            DontDestroyOnLoad(GameManager.gameObject);
            playerStorage.initialValue = playerPostition;
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
