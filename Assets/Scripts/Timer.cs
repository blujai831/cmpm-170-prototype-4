using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class Timer : MonoBehaviour
{
    public Image circleImage;
    public float totalTime = 5f;
    [SerializeField] private float timeRemaining;
    [SerializeField] RawImage[] strikes;
    private int numStrikes;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private Score scoreScript;

    void Start()
    {
        timeRemaining = totalTime;
        circleImage.fillAmount = 1f;

        numStrikes = 0;
        strikes[0].gameObject.SetActive(false);
        strikes[1].gameObject.SetActive(false);
        strikes[2].gameObject.SetActive(false);

        playAgainButton.onClick.AddListener(PlayAgain);
        playAgainButton.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        if (PlayerPrefs.GetInt("highscore") == 0) PlayerPrefs.SetInt("highscore", 0);
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            circleImage.fillAmount = timeRemaining / totalTime;
        }
        else
        {
            circleImage.fillAmount = 0f;
            AddStrike();
        }
    }

    void GameEnded()
    {
        playAgainButton.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);
        if(scoreScript.GetScore() > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", scoreScript.GetScore());
        }
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("highscore").ToString();

    }

    void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public void SetTimeRemaining(float timeSeconds){
        timeRemaining = timeSeconds;
    }

    public void ResetTimer()
    {
        timeRemaining = totalTime;
    }

    void AddStrike()
    {
        if(numStrikes <= 2)
        {
            strikes[numStrikes].gameObject.SetActive(true);
            numStrikes++;
            if(numStrikes < 2) ResetTimer();
        }
        else
        {
            GameEnded();
        }
    }
}
