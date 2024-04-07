using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapTransfer : MonoBehaviour
{
    public Vector2 cameraChange;
    public Vector3 playerChange;
    private CameraMovement cam;
    public CreateMap mapScrObject;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent <CameraMovement>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.isTrigger)
        {
            cam.MapTransfer(mapScrObject.minPosition, mapScrObject.maxPosition, mapScrObject.mapName);
            other.transform.position += playerChange;
            cam.smooothing = 0.03f;
          //  other.transform.position = spawnLocation.transform.position;
        
        }
    }
}


   

   