using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public TMP_Text scoreText;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: 0";
        score = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = $"Score: {score}";
    }

    public int GetScore()
    {
        return score;
    }
}
