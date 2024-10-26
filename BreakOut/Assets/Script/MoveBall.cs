using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
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

    AudioSource audioSource;
    private HUD hud;

    void Start()
    {
        // 获取屏幕边界
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // 根据屏幕高度调整球的大小，确保在不同屏幕上自适应
        float targetBallSize = screenBounds.y * 0.07f; // 球的高度为屏幕高度的5%
        float currentBallSize = transform.GetComponent<SpriteRenderer>().bounds.size.y;
        float scaleFactor = targetBallSize / currentBallSize;
        transform.localScale = new Vector3(scaleFactor * transform.localScale.x, scaleFactor * transform.localScale.y, transform.localScale.z);

        // 初始化其他属性
        squareHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(AutoLaunchBall(2f));
        hud = FindObjectOfType<HUD>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 newPosition = transform.position + new Vector3(0, 1, 0) * speed * Time.deltaTime;
        if (newPosition.y < screenBounds.y * -1 - squareHeight)
        {
            ResetBall();
            hud.lifeReduce();
        }

        if (!islaunch && padding != null)
        {
            transform.position = new Vector3(padding.transform.position.x, padding.transform.position.y + offsetY, transform.position.z);
        }

        if (!islaunch && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            LaunchBall();
        }
    }

    public void ResetBall()
    {
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
        yield return new WaitForSeconds(delay);
        if (!islaunch)
        {
            LaunchBall();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (islaunch)
        {
            audioSource.Play();
        }

        const float minYVelocity = 0.5f;
        if (Mathf.Abs(rb.velocity.y) < minYVelocity && islaunch == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * minYVelocity);
            rb.gravityScale = 5;
            Debug.Log("Y DANGER");
        }
        else
        {
            rb.gravityScale = 0;
        }

        if (speed >= maxVelocity)
        {
            speed = maxVelocity;
        }
        else
        {
            speed += speedIncrement;
            rb.velocity = rb.velocity.normalized * speed;
        }
    }
}
