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
    void Start()
    {

        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        squareHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        rb = GetComponent<Rigidbody2D>();
        //LaunchBall();
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
            Debug.Log("Die");
            islaunch = false;
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
    void LaunchBall()
    {
        velocity.x = Random.Range(-1f, 1f);
        velocity.y = 1;
        rb.velocity = velocity.normalized * speed;
        islaunch = true; 
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

        //if (collision.gameObject.CompareTag("Brick")) {

        //    Destroy(collision.gameObject);
        //}

    }
}
