using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScr : MonoBehaviour
{
    [Header("Basics")]
    [SerializeField]
    protected string name= "nameless";
    [SerializeField]
    public string dialogTekst;
    protected GameObject dialogBox;
    protected DialogScr dialogBoxScr;
    protected bool playerInRange;
    private Vector2 directionToPlayer;
    protected Animator anim;
    [SerializeField] protected Vector2 animationTurnDirection; 
    protected virtual void Start()
    {
        dialogBox = GameObject.FindGameObjectWithTag("Dialog");
        dialogBoxScr = dialogBox.GetComponent<DialogScr>();
        anim = gameObject.GetComponent<Animator>();
   
        anim.SetFloat("x", animationTurnDirection.x);
        anim.SetFloat("y", animationTurnDirection.y);
    }

    protected virtual void Update()
    {
            Interact();
    }
 

    public virtual void Interact()
    {
        if (playerInRange == true)

            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogBoxScr.showDialog(dialogTekst, name);
            }
    }
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            playerInRange = true;
            Vector2 directionToPlayer = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
            anim.SetFloat("x", directionToPlayer.x);
            anim.SetFloat("y", directionToPlayer.y);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            playerInRange = false;
            if (dialogBoxScr != null)
            {
                dialogBoxScr.hideDialog();
            }
            anim.SetFloat("x", animationTurnDirection.x);
            anim.SetFloat("y", animationTurnDirection.y);
        }
       
    }
}
