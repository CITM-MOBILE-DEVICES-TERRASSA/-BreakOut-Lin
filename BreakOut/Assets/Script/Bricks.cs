using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    public int health;
    public int score;
    public bool isDestroyed;
    public Color brickcolor;
    public Vector3 startPosition;
    public bool hasPowerUp;
    public GameObject powerUp;
    private HUD hud;
    private List<GameObject> activePowerUps = new List<GameObject>();
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
                this.isDestroyed = true;
                Destroy(this.gameObject);
                GameManager.instance.bricksDestroyed +=1;
                if (this.hasPowerUp) {
                    GameObject newPowerUp = Instantiate(powerUp, this.transform.position, Quaternion.identity);
                    activePowerUps.Add(newPowerUp);
                }
            }
            else {
                this.health -= 1;
            }
        }

    }

    public void RemovePowerUp(GameObject powerUpToRemove)
    {
        if (activePowerUps.Contains(powerUpToRemove))
        {
            activePowerUps.Remove(powerUpToRemove);
            Destroy(powerUpToRemove); 
        }
    }

}
