using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    public int health;
    private TextMeshProUGUI textMesh;
    private void Awake()
    {
        //// ��ȡ TextMeshPro ���
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        UpdateHealthDisplay(); // ��ʼ��ʱ������ʾ
    }

    // ��ש���ܵ�ײ��ʱ���ã���������ֵ
    public void TakeDamage(int damage)
    {
        //health -= damage;
        //UpdateHealthDisplay(); // ������ʾ
        //if (health <= 0)
        //{
        //    Destroy(gameObject); // ������ֵΪ0�����ʱ����ש��
        //}
    }

    // ��������ֵ��ʾ
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
                Destroy(this.gameObject);
            }
            else {
                this.health -= 1;
            }
        }
    }

}
