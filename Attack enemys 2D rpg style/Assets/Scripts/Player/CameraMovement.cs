using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{  
    public Transform target;
    public float smooothing;
    public Vector2[] maxPosition;
    public Vector2[] minPosition;
    private Scene scene;
    private string nameofScene;
    Vector3 targetPosition;


    private void Update()
    {
      //  scene = SceneManager.GetActiveScene();
      //  nameofScene = scene.name;
    }

    // Ogranicava koliko kamera moze ici ... I dodaje onako malo da zaostane kad krenes effect 

    void LateUpdate()
    {
        
            if (transform.position != target.position && nameofScene != "House")
            {

                targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
                targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition[0].x, maxPosition[0].x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition[0].y, maxPosition[0].y);
                
                transform.position = Vector3.Lerp(transform.position, targetPosition, smooothing);
            }
        
       
       
      
    }
}
