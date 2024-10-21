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
    public float offsetY = 0.5f;
    private bool islaunch = false;
    public GameObject padding;
    private Rigidbody2D rb;

    private HUD hud;
    void Start()
    {

        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        squareHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(AutoLaunchBall(2f));
        hud = FindObjectOfType<HUD>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position + new Vector3(0, 1, 0) * speed * Time.deltaTime;
        if (newPosition.y < screenBounds.y * -1 - squareHeight)
        {
            ResetBall();
            hud.lifeReduce();
        }

        //const float minYVelocity = 0.5f;

        //if (Mathf.Abs(rb.velocity.y) < minYVelocity)
        //{
        //    // 如果y方向的速度太小，强制增加y方向的速度
        //    rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * minYVelocity);
        //}


        if (!islaunch && padding != null)
        {
            transform.position = new Vector3(padding.transform.position.x, padding.transform.position.y + offsetY, transform.position.z);
        }

        if (!islaunch && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            LaunchBall();
        }
    }

    public void ResetBall() {
        transform.position = Vector3.zero;
        speed = speedInici;
        rb.velocity = Vector3.zero;
        islaunch = false;
        StartCoroutine(AutoLaunchBall(2f));
    }
    void LaunchBall()
    {
        velocity.x = Random.Range(0, 2) == 0 ? -1 : 1;
        velocity.y = 1;
        rb.velocity = velocity.normalized * speed;
        islaunch = true; 
    }
    private IEnumerator AutoLaunchBall(float delay)
    {
        yield return new WaitForSeconds(delay); // 等待指定的延迟时间
        if (!islaunch) // 只有在未发射的情况下才发射
        {
            LaunchBall();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        const float minYVelocity = 0.5f;

        if (Mathf.Abs(rb.velocity.y) < minYVelocity)
        {
            // 设置y方向的最小速度，保持球不会卡死在水平运动
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * minYVelocity);
            rb.gravityScale = 10;
    
            Debug.Log("Y DANGER");
        }
        else {

            rb.gravityScale = 0;
        }

        if (speed >= maxVelocity)
        {
            speed = 15;
        }
        else {
            speed += speedIncrement;
            rb.velocity = rb.velocity.normalized * speed;
        }
    }
}
