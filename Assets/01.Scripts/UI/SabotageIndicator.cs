using TMPro;
using UnityEngine;

public class SabotageIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetIndicator(SabotageSO sabotage, int count)
    {
        _text.SetText($"방해까지: {count}");
        //indicate sabotage info 
    }
}
