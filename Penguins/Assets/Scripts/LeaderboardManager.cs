using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    private List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>(); // Store leaderboard entries
    private bool hasCompletedFirstLevel = false;           // Tracks if the first level is completed

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist data across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    // Adds a new entry to the leaderboard
   public void AddToLeaderboard(string playerName, int level, float time)
{
    if (!string.IsNullOrEmpty(playerName))
    {
        LeaderboardEntry newEntry = new LeaderboardEntry(playerName, level, time);
        leaderboard.Add(newEntry);

        // Sort by level (descending), then by time (ascending) for ties
        leaderboard.Sort((a, b) =>
        {
            int levelComparison = b.level.CompareTo(a.level);
            if (levelComparison == 0)
            {
                return a.time.CompareTo(b.time); // Lower time is better
            }
            return levelComparison;
        });

        Debug.Log($"Added {playerName} time {time} to the leaderboard.");
    }
    else
    {
        Debug.LogWarning("Player name is empty or null. Skipping leaderboard entry.");
    }
}


    // Returns the leaderboard entries
    public List<LeaderboardEntry> GetLeaderboard()
    {
        return leaderboard;
    }

    // Marks that the first level has been completed
    public void CompleteFirstLevel()
    {
        hasCompletedFirstLevel = true;
    }

    // Checks if the first level is completed
    public bool IsFirstLevelCompleted()
    {
        return hasCompletedFirstLevel;
    }

    // Load the LeaderBoard UI scene
    public void LoadLeaderboardScene()
    {
        SceneManager.LoadScene("LeaderBoard UI");
    }

    

    // Load the main game scene
    public void LoadGameScene()
    {
        SceneManager.LoadScene("StartScreen"); // Adjust based on your main game scene's name
    }
}

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int level;    // Level completed
    public float time;   // Time taken to complete the level

    public LeaderboardEntry(string playerName, int level, float time)
    {
        this.playerName = playerName;
        this.level = level;
        this.time = time;
    }
}
