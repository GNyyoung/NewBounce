using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    
    // 필요없을 가능성이 높음.
    // 여기 존재하는 각 메소드들은 CategoryView 상속받은 클래스에 집어넣으면 됨.
    public class UIController : MonoBehaviour
    {
        public EditorManager editorManager;

        public GameObject scrollViewObject;
        [Space(10)]
        // 메뉴에서 사용하는 버튼
        public GameObject buttonPrefab;
        public GameObject stageListPanel;
        public GameObject savepanel;
        public GameObject tileSpriteButtonPrefab;
        public InputField bounceNumInput;

        // 모든 맵 목록 만큼의 버튼을 생성함
        // public void ControlStageButtons()
        // {
        //     if(stageListPanel.transform.childCount > 0)
        //     {
        //         ClearAllStageButton();
        //         stageListPanel.SetActive(false);
        //     }
        //     else
        //     {
        //         string[] names = JsonManager.GetJsonFileNames();
        //
        //         for (int i = 0; i < names.Length; i++)
        //         {
        //             string stageName = names[i].Split('\\')[1].Split('.')[0];
        //             GameObject stageButton = Instantiate(buttonPrefab, stageListPanel.transform);
        //             stageButton.GetComponent<RectTransform>().sizeDelta = new Vector2(506, 60);
        //             stageButton.transform.Find("Text").GetComponent<Text>().text = stageName;
        //             Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
        //             buttonClickedEvent.AddListener(() => {
        //                 editorManager.LoadMap(stageName);
        //                 ClearAllStageButton();
        //                 stageListPanel.SetActive(false);
        //             });
        //             stageButton.GetComponent<Button>().onClick = buttonClickedEvent;
        //         }
        //         stageListPanel.SetActive(true);
        //     }
        // }

        // 저장 버튼 클릭 시 호출
        public void OpenSavePanel()
        {
            savepanel.SetActive(true);
        }

        // 저장화면을 닫음
        public void CloseSavePanel()
        {
            savepanel.transform.Find("Name").Find("NameInput").GetComponent<InputField>().text = null;
            savepanel.SetActive(false);
        }

        // public void SaveCurrentStage()
        // {
        //     string stageName = savepanel.transform.Find("Name").Find("NameInput").GetComponent<InputField>().text;
        //     bool isOverwrite = savepanel.transform.Find("Overwrite").GetComponent<Toggle>().isOn;
        //     bool isDeleteFile = savepanel.transform.Find("DeletePrevious").GetComponent<Toggle>().isOn;
        //     if(editorManager.SaveMap(stageName, isOverwrite, isDeleteFile) == true)
        //     {
        //         savepanel.SetActive(false);
        //     }
        // }

        // 모든 맵 목록 버튼을 제거함
        public void ClearAllStageButton()
        {
            for(int i = stageListPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(stageListPanel.transform.GetChild(i).gameObject);
            }
        }

        public void ChangeBounceCount()
        {
            int.TryParse(bounceNumInput.text, out int bounceNum);
            editorManager.bounceCount = bounceNum;
        }
    }
}

