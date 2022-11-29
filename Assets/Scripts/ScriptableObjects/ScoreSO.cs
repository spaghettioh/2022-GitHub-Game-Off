using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Score", menuName = "Game Off/Score")]
public class ScoreSO : ScriptableObject
{
    [field: SerializeField] public int TotalScore { get; private set; }
    [field: SerializeField] public float TotalTime { get; private set; }
    [field: SerializeField]
    public float ThisLevelTimer { get; private set; }
    [field: SerializeField]
    public float ThisLevelTimeRemaining { get; private set; }
    [field: SerializeField]
    public List<int> ScorePerLevel { get; private set; }
    [field: SerializeField]
    public List<float> TimePerLevel { get; private set; }

    public void Reset()
    {
        TotalScore = 0;
        TotalTime = 0;
        ThisLevelTimer = 0f;
        ThisLevelTimeRemaining = 0f;
    }

    public void UpdateTotalScore(int addedScore)
    {
        TotalScore += addedScore;
    }

    public void UpdateTotalTime()
    {
        TotalTime += ThisLevelTimer - ThisLevelTimeRemaining;
    }

    public void SetTimeThisForLevel(float amount)
    {
        ThisLevelTimer = amount;
    }

    public void SetTimeWhenWon(float amount)
    {
        ThisLevelTimeRemaining = amount;
    }

    public string GetFormattedTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
