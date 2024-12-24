using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
public void QuitGame()
    {
        Debug.Log("Quitting the game...");

        // Reset time scale to 1
        Time.timeScale = 1;

        // Unload the pause menu if active
        if (SceneManager.GetSceneByName("PauseMenuScene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("PauseMenuScene");
        }

        // Destroy persistent objects
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("MainObject"))
        {
            Destroy(obj);
        }

        // Load the StartScreen
        SceneManager.LoadScene("StartScreen");
    }

    public void RestartGame()
    {
        Debug.Log("Restarting the game...");

        // Reset time scale to 1
        Time.timeScale = 1;

        // Unload the pause menu if active
        if (SceneManager.GetSceneByName("PauseMenuScene").isLoaded)
        {
            LevelManager.Instance.DestroyLevelManager();

            SceneManager.UnloadSceneAsync("PauseMenuScene");
        }

        // Destroy persistent objects
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("MainObject"))
        {
            Destroy(obj);
        }

        // Reload the Unity Scene
        SceneManager.LoadScene("Unity Scene");
    }




    public void ResumeGame()
    {
        Debug.Log("Resuming Game...");
        Time.timeScale = 1; // Resume the game
        SceneManager.UnloadSceneAsync("PauseMenuScene"); // Unload the pause menu scene
    }
}
