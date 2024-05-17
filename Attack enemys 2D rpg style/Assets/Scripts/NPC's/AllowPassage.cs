using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowPassage : MonoBehaviour
{
    public static bool CanPass = false;
    public GameObject Wall;
    public Transform movespots;
    public float speed;
    [SerializeField] 
    private string newText;
    private Animator anim;
    private NpcScr npcScr;
    private Vector3 initalPos;

    private void Start()
    {  
        anim =  gameObject.GetComponent<Animator>();
        npcScr = gameObject.GetComponent<NpcScr>();
        initalPos = transform.position;
    }
    private void Update()
    {
        if (CanPass)
        {
            anim.SetBool("Moving", true);
            npcScr.dialogTekst = newText;
            if (Vector3.Distance(movespots.position, transform.position) <= 1) {

                //CanPass = false;
                anim.SetBool("Moving", false);
                gameObject.GetComponent<AllowPassage>().enabled = false;
            }
            Wall.SetActive(false);
            transform.position = Vector2.MoveTowards(transform.position, movespots.position, speed * Time.deltaTime); //(current , target ,  speed)
            anim.SetFloat("x1", (movespots.position.x - transform.position.x));
            anim.SetFloat("y1", (movespots.position.y - transform.position.y));
        }
    }

    public void load(bool value)
    {
        CanPass = value;
        if (!value)
        {
            gameObject.GetComponent<AllowPassage>().enabled = true;
            transform.position = initalPos;
            anim.SetBool("Moving", false);
            Wall.SetActive(true);
        }
        else
        {
            transform.position = movespots.position;
            anim.SetBool("Moving", false);
            Wall.SetActive(false);
        }
    }
}
