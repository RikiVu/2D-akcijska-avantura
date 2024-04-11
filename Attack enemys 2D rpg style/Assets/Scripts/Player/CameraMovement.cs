using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{  
    public Transform target;
    public float smooothing;
    public Vector2 maxPosition;
    public Vector2 minPosition;
    private Scene scene;
    private string nameofScene;
    Vector3 targetPosition;
    public AlertPanelScr alertPanelScr;


    // Ogranicava koliko kamera moze ici ... I dodaje onako malo da zaostane kad krenes effect 

    void LateUpdate()
    {
            if (transform.position != target.position && nameofScene != "House")
            {
                targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
                targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
                transform.position = Vector3.Lerp(transform.position, targetPosition, smooothing);
            }
    }

    public void MapTransfer(Vector2 valueMin, Vector2 valueMax, string name)
    {
        minPosition = valueMin;
        maxPosition = valueMax;
        alertPanelScr.showAlertPanel(name);

    }
    public void MapTransfer(Vector2 valueMin, Vector2 valueMax)
    {
        minPosition = valueMin;
        maxPosition = valueMax;
    }
}
