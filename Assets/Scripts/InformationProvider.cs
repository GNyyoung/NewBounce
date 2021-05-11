
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapEditor;
using UnityEngine;

public class InformationProvider : MonoBehaviour
{
    private InformationProvider(){}
    private static InformationProvider _instance;

    public static InformationProvider Instance
    {
        get
        {
            if (_instance == null)
            {
                var components = FindObjectsOfType<InformationProvider>();

                if (components.Length >= 1)
                {
                    for (int i = components.Length - 1; i >= 0; i--)
                    {
                        if (components[i].gameObject.name != "DontDestroyOnLoad")
                        {
                            Destroy(components[i].gameObject);
                        }
                        else
                        {
                            _instance = components[i];
                        }
                    }
                }

                if (components.Length == 0)
                {
                    var componentObject = GameObject.Find("DontDestroyOnLoad");
                    if (componentObject == null)
                    {
                        Debug.LogError("컴포넌트를 부착할 오브젝트가 없습니다.");
                    }
                    else
                    {
                        _instance = componentObject.AddComponent<InformationProvider>();   
                    }
                }
            }

            return _instance;
        }
    }

    [HideInInspector]
    public int stageIndex;
    [HideInInspector]
    public List<int> stageStarNum = new List<int>();
    [HideInInspector] 
    public TextAsset[] stageDataset;
    public List<int> originalStarNum = new List<int>();
    public List<int> allOpenedStarNum = null;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        var savedStarNum = JsonLoader.LoadSaveData().stageStarNum;
        if (savedStarNum == null)
        {
            stageStarNum = new List<int>();
        }
        else
        {
            stageStarNum = savedStarNum.ToList();
            originalStarNum = new List<int>(stageStarNum);
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void Save()
    {
        SaveData saveData;
        if (stageStarNum == allOpenedStarNum)
        {
            saveData = new SaveData{stageStarNum = originalStarNum.ToArray()};
        }
        else
        {
            saveData = new SaveData{stageStarNum = stageStarNum.ToArray()};    
        }
        var saveDataJson = JsonUtility.ToJson(saveData);
        File.WriteAllText($"{Application.persistentDataPath}/SaveData.json", saveDataJson);
    }
}