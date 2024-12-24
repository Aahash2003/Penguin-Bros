using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderboardUI : MonoBehaviour
{
    public Text LeaderBoardNames;        // Text component to display leaderboard
    public InputField NameInput;   // InputField for the player's name
    public Button SubmitButton;         // Submit button
    public Button BackButton;           // Back button

    private bool allowNameInput = false; // Flag to control name input access

   void Start()
{
    EnableNameInput(false); // Disable input by default

    PopulateLeaderboard(); // Populate leaderboard (may be empty)
    BackButton.onClick.AddListener(GoBackToGame);

    // Start rechecking for LevelManager.Instance periodically
    InvokeRepeating("CheckForLevelManager", 0f, 0.5f);

    // Attach button listeners
    SubmitButton.onClick.AddListener(SubmitName);
    
}

void CheckForLevelManager()
{
    // If LevelManager.Instance is still null, wait for it
    if (LevelManager.Instance == null)
    {
        Debug.LogWarning("LevelManager.Instance is null. Waiting...");
        return;
    }

    // If LevelManager.Instance exists, update the leaderboard UI and stop rechecking
    CancelInvoke("CheckForLevelManager");

    // Enable name input only if the player has progressed past Level 1
    if (LevelManager.Instance.GetCurrentLevel() > 1)
    {
        EnableNameInput(true);
        Debug.Log("Name input enabled after Level 1.");
        LevelManager.Instance.ResetGame();
    }
    else
    {
        Debug.Log("Player is still at Level 1 or hasn't started the game.");
    }
}



    public void EnableNameInput(bool enable)
    {
        NameInput.interactable = enable;
        SubmitButton.interactable = enable;
        allowNameInput = enable;
    }


    // Populate leaderboard entries
    void PopulateLeaderboard()
    
{
    // Get leaderboard entries as a List of LeaderboardEntry
    List<LeaderboardEntry> leaderboardEntries = LeaderboardManager.Instance.GetLeaderboard();

    // Clear and populate the leaderboard text
    LeaderBoardNames.text = "Leaderboard:\n"; // Reset text
    foreach (LeaderboardEntry entry in leaderboardEntries)
    {
        LeaderBoardNames.text += $"{entry.playerName}: Time {entry.time:F2}s\n";
    }
}


    // Submit the player's name and add to leaderboard
    public void SubmitName()
{
    if (!allowNameInput) return; // Prevent submissions if input is disabled

    string playerName = NameInput.text;

    if (!string.IsNullOrEmpty(playerName))
    {
        // Get the current level and time
        int level = LevelManager.Instance.GetCurrentLevel();
        float time = LevelManager.Instance.leveltime;

        // Add the player's name, level, and time to the leaderboard
        LeaderboardManager.Instance.AddToLeaderboard(playerName, level, time);

        Debug.Log($"Added {playerName} to leaderboard with level: {level}, time: {time}");

        // Clear input, refresh leaderboard, and disable further input
        NameInput.text = "";
        PopulateLeaderboard();
        EnableNameInput(false);
    }
    else
    {
        Debug.LogWarning("Name input is empty. Cannot submit to leaderboard.");
    }
}

    // Go back to the main game scene
    public void GoBackToGame()
    {   NameInput.interactable = false;
        SubmitButton.interactable = false;
        allowNameInput = false;
        SceneManager.LoadScene("StartScreen"); // Replace with your main game scene name
    }
}
