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
        //// ��ȡ TextMeshPro ���
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        UpdateHealthDisplay(); // ��ʼ��ʱ������ʾ
       
    }

    void Start()
    {
        hud = FindObjectOfType<HUD>();
    }
    public void UpdateHealthDisplay()
    {
        if (textMesh != null)
        {
            textMesh.text = (this.health + 1).ToString(); // �����ı�����Ϊ��ǰ����ֵ
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
