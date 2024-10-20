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

        if (scoreText != null)
        {
            scoreText.text = "Score: "+ GameManager.instance.Score; // 更新文本内容为当前生命值
        }

        if (maxScoreText != null)
        {
            maxScoreText.text = "MaxScore: "+ GameManager.instance.MaxScore; // 更新文本内容为当前生命值
        }

        if (maxScoreText != null)
        {
            lifeText.text = "Life: "+ GameManager.instance.life; // 更新文本内容为当前生命值
        }
    }



}
