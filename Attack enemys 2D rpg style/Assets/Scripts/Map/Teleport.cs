using TMPro;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject desiredLocation;
    public Vector3 offset;
    private CameraMovement cam;
    public int number;
    public CreateMap mapScrObject;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
        {
            cam.MapTransfer(mapScrObject.minPosition, mapScrObject.maxPosition, mapScrObject.mapName);
            collision.transform.position = desiredLocation.transform.position + offset;
            cam.smooothing = 0.03f;
        }
    }
}




