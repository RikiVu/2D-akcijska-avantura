using System;
using TMPro;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject desiredLocation;
    private Vector3 locationToSpawn;
    public Vector3 offset;
    private CameraMovement cam;
    public int number;
    public CreateMap mapScrObject;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
        locationToSpawn = new Vector3(desiredLocation.transform.position.x, desiredLocation.transform.position.y, 0);
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
        {
           
            audioSource.clip = mapScrObject.songToPlay;
            audioSource.Play();
            cam.MapTransfer(mapScrObject.minPosition, mapScrObject.maxPosition, mapScrObject.mapName);
           
            collision.transform.position = locationToSpawn + offset;
        }
    }
}




