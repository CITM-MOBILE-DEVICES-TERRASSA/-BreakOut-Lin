using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI scoreText;    
    public TextMeshProUGUI maxScoreText;
    public TextMeshProUGUI lifeText;



    void Start()
    {
    }


    public void AddScore(int amount)
    {
        GameManager.instance.Score += amount;
    }

    public void lifeReduce()
    {
        GameManager.instance.life -= 1;
        if(GameManager.instance.life <= 0) {
            GameManager.instance.GameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {

        
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        
        int fontSize = Mathf.RoundToInt(Mathf.Min(screenWidth, screenHeight) * 0.05f);

        if (scoreText != null)
        {
            scoreText.text = "Score: " + GameManager.instance.Score;
            scoreText.fontSize = fontSize; 
        }

        if (maxScoreText != null)
        {
            maxScoreText.text = "MaxScore: " + GameManager.instance.MaxScore; 
            maxScoreText.fontSize = fontSize; 
        }

        if (lifeText != null)
        {
            lifeText.text = "Life: " + GameManager.instance.life; 
            lifeText.fontSize = fontSize; 
        }
    }



}
