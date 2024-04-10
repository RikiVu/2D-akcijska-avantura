using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class JsonReadWriteSystem : MonoBehaviour
{
    public TMP_InputField idInputField;
    public TMP_InputField nameInputField;
    public TMP_InputField infoInputField;
    public void SavetoJson()
    {
        TestData data = new TestData();
        data.Id = idInputField.text;
        data.Name = nameInputField.text;
        data.Info = infoInputField.text;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/testData.json", json);
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/testData.json");
        TestData data = JsonUtility.FromJson<TestData>(json);

        idInputField.text = data.Id;
        nameInputField.text = data.Name;    
        infoInputField.text = data.Info;

    }
}
