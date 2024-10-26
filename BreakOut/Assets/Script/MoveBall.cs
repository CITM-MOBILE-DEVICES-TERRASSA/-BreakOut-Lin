using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
    public float speed = 3;
    public float maxVelocity = 15;
    public float speedIncrement = 0.5f;
    public float offsetY = 0.35f;
    public GameObject padding;
    public AudioSource audioSource;

    private float speedInici = 10f;
    private float squareHeight;
    private bool islaunch = false;
    private Vector2 velocity;
    private Vector2 screenBounds;
    private Rigidbody2D rb;
    private HUD hud;

    void Start()
    {
        // Get the screen boundaries
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // Adjust the ball size based on the screen height to ensure adaptability across different screens.
        float targetBallSize = screenBounds.y * 0.07f; 
        float currentBallSize = transform.GetComponent<SpriteRenderer>().bounds.size.y;
        float scaleFactor = targetBallSize / currentBallSize;
        transform.localScale = new Vector3(scaleFactor * transform.localScale.x, scaleFactor * transform.localScale.y, transform.localScale.z);

        squareHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(AutoLaunchBall(2f));
        hud = FindObjectOfType<HUD>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 newPosition = transform.position + new Vector3(0, 1, 0) * speed * Time.deltaTime;
        //Whether the ball has fallen to the bottom of the game screen
        if (newPosition.y < screenBounds.y * -1 - squareHeight)
        {
            ResetBall();
            hud.lifeReduce();
        }
        // ball on padding
        if (!islaunch && padding != null)
        {
            transform.position = new Vector3(padding.transform.position.x, padding.transform.position.y + offsetY, transform.position.z);
        }
        //lauch game
        if (!islaunch && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            LaunchBall();
        }
    }

    public void ResetBall()
    {
        //Reset ball
        transform.position = Vector3.zero;
        speed = speedInici;
        rb.velocity = Vector3.zero;
        islaunch = false;
        StartCoroutine(AutoLaunchBall(2f));
    }

    void LaunchBall()
    {
        //launch Ball
        velocity.x = Random.Range(0, 2) == 0 ? -1 : 1;
        velocity.y = 1;
        rb.velocity = velocity.normalized * speed;
        islaunch = true;
    }

    private IEnumerator AutoLaunchBall(float delay)
    {
        //wait 2 sec if player no start game
        yield return new WaitForSeconds(delay);
        if (!islaunch)
        {
            LaunchBall();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //ball FX
        if (islaunch)
        {
            audioSource.Play();
        }
        //Prevent the ball from moving horizontally indefinitely
        const float minYVelocity = 0.5f;
        if (Mathf.Abs(rb.velocity.y) < minYVelocity && islaunch == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * minYVelocity);
            rb.gravityScale = 5;
        }
        else
        {
            rb.gravityScale = 0;
        }

        // Max ball speed
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
