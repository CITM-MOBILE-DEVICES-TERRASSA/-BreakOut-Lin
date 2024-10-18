using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    public int health;
    public int score;
    private HUD hud;

    private TextMeshProUGUI textMesh;
    private void Awake()
    {
        //// 获取 TextMeshPro 组件
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        UpdateHealthDisplay(); // 初始化时更新显示
       
    }

    void Start()
    {
        hud = FindObjectOfType<HUD>();
    }
    public void UpdateHealthDisplay()
    {
        if (textMesh != null)
        {
            textMesh.text = (this.health + 1).ToString(); // 更新文本内容为当前生命值
        }
    }

    void Update()
    {
        UpdateHealthDisplay();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("ball"))
        {
            if (this.health <= 0)
            {
                hud.AddScore(score+1);
                Destroy(this.gameObject);
            }
            else {
                this.health -= 1;
            }
        }
    }

}
