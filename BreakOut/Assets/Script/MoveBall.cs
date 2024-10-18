using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 3;
    public float maxVelocity = 15;
    public float speedIncrement = 0.5f;

    private float speedInici = 10f;
    private Vector2 screenBounds;
    private float squareHeight;

    private Vector2 velocity;

    private Rigidbody2D rb;
    void Start()
    {

        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        squareHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        rb = GetComponent<Rigidbody2D>();
        LaunchBall();
    }

    // Update is called once per frame
    void Update()
    {





        Vector3 newPosition = transform.position + new Vector3(0, 1, 0) * speed * Time.deltaTime;
        if (newPosition.y < screenBounds.y * -1 - squareHeight)
        {
            transform.position = Vector3.zero;
            speed = speedInici;
            rb.velocity = Vector3.zero;
            LaunchBall();
            Debug.Log("Die");
        }

        


    }
    void LaunchBall()
    {
        velocity.x = Random.Range(-1f, 1f);
        velocity.y = -1;
        rb.velocity = velocity.normalized * speed;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (speed >= maxVelocity)
        {
            speed = 15;
        }
        else {
            speed += speedIncrement;
            rb.velocity = rb.velocity.normalized * speed;
        }

        if (collision.gameObject.CompareTag("Brick")) { 
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("padding"))
        {
            // 生成一个0到1之间的随机数
            float randomChance = Random.value;

            // 如果随机数小于0.1（10%的概率），触发向左或向右的动作
            if (randomChance < 0.7f)
            {
                // 随机选择-1或1，使其向左或向右移动
                Debug.Log("Die");
                velocity.x = Random.Range(0, 2) == 0 ? -Mathf.Abs(velocity.x) : Mathf.Abs(velocity.x);
            }

            // 无论是否触发10%的概率，对y方向也增加一点小的随机扰动，避免死循环
            velocity.y += Random.Range(-0.1f, 0.1f); // 在y方向增加一点随机偏移
        }



    }
}
