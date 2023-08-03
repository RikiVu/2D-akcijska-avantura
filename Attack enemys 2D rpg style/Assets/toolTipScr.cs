using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class toolTipScr : MonoBehaviour
{
    public TextMeshProUGUI nameGM;
    public TextMeshProUGUI descGM;
    // Start is called before the first frame update

   public void ChangeText(string name, string desc)
    {
        nameGM.text = name;
        descGM.text = desc;
    }
}
