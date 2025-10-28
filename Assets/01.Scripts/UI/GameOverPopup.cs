using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour, IUIElement<ScoreData>
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _maxScoreText;
    [SerializeField] private TextMeshProUGUI _newText;
    [Space]
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _gotoMenuButton;
    [Space]
    [SerializeField] private float _duration;

    private int _currentScene;
    private CanvasGroup _canvasGroup;
    private Tween _openCloseTween;


    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _currentScene = SceneManager.GetActiveScene().buildIndex;
        _retryButton.onClick.AddListener(() => SceneManager.LoadScene(_currentScene));
        _gotoMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));  
    }

    private void OnDestroy()
    {
        _retryButton.onClick.RemoveAllListeners();
        _gotoMenuButton.onClick.RemoveAllListeners();
    }

    public void EnableFor(ScoreData data)
    {
        if (_openCloseTween != null && _openCloseTween.active) _openCloseTween.Kill();
        _openCloseTween = _canvasGroup.DOFade(1f, _duration).OnComplete(() =>
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        });

        _scoreText.SetText($"점수: {data.score}");
        _maxScoreText.SetText($"최대 점수: {data.maxScore}");
        _newText.gameObject.SetActive(data.maxScoreUpdated);
    }

    public void Disable()
    {
        if (_openCloseTween != null && _openCloseTween.active) _openCloseTween.Kill();
        _openCloseTween = _canvasGroup.DOFade(0f, _duration);
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }
}

public struct ScoreData
{
    public bool maxScoreUpdated;

    public int maxScore;
    public int score;

    public ScoreData(bool maxScoreUpdated, int maxScore, int score)
    {
        this.maxScoreUpdated = maxScoreUpdated;
        this.maxScore = maxScore;
        this.score = score;
    }
}