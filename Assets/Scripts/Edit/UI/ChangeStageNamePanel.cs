using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    public class ChangeStageNamePanel : MonoBehaviour
    {
        private EditorManager _editorManager;
        
        public Text errorText;
        public Text currentStageNameText;
        public InputField newStageNameInput;
        
        public void Initialize(EditorManager editorManager)
        {
            _editorManager = editorManager;
        }
        
        public void Show()
        {
            errorText.gameObject.SetActive(false);
            currentStageNameText.text = _editorManager.currentStageName;
            newStageNameInput.text = null;
            gameObject.SetActive(true);
        }

        public void ChangeStageName()
        {
            string previousStageName = _editorManager.currentStageName;
            string newStageName = newStageNameInput.text;
            
            if (string.IsNullOrEmpty(newStageName))
            {
                Debug.LogWarning("파일 이름을 입력해야 합니다.");
            }
            else if (File.Exists($"{FileView.SavePath}/{newStageName}.json"))
            {
                Debug.LogWarning("이미 존재하는 파일명입니다.");
            }
            else
            {
                File.Move($"{FileView.SavePath}/{previousStageName}.json", 
                    $"{FileView.SavePath}/{newStageName}.json");
                File.Move($"{FileView.SavePath}/{previousStageName}.json.meta", 
                    $"{FileView.SavePath}/{newStageName}.json.meta");
                _editorManager.currentStageName = newStageName;
                currentStageNameText.text = newStageName;
                Debug.Log($"이름 변경 완료 : {previousStageName} -> {newStageName}");
            }
        }
    }
}