using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interactable : MonoBehaviour
{
    public bool playerInRange;
    public SignalSender contextOn;
    public SignalSender contextOff;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.isTrigger)
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            playerInRange = false;
            contextOff.Raise();
        }
    }

}
