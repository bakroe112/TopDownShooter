using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public int currentScore = 0;
    
    private void Start()
    {
        if (scoreText == null)
        {
            GameObject numberObject = GameObject.Find("CoinNumText");
            if (numberObject != null)
            {
                scoreText = numberObject.GetComponent<Text>();
                Debug.Log("scoreText" + scoreText);
            }
        }
        UpdateScoreDisplay();
    }
    
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
    }
    
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }
}
