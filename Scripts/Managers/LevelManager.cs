using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private int finalLevelIndex = 6;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void GoToNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (GameManager.Instance.bHasFinishedGame)
        {
            LoadRandomPlayableLevel(currentSceneIndex);
        }
        else
        {
            if (currentSceneIndex < finalLevelIndex)
            {
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
            else
            {
                GameManager.Instance.bHasFinishedGame = true;
                LoadRandomPlayableLevel(currentSceneIndex);
            }
        }
    }

    private void LoadRandomPlayableLevel(int currentLevelIndex)
    {
        int randomLevelToPlay = Random.Range(2, 7);
        while (currentLevelIndex == randomLevelToPlay)
        {
            randomLevelToPlay = Random.Range(2, 7);
        }
        SceneManager.LoadScene(randomLevelToPlay);
    }
}
