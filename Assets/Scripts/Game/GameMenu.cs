using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject MenuObject;
    
    public void CloseMenu()
    {
        MenuObject.SetActive(false);
        Control.isControllable = true;
    }

    public void ReplayCurrentStage()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitCurrentStage()
    {
        InformationProvider.Instance.stageIndex = -1;
        SceneManager.LoadScene("Main");
    }
}