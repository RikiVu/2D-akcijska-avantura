using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{  
    public Transform target;
    public GameObject targetGM;
    public float smooothing;
    public Vector2[] maxPosition;
    public Vector2[] minPosition;
    private Scene scene;
    private string nameofScene;
    public int i = 0;

    private void Awake()
    {
        targetGM = GameObject.FindGameObjectWithTag("Player");
        //FindObjectOfType<PlayerScr>;

    }
    
    private void Update()
    {
      //  scene = SceneManager.GetActiveScene();
      //  nameofScene = scene.name;
    }



    // Ogranicava koliko kamera moze ici ... I dodaje onako malo da zaostane kad krenes effect 

    void LateUpdate()
    {
        if (transform.position != targetGM.transform.position && nameofScene != "House" )

        {
            Vector3 targetPosition = new Vector3(targetGM.transform.position.x, targetGM.transform.position.y, transform.position.z);

            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition[i].x, maxPosition[i].x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition[i].y, maxPosition[i].y);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smooothing);
        }
      /*  else if (transform.position != targetGM.transform.position && nameofScene == "Dungeon")
        {
            Vector3 targetPosition = new Vector3(targetGM.transform.position.x, targetGM.transform.position.y, transform.position.z);

        }
        */
    }
}
