using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text scoreText;

    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] GameObject gameOverPanel;

    int score = 0;
    int playerHP = 3;

    void Start()
    {
        Time.timeScale = 1f;
        UpdateHud();
        gameOverPanel.SetActive(false);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Final Score: " + score;
        Time.timeScale = 0f;
    }

    public void BrickDestroyed()
    {
        score++;
        UpdateHud();
    }

    public void BrickHitBottom()
    {
        playerHP -= 1;
        UpdateHud();
        if(playerHP <= 0)
        {
            GameOver();
        }
    }

    void UpdateHud()
    {
        healthText.text = "Health: " + playerHP;
        scoreText.text = "Score: " + score;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("PhysicsGame");
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {
        
    }
}
