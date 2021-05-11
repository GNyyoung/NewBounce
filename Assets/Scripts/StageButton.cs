using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public GameObject[] starObjects;
    public Text stageNumText;
    public Button entranceButton;
    public int stageIndex;

    public void StartStage()
    {
        InformationProvider.Instance.stageIndex = stageIndex;
        SceneManager.LoadScene("Game");
    }
}
