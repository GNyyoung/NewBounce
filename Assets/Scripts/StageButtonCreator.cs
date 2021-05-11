using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonCreator : MonoBehaviour
{
    public GameObject stageContent;
    public RectTransform ScrollViewRectTransform;
    public GameObject stageButtonPrefab;
    public Sprite selectedStageImage;
    
    // Start is called before the first frame update
    private void Start()
    {
        var stageData = Resources.LoadAll<TextAsset>("Data/Stage");
        InformationProvider.Instance.stageDataset = stageData;
        CreateStageButton(stageData);
    }

    private void CreateStageButton(TextAsset[] stageData)
    {
        int stageNum = 1;
        foreach (var stage in stageData)
        {
            var stageButtonObject = Instantiate(stageButtonPrefab, stageContent.transform);
            stageButtonObject.name = stage.name;
            var stageButtonComponent = stageButtonObject.GetComponent<StageButton>();
            stageButtonComponent.stageNumText.text = stageNum.ToString();
            // stageButtonComponent.buttonObject.GetComponent<Image>().sprite = null;
            if (stageNum <= InformationProvider.Instance.stageStarNum.Count)
            {
                for (int i = 0; i < InformationProvider.Instance.stageStarNum[stageNum - 1]; i++)
                {
                    stageButtonComponent.starObjects[i].SetActive(true);
                }
            }
            SetStageButtonObjPosition(stageButtonObject, stageNum);
            stageButtonComponent.stageIndex = stageNum;
            if (stageNum > InformationProvider.Instance.stageStarNum.Count + 1)
            {
                stageButtonComponent.entranceButton.interactable = false;
            }
            stageNum += 1;
        }

        Debug.Log(InformationProvider.Instance.stageIndex - 1);
        if (InformationProvider.Instance.stageIndex > 0)
        {
            stageContent.transform.GetChild(InformationProvider.Instance.stageIndex - 1).
                    Find("Button").GetComponent<Image>().sprite = selectedStageImage;
        }
    }

    public void UpdateStageButton()
    {
        for (int i = 0; i < stageContent.transform.childCount; i++)
        {
            int starActiveNum = 0;
            var stageButton = stageContent.transform.GetChild(i).GetComponent<StageButton>();
            foreach (var starObject in stageButton.starObjects)
            {
                if (InformationProvider.Instance.stageStarNum.Count > i &&
                    InformationProvider.Instance.stageStarNum[i] > starActiveNum)
                {
                    starObject.SetActive(true);
                    starActiveNum += 1;
                }
                else
                {
                    starObject.SetActive(false);
                }
            }

            if (starActiveNum > 0)
            {
                stageButton.entranceButton.interactable = true;
            }
            else
            {
                stageButton.entranceButton.interactable = false;
            }
        }
    }

    private void SetStageButtonObjPosition(GameObject stageButtonObject, int stageNum)
    {
        var stageButtonRectTransform = stageButtonObject.GetComponent<RectTransform>();
        var scrollViewRect = ScrollViewRectTransform.rect;
        if (stageNum % 2 == 0)
        {
            stageButtonRectTransform.anchoredPosition = 
                new Vector2(scrollViewRect.width * 0.2f,-256 * (stageNum - 1) - stageContent.GetComponent<ScrollContent>().TopPadding);       
        }
        else
        {
            stageButtonRectTransform.anchoredPosition = 
                new Vector2(scrollViewRect.width * -0.2f,-256 * (stageNum - 1) - stageContent.GetComponent<ScrollContent>().TopPadding);
        }
    }
}
