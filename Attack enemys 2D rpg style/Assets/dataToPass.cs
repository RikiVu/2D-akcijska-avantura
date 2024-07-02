using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class dataToPass : MonoBehaviour
{
    public CreateSettings createSettings;
    public SaveOrLoad SaveOrLoad;
    [SerializeField]
    private PlayerScr playerScr;
    [SerializeField]
    CameraMovement cameraMovement;


    private void Awake()
    {
        //dodaj loading panel pa kad rjesi ovo dolje onda maknes
    }
    private void Start()
    {
        if (createSettings.newGame)
        {
            //camera intro
            PlayerScr.GodMode = createSettings.godmode;
            PlayerScr.Gold = 100;
            if (createSettings.godmode)
            {
                PlayerScr.Gold = 10000;
            }
            StopCoroutine(WaitCoro());
            StartCoroutine(WaitCoro());
        }
        else
        {
            StopCoroutine(WaitCoro2());
            StartCoroutine(WaitCoro2());
        }
    }
    IEnumerator WaitCoro()
    {
        yield return new WaitForSeconds(.5f);
        cameraMovement.newgame();
        SaveOrLoad.NewGame(createSettings.recordSelected, createSettings.godmode, createSettings.diff);
        SpawnEnemies.defaultDifficulty = createSettings.diff.ToString();
    }
    IEnumerator WaitCoro2()
    {
        yield return new WaitForSeconds(0.3f);
        cameraMovement.cameraLoaded();
        SaveOrLoad.LoadFromJson(createSettings.recordSelected);
        playerScr.spawningPlayer();
    }

}
