using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    public class InfoPanel : MonoBehaviour
    {
        private InfoPanel(){}
        private static InfoPanel _instance;

        public static InfoPanel Instance
        {
            get
            {
                if (_instance == null)
                {
                    var infoPanels = FindObjectsOfType<InfoPanel>();
                    if (infoPanels.Length == 0)
                    {
                        Debug.LogError("InfoPanel에 스크립트를 추가해주세요.");    
                    }
                    else if (infoPanels.Length == 1)
                    {
                        _instance = infoPanels[0];
                    }
                    else
                    {
                        Debug.LogError("InfoPanel 외 다른 오브젝트에 이 스크립트가 포함돼있습니다.");
                        for (int i = infoPanels.Length - 1; i >= 0; i--)
                        {
                            if (infoPanels[i].gameObject.name != "InfoPanel")
                            {
                                Destroy(infoPanels[i]);
                            }
                            else
                            {
                                _instance = infoPanels[i];
                            }
                        }

                        if (_instance == null)
                        {
                            Debug.LogError("InfoPanel에 스크립트를 추가해주세요.");
                        }
                    }
                }

                return _instance;
            }
        }
        
        public Text typeText;
        public Text typeInfoText;
        public Text optionName;
        public Text optionValue;
        public Button button;
        public EditorManager editorManager;

        private void Start()
        {
            _instance = this;
            gameObject.SetActive(false);
        }

        public void ShowCreatePanel(string objectName)
        {
            typeText.text = "생성";
            typeInfoText.text = objectName;
            var clickedEvent = new Button.ButtonClickedEvent();
            // 오브젝트 생성 취소 버튼
            clickedEvent.AddListener(() =>
            {
                editorManager.currentMode = EditorMode.Normal;
                editorManager.objectPrefab = null;
                gameObject.SetActive(false);
            });
            button.onClick = clickedEvent;
            button.gameObject.name = "취소";
            optionValue.text = editorManager.objectPrefab.GetComponent<EditorObject>().value.ToString();
            gameObject.SetActive(true);
        }

        public void ShowInfoPanel(EditorObject[] editorObjects)
        {
            typeText.text = "타입";
            if (editorObjects.Length == 1)
            {
                typeInfoText.text = editorObjects[0].objectName;
                optionValue.text = editorObjects[0].value.ToString();
            }
            else
            {
                typeInfoText.text = $"{editorObjects.Length}개 오브젝트";
                optionValue.text = null;
            }
            var clickedEvent = new Button.ButtonClickedEvent();
            // 오브젝트 삭제 버튼
            clickedEvent.AddListener(() =>
            {
                foreach (var editorObject in editorObjects)
                {
                    editorManager.RemoveObject(editorObject);
                }
                editorManager.selectedObjectList.Clear();
                gameObject.SetActive(false);
            });
            button.onClick = clickedEvent;
            button.gameObject.name = "삭제";
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}