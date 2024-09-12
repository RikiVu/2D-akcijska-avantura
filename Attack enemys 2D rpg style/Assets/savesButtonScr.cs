using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class savesButtonScr : MonoBehaviour
{
   // public static bool showButtons = false;
    public GameObject buttonPrefab1;
    private Button buttonComponent;
    private List<string> saves = new List<string>();
    //private string fileName;
    private GameObject buttonPrefab;
    [SerializeField]
    private GameObject parent;
    private TextMeshProUGUI tmpro;
    private List<GameObject> gameObjectsToDestroy = new List<GameObject>();
    [SerializeField]
    private SaveOrLoad saveOrLoad;
    [SerializeField]
    private CreateSettings createSettings;
    public void ShowLoadRecords()
    {
        parent.gameObject.SetActive(true); // Ensure the parent is active
        string[] jsonFiles = GetJsonFiles(Application.dataPath + "/saves/");

        for (int i = 0; i < jsonFiles.Length; i++)
        {
            string fileName = jsonFiles[i];
            if (fileName != null)
            {
               
                buttonPrefab = Instantiate(buttonPrefab1, this.transform);
                buttonComponent = buttonPrefab.GetComponent<Button>();
                buttonPrefab.name = "record" + i;

    
                tmpro = buttonPrefab.GetComponentInChildren<TextMeshProUGUI>();
                if (tmpro != null)
                {
                    tmpro.text = Path.GetFileNameWithoutExtension(fileName);
                }

                buttonComponent.onClick.AddListener(() => OnButtonClick(fileName));
                gameObjectsToDestroy.Add(buttonPrefab);
            }
        }
    }
    public void ShowLoadRecordsMenu()
    {
        parent.gameObject.SetActive(true); // Ensure the parent is active
        string[] jsonFiles = GetJsonFiles(Application.dataPath + "/saves/");

        for (int i = 0; i < jsonFiles.Length; i++)
        {
            string fileName = jsonFiles[i];
            if (fileName != null)
            {

                buttonPrefab = Instantiate(buttonPrefab1, this.transform);
                buttonComponent = buttonPrefab.GetComponent<Button>();
                buttonPrefab.name = "record" + i;


                tmpro = buttonPrefab.GetComponentInChildren<TextMeshProUGUI>();
                if (tmpro != null)
                {
                    tmpro.text = Path.GetFileNameWithoutExtension(fileName);
                }

                buttonComponent.onClick.AddListener(() => OnButtonClickMenu(fileName));
             
                gameObjectsToDestroy.Add(buttonPrefab);
            }
        }
    }
    public void backBtn()
    {
        parent.gameObject.SetActive(false);
        saves.Clear();
        foreach (GameObject obj in gameObjectsToDestroy)
        {
            Destroy(obj);
        }
        gameObjectsToDestroy.Clear();
    }
    void OnButtonClick(string name)
    {
        //audioManager.Play("Click");
        saveOrLoad.LoadFromJson(name);
    }
    void OnButtonClickMenu (string name)
    {
        Debug.Log(name);
        createSettings.newGame = false;
        createSettings.recordSelected = name;
        SceneManager.LoadScene("Act1");
    }
    public string[] GetJsonFiles(string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            // Get all files with the .json extension in the specified directory
            string[] files = Directory.GetFiles(directoryPath, "*.json");

            // Get only the file names from the full paths
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }
            return files;
        }
        else
        {
            Debug.LogWarning("Directory does not exist: " + directoryPath);
            return new string[0];
        }
    }


}
