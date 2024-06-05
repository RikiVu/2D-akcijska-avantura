using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Settings", menuName = "Settings" )]


public class CreateSettings : ScriptableObject
{
    new public bool fullscreen =false;
    new public float volume = 0.3f;
    new public int qualityIndex = 2;
    new public bool newGame = true;
    new public string recordSelected= "";

}



