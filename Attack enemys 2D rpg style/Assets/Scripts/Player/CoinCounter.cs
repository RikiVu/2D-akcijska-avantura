using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CoinCounter : MonoBehaviour
{

    private TextMeshProUGUI text;
    private   int coinAmount;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        coinAmount = PlayerScr.Gold;
        text.text = coinAmount.ToString();
    }
}
