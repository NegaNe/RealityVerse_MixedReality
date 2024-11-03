using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public GameObject scorePanelPrefab; 
    public Transform scoreGrid; 
    private List<GameObject> scoreEntries = new List<GameObject>();

    private const int maxEntries = 10;   

    public void RefreshDisplay(List<ScoreData> scores)
    {
        // Clear existing panels
        foreach (var entry in scoreEntries)
        {
            Destroy(entry);
        }
        scoreEntries.Clear();

        // Add a new panel for each score in the list
        foreach (ScoreData score in scores)
        {
            AddScoreEntry(score);
        }
    }

    private void AddScoreEntry(ScoreData score)
    {
        // Instantiate a new score panel
        GameObject newScorePanel = Instantiate(scorePanelPrefab, scoreGrid);

        // Find and set the text components for WinState, EnemiesKilled, and Time
        Text winStateText = newScorePanel.transform.Find("WinState").GetComponent<Text>();
        Text enemiesKilledText = newScorePanel.transform.Find("EnemiesKilled").GetComponent<Text>();
        Text timeText = newScorePanel.transform.Find("Time").GetComponent<Text>();

        // Set the text values based on the ScoreData
        if (winStateText != null) winStateText.text = score.gameResult;
        if (enemiesKilledText != null) enemiesKilledText.text = score.enemiesKilled.ToString();
        if (timeText != null) timeText.text = $"{score.minutes:00}:{score.seconds:00}";

        // Keep track of this panel so we can clear it later
        scoreEntries.Add(newScorePanel);
    }
}
