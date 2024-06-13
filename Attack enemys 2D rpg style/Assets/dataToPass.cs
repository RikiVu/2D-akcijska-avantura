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
            PlayerScr.Gold = 100;
            StartCoroutine(WaitCoro());
        }
        else
        {
            StartCoroutine(WaitCoro2());
        }
    }
    IEnumerator WaitCoro()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerScr.GodMode = createSettings.godmode;
        if (createSettings.godmode)
        {
            PlayerScr.Gold = 10000;
        }
        SaveOrLoad.NewGame(createSettings.recordSelected, createSettings.godmode, createSettings.diff);
        SpawnEnemies.defaultDifficulty = createSettings.diff.ToString();
    }
    IEnumerator WaitCoro2()
    {
        yield return new WaitForSeconds(0.1f);
        SaveOrLoad.LoadFromJson(createSettings.recordSelected);
    }

}
