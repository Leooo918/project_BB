using DG.Tweening;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModeSelector : MonoBehaviour
{
    [SerializeField] private Button _leftButton, _rightButton;
    [SerializeField] private Button _startButton, _quitButton;
    [SerializeField] private RectTransform _modeRectTransform;

    [SerializeField] private List<TextMeshProUGUI> _maxScoreText;
    [SerializeField] private List<float> _modePositionList;
    [SerializeField] private List<string> _sceneName;

    [SerializeField] private float _tweenDuration = 0.3f;

    private Tween _moveTween;
    private int _currentIndex = 0;

    private void Awake()
    {
        _startButton.onClick.AddListener(StartGame);
        _quitButton.onClick.AddListener(QuitGame);

        _rightButton.onClick.AddListener(MoveRight);
        _leftButton.onClick.AddListener(MoveLeft);
        SetMoveButton();

        for (int i = 0; i < _maxScoreText.Count; i++)
        {
            string path = Path.Combine(Application.dataPath, $"{_sceneName[i]}maxscore.txt");
            string score = "0";
            if (File.Exists(path)) score = File.ReadAllText(path);
            _maxScoreText[i].SetText($"최대점수: {score}");
        }
    }

    private void OnDestroy()
    {
        _leftButton.onClick.RemoveAllListeners();
        _rightButton.onClick.RemoveAllListeners();
        _startButton.onClick.RemoveAllListeners();
        _quitButton.onClick.RemoveAllListeners();
    }

    private void MoveLeft()
    {
        if (_currentIndex == 0) return;
        _currentIndex--;
        SetMoveButton();

        _moveTween = _modeRectTransform.DOAnchorPosX(_modePositionList[_currentIndex], _tweenDuration);
    }

    private void MoveRight()
    {
        if (_currentIndex == _modePositionList.Count - 1) return;
        _currentIndex++;
        SetMoveButton();

        _moveTween = _modeRectTransform.DOAnchorPosX(_modePositionList[_currentIndex], _tweenDuration);
    }

    private void SetMoveButton()
    {
        _leftButton.gameObject.SetActive(_currentIndex > 0);
        _rightButton.gameObject.SetActive(_currentIndex < (_modePositionList.Count - 1));
    }


    private void StartGame()
    {
        SceneManager.LoadScene(_sceneName[_currentIndex]);
    }

    private void QuitGame() => Application.Quit();
}
