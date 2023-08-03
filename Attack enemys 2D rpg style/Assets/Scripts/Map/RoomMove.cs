using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomMove : MonoBehaviour
{
    
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
            cam.i = this.number;
            cam.smooothing = 0.03f;
          
            collision.transform.position += playerChange;
            
            if (nextText)
            {
                StartCoroutine(placeNameCo());
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


   

   