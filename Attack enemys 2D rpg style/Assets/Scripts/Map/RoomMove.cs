using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomMove : MonoBehaviour
{
    public bool spawn = false;
    public GameObject spawnLocation;
    public GameObject WallSprite;
    public static bool bossFight = false;
    public Vector3 playerChange;
    private CameraMovement cam;
    public bool nextText;
    public string placeName;
    public GameObject text;
    public TextMeshProUGUI placeText;
    private bool changedSmoothing = false; 
    public int number;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent <CameraMovement>();
       /* if (!spawn)
            WallSprite = gameObject.GetComponent<SpriteRenderer>();
       */
    }

    // Update is called once per frame
    void Update()
    {
        if(changedSmoothing)
        {
            cam.smooothing = cam.smooothing + 0.05f * Time.deltaTime;
            if( cam.smooothing >= 0.1f)
            {
                changedSmoothing = false;
                cam.smooothing = 0.1f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && collision.isTrigger)
        {
            if(spawn)
            {
                cam.i = this.number;
                cam.smooothing = 0.03f;
                collision.transform.position = spawnLocation.transform.position;
            }
            else
            {
                if (bossFight)
                {
                    cam.i = this.number;
                    cam.smooothing = 0.03f;
                    collision.transform.position += playerChange;
                    if (nextText)
                        StartCoroutine(placeNameCo());
                }
                else
                {
                    //cam.i = this.number;
                    //cam.smooothing = 0.03f;
                    collision.transform.position += playerChange + new Vector3(0,0,0);
                    if (nextText)
                        StartCoroutine(placeNameCo());
                    bossFight = true;
                    WallSprite.SetActive(true);
                    BossAi.chaseRadius = 58;
                }
            }
          
            
            
        
        }
    }
    private IEnumerator placeNameCo()
    {
        yield return new WaitForSeconds(1f);
        text.SetActive(true);
        placeText.text = placeName;
        changedSmoothing = true;
        yield return new WaitForSeconds(3f);
        text.SetActive(false);
        
    }



}


   

   