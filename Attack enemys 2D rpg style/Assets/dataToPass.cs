using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataToPass : MonoBehaviour
{
    public CreateSettings createSettings;
    public SaveOrLoad SaveOrLoad;

    private void Awake()
    {
        //dodaj loading panel pa kad rjesi ovo dolje onda maknes
    }
    private void Start()
    {
        if (createSettings.newGame)
        {
            StartCoroutine(WaitCoro());
        }
        else
        {
            StartCoroutine(WaitCoro2());
        }
    }
    IEnumerator WaitCoro()
    {
        yield return new WaitForSeconds(0.25f);
        SaveOrLoad.NewGame();
    }
    IEnumerator WaitCoro2()
    {
        yield return new WaitForSeconds(0.25f);
        SaveOrLoad.LoadFromJson(createSettings.recordSelected);
    }

}
