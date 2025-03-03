using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoinCollector : MonoBehaviour
{
    public TMP_Text scoreText; 
    private int score = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject); 
            score++; 
            UpdateScoreText(); 
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}