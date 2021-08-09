using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainInterface : MonoBehaviour
{
    public StageButtonCreator stageButtonCreator;
    private bool isOpenAll = false;

    private void Start()
    {
        if (InformationProvider.Instance.stageStarNum == InformationProvider.Instance.allOpenedStarNum)
        {
            isOpenAll = true;
        }
        else
        {
            isOpenAll = false;
        }
    }

    public void ToggleOpenAllStage()
    {
        if (isOpenAll == true)
        {
            InformationProvider.Instance.stageStarNum = InformationProvider.Instance.originalStarNum;
            stageButtonCreator.UpdateStageButton();
            isOpenAll = false;
        }
        else
        {
            InformationProvider.Instance.originalStarNum = new List<int>(InformationProvider.Instance.stageStarNum);
            var allOpenedStarNum = new List<int>();
            if (InformationProvider.Instance.allOpenedStarNum == null ||
                InformationProvider.Instance.allOpenedStarNum.Count == 0)
            {
                Debug.Log("제작");
                for (int i = 0; i < InformationProvider.Instance.stageDataset.Length; i++)
                {
                    allOpenedStarNum.Add(3);
                }

                InformationProvider.Instance.allOpenedStarNum = allOpenedStarNum;
            }
            InformationProvider.Instance.stageStarNum = InformationProvider.Instance.allOpenedStarNum;
            stageButtonCreator.UpdateStageButton();
            isOpenAll = true;
        }
    }

    public void DeleteSaveData()
    {
        var saveDataPath = $"{Application.persistentDataPath}/SaveData.json"; 
        if (File.Exists(saveDataPath))
        {
            File.Delete(saveDataPath);
            InformationProvider.Instance.stageStarNum = new List<int>();
            InformationProvider.Instance.originalStarNum = new List<int>();
            stageButtonCreator.UpdateStageButton();
        }
    }
}
