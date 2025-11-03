public struct AddScoreEvent : IGameEvent
{
    public int scoreToAdd;

    public AddScoreEvent(int score)
    {
        scoreToAdd = score;
    }
}
