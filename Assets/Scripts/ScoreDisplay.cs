using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public GameObject scorePanelPrefab; 
    public Transform scoreGrid; 
    private List<GameObject> scoreEntries = new List<GameObject>();

    private const int maxEntries = 10;   

    void Start()
    {
        
    }
    public void RefreshDisplay(List<ScoreData> scores)
    {
        foreach (var entry in scoreEntries)
        {
            Destroy(entry);
        }
        scoreEntries.Clear();

        foreach (ScoreData score in scores)
        {
            AddScoreEntry(score.GetScoreText());
        }
    }

    private void AddScoreEntry(string scoreText)
    {
        GameObject newScorePanel = Instantiate(scorePanelPrefab, scoreGrid);

        // Set the text of the new panel
        Text[] texts = newScorePanel.GetComponentsInChildren<Text>();
        if (texts.Length > 0)
        {
            texts[0].text = scoreText;  
        }

        scoreEntries.Add(newScorePanel);
    }
}