using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SabotagePopup : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private float _animationDuration = 0.2f;
    [SerializeField] private float _indicateDuration = 1f;
    [SerializeField] private TextMeshProUGUI _explainText;

    private RectTransform RectTrm => transform as RectTransform;
    private Sequence _seq;

    public void SetExplain(SabotageSO sabotage)
    {
        _icon.sprite = sabotage.icon;
        _explainText.SetText($"{sabotage.sabotageName}\n{sabotage.sabotageExplain}");

        if (_seq != null && _seq.active) _seq.Kill();
        _seq = DOTween.Sequence();

        _seq.Append(RectTrm.DOAnchorPosX(0, _animationDuration))
            .AppendInterval(_indicateDuration)
            .Append(RectTrm.DOAnchorPosX(-1920f, _animationDuration));
    }
}
