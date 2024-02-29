using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcWithDialog : NPC01
{
    public bool moveBefore = true;
    public float speed;
    public float cekanje;
    public float kreni;
    protected int randomspots;
    public Animator anim;
    public Transform[] movespots;
    protected Vector2 direction;
    protected bool Reverse;
    public bool BlockPath = false;
    // Start is called before the first frame update
    void Start()
    {
        Reverse = false;
        cekanje = kreni;
        randomspots = 0;
        anim = GetComponent<Animator>();
        direction = Vector2.zero;
    }

    // Update is called once per frame
   void Update()
    {
        Interact();
    }
    public override void Interact()
    {
        base.Interact();
        if (BlockPath == false)
        {
            Move();
        }
    }
    public void Move()
    {
            if (movespots.Length != 0)
            {
                anim.SetBool("Moving", true);
                transform.position = Vector2.MoveTowards(transform.position, movespots[randomspots].position, speed * Time.deltaTime); //(current , target ,  speed)
                anim.SetFloat("x1", (movespots[randomspots].position.x - transform.position.x));
                anim.SetFloat("y1", (movespots[randomspots].position.y - transform.position.y));
                if (Vector2.Distance(transform.position, movespots[randomspots].position) < 0.2)  //(current , target ,  )
                {
                    if (cekanje <= 0)
                    {
                        if (randomspots < movespots.Length && Reverse == false)
                        {
                            if (randomspots != movespots.Length)
                            {
                                randomspots += 1;
                                if (randomspots == movespots.Length)
                                    Reverse = true;
                            }
                        }
                        if (Reverse == true)
                        {
                            randomspots -= 1;
                            if (randomspots == 0)
                                Reverse = false;
                        }
                        anim.SetFloat("x", (movespots[randomspots].position.x - transform.position.x));
                        anim.SetFloat("y", (movespots[randomspots].position.y - transform.position.y));
                        anim.SetFloat("x1", (movespots[randomspots].position.x - transform.position.x));
                        anim.SetFloat("y1", (movespots[randomspots].position.y - transform.position.y));
                        cekanje = kreni;
                    }
                    else
                    {
                        cekanje -= Time.deltaTime;
                        anim.SetBool("Moving", false);
                    }
                }
            }
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!quest.questTaken && !quest.questEnded)
            this.ContextClue.SetActive(true);

        inRangeforTalk = true;
        if (collision.CompareTag("Player"))
        {

            BlockPath = true;
            anim.SetBool("Moving", false);

        }

    }
    public override void OnTriggerExit2D(Collider2D collision)
    {
        dialogScr.currentQuestGiver = null;
        dialogScr.TmProText.text = "";
        dialogScr.RejectQuest();
        BlockPath = false;
        inRangeforTalk = false;
        DialogBox.SetActive(false);
        quest.NpcQuest.AtNpcPlace = false;
        ContextClue.SetActive(false);
    }

}
