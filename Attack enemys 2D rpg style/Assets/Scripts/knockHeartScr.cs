using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class knockHeartScr : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    public SpriteRenderer sprRend;
    private float lifetime = 1.2f;
    private float lifetimeSeconds;
    private int temp;
    void Awake()
    {
        lifetimeSeconds = lifetime;
        myRigidbody = GetComponent<Rigidbody2D>();
        temp = Random.Range(-3, 3);
    }

    void Update()
    {
        lifetimeSeconds -= Time.deltaTime;
        if (lifetimeSeconds <= 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            sprRend.color = new Color(1, 1, 1, lifetimeSeconds / 1f);
            myRigidbody.velocity = new Vector3(temp, Random.Range(1, 2),0);
        }

    }

}
