using System.IO;
using UnityEngine;

public class GameDataController : MonoBehaviour
{
    [SerializeField] private GameOverPopup _gameOverPopup;
    [SerializeField] private ScoreIndicator _scoreIndicator;
    [SerializeField] private BlockMap _blockMap;

    private SabotageController _sabotageController;
    private int score = 0;

    private readonly string _path = Path.Combine(Application.dataPath, "maxscore.txt");

    public int MaxScore { get; private set; } = 0;

    protected void Awake()
    {
        LoadMaxScore();
        Bus<AddScoreEvent>.OnEvent += AddScore;
        Bus<GameOverEvent>.OnEvent += SetGameOverPopup;

        _sabotageController = GetComponent<SabotageController>();
        _sabotageController.StartSabotage(_blockMap);
    }

    private void OnDestroy()
    {
        Bus<AddScoreEvent>.OnEvent -= AddScore;
        Bus<GameOverEvent>.OnEvent -= SetGameOverPopup;
    }

    public void AddScore(AddScoreEvent evt)
    {
        score += evt.scoreToAdd;
        _scoreIndicator.UpdateScore(score);
    }

    public void SetGameOverPopup(GameOverEvent evt)
    {
        _gameOverPopup.EnableFor(new ScoreData(TryUpdateMaxScore(score), MaxScore, score));
    }

    public bool TryUpdateMaxScore(int score)
    {
        if (score > MaxScore)
        {
            MaxScore = score;
            SaveMaxScore();
            return true;
        }

        return false;
    }

    #region Save&Load

    public void SaveMaxScore()
    {
        File.WriteAllText(_path, MaxScore.ToString());
    }

    private void LoadMaxScore()
    {
        if (File.Exists(_path))
        {
            string scoreString = File.ReadAllText(_path);
            if (int.TryParse(scoreString, out int score))
            {
                MaxScore = score;
            }
        }
    }

    #endregion
}
