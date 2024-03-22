using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int lives = 3;
    private int currentScore = 0;
    private int maxBombs = 1;
    private int explodeRange = 1;
    private bool isPaused = false;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winGamePanel;
    
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerParentTransform;

    private PlayerController currentPlayer;

    [SerializeField] private float delayToSpawnPlayer = 1f;
    [SerializeField] private CameraController myCamera;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text bombsText;
    [SerializeField] private Text explodeRangeText;

    private int enemiesThisLevel = 0;

    [SerializeField] private bool isLastLevel = false;
    
    void Start()
    {
        UpdateScore(0);
        UpdateLives();
        UpdateBombs();
        UpdateExplodeRange();
        SpawnPlayer();
        
        enemiesThisLevel = GetEnemyCount();
    }

    public void PlayerDied()
    {
        if (lives > 1)
        {
            Debug.Log("GameManager: Player has died.");
            lives--;
            
            // Spawn new player
            Invoke("SpawnPlayer", currentPlayer.GetDestroyDelayTime() + delayToSpawnPlayer);
        }
        else
        {
            GameOver();
            lives = 0;
            UpdateLives();
        }
    }

    private void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity, playerParentTransform);
        currentPlayer = player.GetComponent<PlayerController>();
        currentPlayer.InitializePlayer(maxBombs);
        myCamera.SetPlayer(player);
        UpdateLives();
    }

    public void UpdateScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        scoreText.text = "Score: " + currentScore.ToString("D6");
    }

    private void UpdateLives()
    {
        livesText.text = "Lives: " + lives.ToString("D2");
    }

    public void UpdateBombs()
    {
        bombsText.text = "Bombs: " + maxBombs.ToString("D2");
    }

    private void UpdateExplodeRange()
    {
        explodeRangeText.text = "Range: " + explodeRange.ToString("D2");
    }
    
    public int GetExplodeRange()
    {
        return explodeRange;
    }

    public void IncreaseExplodeRange()
    {
        explodeRange++;
    }

    public void PauseButton()
    {
        if (isPaused)
        {
            pausePanel.SetActive(false);
            currentPlayer.SetPaused(false);
            isPaused = false;
            Time.timeScale = 1f;
        }
        else
        {
            pausePanel.SetActive(true);
            currentPlayer.SetPaused(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private int GetEnemyCount()
    {
        int count = GameObject.FindGameObjectsWithTag("Enemy").Length;
        return count;
    }

    public void EnemyHasDied()
    {
        enemiesThisLevel--;

        if (enemiesThisLevel <= 0)
        {
            if (!isLastLevel)
            {
                LoadNextLevel();
            }
            else
            {
                winGamePanel.SetActive(true);
            }
        }
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void IncreaseMaxBombs()
    {
        maxBombs++;
        currentPlayer.InitializePlayer(maxBombs);
    }
}
