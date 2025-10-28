using DG.Tweening;
using TMPro;
using UnityEngine;

public class PerfectText : MonoBehaviour
{
    private const float FADE_DURATION = 0.15f;
    private const float ENABLE_DURATION = 0.3f;

    [SerializeField] private CanvasGroup _canvasgroup;
    private Sequence _seq;

    public void Perfect()
    {
        if (_seq != null && _seq.active) _seq.Kill();

        _seq = DOTween.Sequence();

        _canvasgroup.alpha = 1f;
        transform.localScale = Vector3.zero;

        _seq.Append(transform.DOScale(Vector3.one, FADE_DURATION))
            .AppendInterval(ENABLE_DURATION)
            .Append(_canvasgroup.DOFade(0f, FADE_DURATION));

        //뭐 소리 재생 같은 것도 해줘
    }
}
