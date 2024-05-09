using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum name
{
    pot,
    bush
}

public class pot : MonoBehaviour
{
    private PlayerScr player;
    public name objectName;
    private Animator anim;
    int[] RandomGold = new int[] { 1, 2, 3, 4, 5 };
    int[] RandomChance = new int[] { 0, 1, 2, 3, 4, 5, };
   // public Transform SpawnPosition;
    public GameObject[] itemInside;
    public Transform hitBox;
    bool isHited = false;
    private Collider2D m_Collider;
    // Start is called before the first frame update

    public GameManager gameManager;

    public int assignedId;

    void Start()
    {
        anim = GetComponent<Animator>();
        m_Collider = GetComponent<Collider2D>();
        // hitBox = this.gameObject;
    }
    public virtual void DeSelect()
    {
        //player.MyTarget = null;

    }

    public void loadPot(bool state)
    {
        if(state!=null) {
            isHited = state;
            this.gameObject.SetActive(!state);
        }
    }


    public virtual Transform Select()
    {
        return hitBox;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Smash()
    {
        PlayerScr.CantAtt = true;
        if (objectName == global::name.bush != null )
        {
            
            if (objectName == global::name.bush && isHited == false)
            {
                isHited = true;
                m_Collider.enabled = false;
                FindObjectOfType<AudioManager>().Play("bushBreaking");
                anim.SetBool("smash", true);
                StartCoroutine(breakCo());
            }
          
        }
        if (objectName == global::name.pot && isHited == false)
        {
            isHited = true;
            m_Collider.enabled = false;
            FindObjectOfType<AudioManager>().Play("Smash");
            anim.SetBool("smash", true);
            StartCoroutine(breakCo());
        }

    }
    IEnumerator breakCo()
    {
        
        if (objectName == global::name.bush)
        {
           
            yield return new WaitForSeconds(0.3f);
            int randomGeneratedNum = RandomChance[Random.Range(0, RandomChance.Length)];

                if (randomGeneratedNum == 3)
                {
                    try
                    {
                        Instantiate(itemInside[Random.Range(0, itemInside.Length)], transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)), Quaternion.identity);

                    }
                    catch
                    {
                        Debug.Log("bug pot?");
                    }
                 }  
                else
                {
               // Debug.Log("Bad luck");
                }
            anim.SetBool("smash", false);
            gameManager.addInPotList(assignedId, true);
            this.gameObject.SetActive(false);

        }
        if (objectName == global::name.pot)
        {
          
            yield return new WaitForSeconds(0.7f);
            int randomGeneratedNum2 = (RandomGold[Random.Range(0, RandomGold.Length)]);
           
            if (randomGeneratedNum2  <=3)
            {
                Instantiate(itemInside[0], transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)), Quaternion.identity);
                Instantiate(itemInside[0], transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)), Quaternion.identity);

            }
            if (randomGeneratedNum2 >=4 )
            {
                Instantiate(itemInside[0], transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)), Quaternion.identity);
                Instantiate(itemInside[0], transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)), Quaternion.identity);
                Instantiate(itemInside[0], transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)), Quaternion.identity);

            }
            gameManager.addInPotList(assignedId, true);
            this.gameObject.SetActive(false);
            DeSelect();
        }

        //PlayerScr.Gold += 10;
       
    }

}
