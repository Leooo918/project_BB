using TMPro;
using UnityEngine;

public class ScoreIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void UpdateScore(int score)
    {
        _text.SetText(score.ToString());
    }
}
