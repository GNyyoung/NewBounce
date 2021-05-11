using UnityEngine;
using UnityEngine.UI;

public class GameInformation : MonoBehaviour
{
    public Text turnText;
    public Text bounceText;
    public Text starConditionText;

    public void Initialize(int bounce, int initialStar)
    {
        turnText.text = 0.ToString();
        bounceText.text = bounce.ToString();
        starConditionText.text = $"0 / {initialStar}";
    }

    public void UpdateTurn()
    {
        turnText.text = (int.Parse(turnText.text) + 1).ToString();
    }

    public void UpdateBounceCount(int num)
    {
        bounceText.text = num.ToString();
    }

    public void UpdateStarCondition(int initial, int current)
    {
        starConditionText.text = $"{current} / {initial}";
    }
}