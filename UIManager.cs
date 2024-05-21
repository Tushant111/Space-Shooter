using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _ScoreText, _bestScore;

    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private TMP_Text _restartText;

    private GameManager _gameManager;
    public int score; // Current player score

    // High score variables
    private int _highScore;
    private string _highScoreKey = "HighScore"; // Key to store high score in PlayerPrefs

    void Start()
    {
        _ScoreText.text = "Score: 0";
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GAMEMANAGER IS NULL");
        }

        // Load high score from PlayerPrefs
        _highScore = PlayerPrefs.GetInt(_highScoreKey, 0);
        _bestScore.text = "Best: " + _highScore.ToString();
    }

    public void UpdateScore(int playerScore)
    {
        score = playerScore;
        _ScoreText.text = "Score: " + score.ToString();

        // Check for new high score
        CheckForBestScore();
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];
        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void CheckForBestScore()
    {
        if (score > _highScore)
        {
            _highScore = score;
            _bestScore.text = "Best: " + _highScore.ToString();
            PlayerPrefs.SetInt(_highScoreKey, _highScore); // Save high score to PlayerPrefs
        }
    }
}

