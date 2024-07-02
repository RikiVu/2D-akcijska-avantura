using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{  
    public PlayerScr targetScript;
    public Transform target;
    public float smooothing;
    public Vector2 maxPosition;
    public Vector2 minPosition;
    Vector3 targetPosition;
    public AlertPanelScr alertPanelScr;
    public Animator animator;
    [SerializeField] SpawnEnemies introEnemiesScr;
    [SerializeField] Animator spawnAnimator;
    [SerializeField] descriptionPanelScr descriptionPanelScr;
    [SerializeField] List<string> strings = new List<string>();
    public bool cameraAnimDone = true;
    private string str1 = "Controls \n Movement \"W A S D\"\r\nRunning \"left Shift\"\r\nInteraction \"E\"\r\nInventory \"I\"\r\nQuest panel \"L\"\r\nMenu \"Esc\"\r\nAttack \"Space\"\r\n";
    private string str2 = "Welcome to Mystic Quest: Arachne's Lair!\r\n\r\nAs dawn breaks, you find yourself mysteriously summoned to an ancient shrine, its glowing runes pulsating with a strange energy. Before you lies a quaint village nestled within the heart of an enchanted forest, its peaceful facade marred by a sinister threat.\r\n\r\nThe villagers, once thriving and joyous, now live in fear, tormented by monstrous creatures emerging from the shadows. They look to you, a hero from another world, for salvation. Here, in this humble village, you can prepare for the trials ahead by purchasing equipment and gathering vital resources.\r\n\r\nYour ultimate quest is to collect the 10 sacred stars, hidden across perilous landscapes and guarded by formidable foes. Each star brings you closer to unlocking the power needed to face the greatest terror of all: Arachne, the fearsome spider queen. She lurks in an abandoned castle, a looming fortress of dread where countless adventurers have met their end.\r\n\r\nPrepare yourself, brave soul. Your journey will be fraught with danger, but with courage and cunning, you can restore peace to the village and conquer the malevolent Arachne. The fate of this world rests in your hands.";
    private void Start()
    {

        strings.Add(str1);
        strings.Add(str2);
    }
    void LateUpdate()
    {
            if (transform.position != target.position)
            {
                targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
                targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
                transform.position = Vector3.Lerp(transform.position, targetPosition, smooothing);
            }
    }
 
    public void cameraLoaded()
    {
        //Debug.Log("camera loaded");
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }
    public void animationDone()
    {
        animator.enabled = false;
        descriptionPanelScr.showDescriptionPanel(strings);
        cameraAnimDone = true;
    }
    public void spawnAnimation()
    {
        spawnAnimator.enabled = true;
    }
    public void spawnPlayer()
    {
       // Debug.Log("spawn player camera movement ");
        targetScript.spawningPlayer();
    }
    public void newgame()
    {
        //Debug.Log("New game");
        cameraAnimDone = false;
        PlayerScr.playerCanMove = false;
        introEnemiesScr.newGameIntroSpawn();
        animator.Play("cameraanimator");
    }
    public void MapTransfer(Vector2 valueMin, Vector2 valueMax, string name)
    {
        minPosition = valueMin;
        maxPosition = valueMax;
        alertPanelScr.showAlertPanel(name);
    }
    public void MapTransfer(Vector2 valueMin, Vector2 valueMax)
    {
        minPosition = valueMin;
        maxPosition = valueMax;
    }
    public void skipIntro()
    {
        spawnPlayer();
        animationDone();
    }
}
