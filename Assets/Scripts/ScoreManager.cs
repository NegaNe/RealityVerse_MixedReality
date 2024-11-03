using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ScoreData
{
    public int enemiesKilled;
    public int minutes;
    public int seconds;
    public string gameResult;

    public ScoreData(int enemiesKilled, float timeLeftSeconds, string gameResult)
    {
        this.enemiesKilled = enemiesKilled;
        this.minutes = Mathf.FloorToInt(timeLeftSeconds / 60);
        this.seconds = Mathf.FloorToInt(timeLeftSeconds % 60);
        this.gameResult = gameResult;
    }

    public string GetScoreText()
    {
        return $"Enemies Killed: {enemiesKilled}, Time Left: {minutes:00}:{seconds:00}, Result: {gameResult}";
    }
}

[Serializable]
public class ScoreList
{
    public List<ScoreData> scores = new List<ScoreData>();
}

public class ScoreManager : MonoBehaviour
{
    private const string FILE_NAME = "scoreboard.json";
    private const int MaxEntries = 10;
    private string filePath;
    private ScoreList scoreList;

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, FILE_NAME);
        LoadScores();
    }

    void Start()
    {
        ScoreDisplay scoreboard = FindObjectOfType<ScoreDisplay>();
        if (scoreboard != null)
        {
            Debug.Log("Initializing ScoreDisplay with loaded scores.");
            scoreboard.RefreshDisplay(scoreList.scores);
        }
    }

    public void AddScore(int enemiesKilled, float timeLeftSeconds, string gameResult)
    {
        ScoreData newScore = new ScoreData(enemiesKilled, timeLeftSeconds, gameResult);
        scoreList.scores.Insert(0, newScore);

        if (scoreList.scores.Count > MaxEntries)
        {
            scoreList.scores.RemoveAt(scoreList.scores.Count - 1);
        }

        SaveScores();

        ScoreDisplay scoreboard = FindObjectOfType<ScoreDisplay>();
        if (scoreboard != null)
        {
            Debug.Log($"Adding score: {newScore.GetScoreText()}");
            scoreboard.RefreshDisplay(scoreList.scores);
        }
    }

    private void SaveScores()
    {
        string json = JsonUtility.ToJson(scoreList, true);
        File.WriteAllText(filePath, json);
    }

    private void LoadScores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            scoreList = JsonUtility.FromJson<ScoreList>(json);
            Debug.Log($"Scores loaded from file. Total scores: {scoreList.scores.Count}");
        }
        else
        {
            scoreList = new ScoreList();
            Debug.Log("No existing score file found, starting with an empty list.");
        }
    }

    public List<ScoreData> GetAllScores()
    {
        return scoreList.scores;
    }
}
