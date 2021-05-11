using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageClearInterface : MonoBehaviour
{
    private StageClearInterface(){}
    private static StageClearInterface _instance;

    public static StageClearInterface Instance
    {
        get
        {
            if (_instance == null)
            {
                var components = FindObjectsOfType<StageClearInterface>();
                if (components.Length == 0)
                {
                    Debug.LogError("StageClear 패널에 이 컴포넌트가 추가돼있지 않습니다.");
                }
                else if (components.Length == 1)
                {
                    for (int i = components.Length - 1; i >= 0; i--)
                    {
                        if (components[i].gameObject.name == "StageClear")
                        {
                            _instance = components[i];
                        }
                        else
                        {
                            Destroy(components[i]);
                        }
                    }

                    if (_instance == null)
                    {
                        Debug.LogError("StageClear 패널에 이 컴포넌트가 추가돼있지 않습니다.");
                    }
                }
            }

            return _instance;
        }
    }
    
    public StageManager stageManager;
    public Slider turnSlider;
    public Text twoStarLimitText;
    public Text threeStarLimitText;
    public Text usedTurnText;

    private void Start()
    {
        Instance.enabled = true;
        gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        gameObject.SetActive(false);
    }

    public void LoadNextStage()
    {
        InformationProvider.Instance.stageIndex += 1;
        SceneManager.LoadScene("Game");
    }

    private void SetScoreSlider()
    {
        int currentMoveTurn = stageManager.CurrentMoveTurn;
        int twoStarLimit = stageManager.twoStarLimit;
        int threeStarLimit = stageManager.threeStarLimit;
        int minMoveTurn = stageManager.minClearMoveTurn;
        float sliderValue;
        int starNum = 1;
        
        if (currentMoveTurn > twoStarLimit)
        {
            sliderValue = Mathf.Pow((float)twoStarLimit / currentMoveTurn, 1.5f) * 0.5f;
        }
        else if (currentMoveTurn > threeStarLimit)
        {
            sliderValue = 0.5f + Mathf.Pow(
                (float)(twoStarLimit - currentMoveTurn) / (twoStarLimit - threeStarLimit), 1.5f) * 0.4f;
            starNum = 2;
        }
        else
        {
            sliderValue = 0.9f + Mathf.Pow(
                    (float) (threeStarLimit - currentMoveTurn) / (threeStarLimit - minMoveTurn), 1.5f) * 0.1f;
            starNum = 3;
        }

        turnSlider.value = sliderValue;
        InformationProvider.Instance.stageStarNum.Add(starNum);
    }

    private void SetStarLimit()
    {
        twoStarLimitText.text = stageManager.twoStarLimit.ToString();
        threeStarLimitText.text = stageManager.threeStarLimit.ToString();
    }

    public void StageClear()
    {
        gameObject.SetActive(true);
        usedTurnText.text = stageManager.CurrentMoveTurn.ToString();
        SetStarLimit();
        SetScoreSlider();
    }
}