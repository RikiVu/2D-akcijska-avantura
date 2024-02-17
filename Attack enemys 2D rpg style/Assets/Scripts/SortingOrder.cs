using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrder : MonoBehaviour
{
    [SerializeField]
    private int sortingOrderBase = 3;
    [SerializeField]
    private int offset = 0;
    private SpriteRenderer myRenderer;
    public bool inside =false;

    private void Awake()
    {
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    private void LateUpdate()
    {
        myRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
        if (this.tag == "Obstacle" && inside == true)
        {
            myRenderer.color = new Color(1f, 1f, 1f, .6f);
        }
        

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.isTrigger && this.tag == "Obstacle")
        {
            inside = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
       if(collision.tag == "Player" && collision.isTrigger)
        { 
            inside = false;

        if (this.tag == "Obstacle" && inside == false && collision.isTrigger)
        {
            myRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
            
        }
    }


}
