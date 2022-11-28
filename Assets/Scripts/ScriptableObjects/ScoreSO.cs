using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Score", menuName = "Game Off/Score")]
public class ScoreSO : ScriptableObject
{
    [field: SerializeField] public int TotalScore { get; private set; }
    [field: SerializeField] public float TotalTime { get; private set; }
    [field: SerializeField]
    public float LevelTimeRemaining { get; private set; }
    [field: SerializeField]
    public List<int> ScorePerLevel { get; private set; }
    [field: SerializeField]
    public List<float> TimePerLevel { get; private set; }

    public void Reset()
    {
        TotalScore = 0;
        TotalTime = 0;
        LevelTimeRemaining = 0f;
    }

    public void UpdateTotalScore(int addedScore)
    {
        TotalScore += addedScore;
    }

    public void UpdateTotalTime(float addedTime)
    {
        TotalTime += addedTime;
    }

    public void SetTimeThisLevel(float remainingTime)
    {
        LevelTimeRemaining = remainingTime;
    }

    public string GetFormattedTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
