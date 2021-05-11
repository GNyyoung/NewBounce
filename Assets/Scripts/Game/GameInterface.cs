using UnityEngine;

public class GameInterface : MonoBehaviour
{
    public GameObject menuObject;
    
    public void OpenMenu()
    {
        Control.isControllable = false;
        menuObject.SetActive(true);
    }
}