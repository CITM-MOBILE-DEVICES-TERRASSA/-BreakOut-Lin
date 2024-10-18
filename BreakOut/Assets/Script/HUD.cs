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
    public int Score;
    private int MaxScore;
    private int life = 3;


    void Start()
    {
    }


    public void AddScore(int amount)
    {
        Score += amount;
    }

    public void lifeReduce()
    {
        life -= 1;
    }

    // Update is called once per frame
    void Update()
    {

        if (scoreText != null)
        {
            scoreText.text = "Score: "+ Score; // �����ı�����Ϊ��ǰ����ֵ
        }

        if (maxScoreText != null)
        {
            maxScoreText.text = "MaxScore: "+ MaxScore; // �����ı�����Ϊ��ǰ����ֵ
        }

        if (maxScoreText != null)
        {
            lifeText.text = "Life: "+ life; // �����ı�����Ϊ��ǰ����ֵ
        }
    }



}
