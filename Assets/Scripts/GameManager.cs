using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // PLAYER STATS
    private int currentScore = 0;
    private int lives = 3;
    private int maxBombs = 1;
    private int explodeRange = 1;
    private bool isPaused = false;
    private float moveSpeed = 4f;

    [SerializeField] private int bombLimit = 6;
    [SerializeField] private int explodeLimit = 5;
    [SerializeField] private float speedLimit = 6.0f;
    private float speedIncrease = 0.4f;
    
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
    [SerializeField] private Text speedText;

    private int enemiesThisLevel = 0;

    [SerializeField] private bool isLastLevel = false;

	// Const string variables for our PlayerPrefs Keys
	private const string CurrentScoreKey = "CurrentScore";
	private const string LivesKey = "Lives";
	private const string MaxBombsKey = "MaxBombs";
	private const string ExplodeRangeKey = "ExplodeRange";
	private const string MoveSpeedKey = "MoveSpeed";

    private void Awake()
    {
	    LoadPlayerPrefs();
    }

    void Start()
    {
        UpdateScore(0);
        UpdateLives();
        UpdateBombsText();
        UpdateExplodeRangeText();
        UpdateSpeedText();
        SpawnPlayer();
        
        enemiesThisLevel = GetEnemyCount();
    }

    public void PlayerDied()
    {
        if (lives > 1)
        {
            lives--;
            
            // Spawn new player
            Invoke("SpawnPlayer", currentPlayer.GetDestroyDelayTime() + delayToSpawnPlayer);
        }
        else
        {
            Invoke("GameOver", 3.0f);
            lives = 0;
            UpdateLives();
        }
    }

    private void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity, playerParentTransform);
        currentPlayer = player.GetComponent<PlayerController>();
        currentPlayer.InitializePlayer(maxBombs, moveSpeed);
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

    public void UpdateBombsText()
    {
        bombsText.text = "Bombs: " + maxBombs.ToString("D2");
    }

    private void UpdateExplodeRangeText()
    {
        explodeRangeText.text = "Range: " + explodeRange.ToString("D2");
    }
    
    private void UpdateSpeedText()
    {
        speedText.text = "Speed: " + moveSpeed.ToString();
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

    private void DisplayWinPanel()
    {
	    winGamePanel.SetActive(true);
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
            if (isLastLevel)
            {
	            // Tell the player to play his victory animation
	            currentPlayer.PlayVictory();
	            Invoke("DisplayWinPanel", 3.0f);
            }
            else
            {
	            // Tell the player to play his victory animation
	            currentPlayer.PlayVictory();
	            // Save all of the player data PlayerPrefs so it can be loaded in the next level
	            SavePlayerData();
	            
	            Invoke("LoadNextLevel", 3.0f);
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
        maxBombs = Mathf.Clamp(maxBombs, 1, bombLimit);
        UpdateBombsText();
        currentPlayer.InitializePlayer(maxBombs, moveSpeed);
    }

    public void IncreaseSpeed()
    {
        moveSpeed += speedIncrease;
        moveSpeed = Mathf.Clamp(moveSpeed, 4f, speedLimit);
        UpdateSpeedText();
        currentPlayer.InitializePlayer(maxBombs, moveSpeed);
    }
    
    public int GetExplodeRange()
    {
        return explodeRange;
    }

    public void IncreaseExplodeRange()
    {
        explodeRange++;
        explodeRange = Mathf.Clamp(explodeRange, 1, explodeLimit);
        UpdateExplodeRangeText();
    }

    private void SavePlayerData()
    {
	    PlayerPrefs.SetInt(CurrentScoreKey, currentScore);
	    PlayerPrefs.SetInt(LivesKey, lives);
	    PlayerPrefs.SetInt(MaxBombsKey, maxBombs);
	    PlayerPrefs.SetInt(ExplodeRangeKey, explodeRange);
	    PlayerPrefs.SetFloat(MoveSpeedKey, moveSpeed);
    }

    private void LoadPlayerPrefs()
    {
	    if (PlayerPrefs.HasKey(CurrentScoreKey))
	    {
		    currentScore = PlayerPrefs.GetInt(CurrentScoreKey);
	    }
	    if (PlayerPrefs.HasKey(LivesKey))
	    {
		    lives = PlayerPrefs.GetInt(LivesKey);
	    }
	    if (PlayerPrefs.HasKey(MaxBombsKey))
	    {
		    maxBombs = PlayerPrefs.GetInt(MaxBombsKey);
	    }
	    if (PlayerPrefs.HasKey(ExplodeRangeKey))
	    {
		    explodeRange = PlayerPrefs.GetInt(ExplodeRangeKey);
	    }
	    if (PlayerPrefs.HasKey(MoveSpeedKey))
	    {
		    moveSpeed = PlayerPrefs.GetFloat(MoveSpeedKey);
	    }
    }
}
