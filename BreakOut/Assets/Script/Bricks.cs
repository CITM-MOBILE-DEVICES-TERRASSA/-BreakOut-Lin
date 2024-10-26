using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    public int health;
    public int score;
    public bool isDestroyed;
    public bool hasPowerUp;
    public Color brickcolor;
    public Vector3 startPosition;
    public GameObject powerUp;
    private HUD hud;
    private List<GameObject> activePowerUps = new List<GameObject>();
    private TextMeshProUGUI textMesh;
    private void Awake()
    {
 
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        UpdateHealthDisplay();
       
    }

    void Start()
    {
        hud = FindObjectOfType<HUD>();
        if (hud == null)
        {
            Debug.LogError("HUD not found in the scene!");
        }
    }
    public void UpdateHealthDisplay()
    {
        //show brick health
        if (textMesh != null)
        {
            textMesh.text = (this.health + 1).ToString(); 
        }
    }

    void Update()
    {
        UpdateHealthDisplay();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       //Compare Tag if is ball
        if (collision.gameObject.CompareTag("ball"))
        {
            if (this.health <= 0)//if brick health is 0, Destroyed
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
            else {// else brick health -1
                this.health -= 1;
            }
        }

    }

    //Remove powerUp
    public void RemovePowerUp(GameObject powerUpToRemove)
    {
        if (activePowerUps.Contains(powerUpToRemove))
        {
            activePowerUps.Remove(powerUpToRemove);
            Destroy(powerUpToRemove); 
        }
    }

}
