using System.Globalization;
using UnityEngine.UI;

namespace MapEditor
{
    public class SettingView : CategoryView
    {
        public InputField bounceCountInput;
        public InputField twoStarLimitInput;
        public InputField threeStarLimitInput;
        public InputField minMoveTurnInput;
        
        public override void Initialize()
        {
            
        }

        public override void Show()
        {
            bounceCountInput.text = editorManager.bounceCount.ToString();
            twoStarLimitInput.text = editorManager.twoStarTurnLimit.ToString();
            threeStarLimitInput.text = editorManager.threeStarTurnLimit.ToString();
            minMoveTurnInput.text = editorManager.minMoveTurn.ToString();
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ChangeBounceCount(string count)
        {
            editorManager.bounceCount = int.Parse(count);
        }

        public void ChangeTwoStarTurnLimit(string turn)
        {
            editorManager.twoStarTurnLimit = int.Parse(turn);
        }

        public void ChangeThreeStarTurnLimit(string turn)
        {
            editorManager.threeStarTurnLimit = int.Parse(turn);
        }

        public void ChangeMinMoveTurn(string turn)
        {
            editorManager.minMoveTurn = int.Parse(turn);
        }
    }
}