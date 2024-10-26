using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Padding : MonoBehaviour
{
    public float speed = 5f;
    public bool isAutoMode = false;        
    public Bricks bricks;
    public Transform ballTransform;        
    private float squareWidth; 
    private Vector2 screenBounds;  
                

   
    void Start()
    {
       
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // Calculate and set the width and height ratio of the barrier
        float targetWidth = screenBounds.x * 0.2f;   
        float targetHeight = screenBounds.y * 0.05f; 

        
        float currentWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x;
        float currentHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y;

        // Calculate the scaling ratios for the horizontal and vertical directions
        float scaleFactorX = targetWidth / currentWidth;
        float scaleFactorY = targetHeight / currentHeight;

       
        transform.localScale = new Vector3(scaleFactorX * transform.localScale.x, scaleFactorY * transform.localScale.y, transform.localScale.z);

        // Update the width of the barrier for subsequent boundary limitations
        squareWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    void Update()
    {
        // If automatic mode is enabled, automatically track the ball's position
        if (isAutoMode && ballTransform != null)
        {
            AutoMoveToBall();
        }
        else
        {
            // Otherwise, use the player's input to move
            PlayerControl();
        }


    }

    void PlayerControl()
    {
       
        float m_horizontal = Input.GetAxis("Horizontal");

        // If there is touch input, use the touch position to control the barrier
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            // Keep the y-axis position unchanged, and only update the x-axis
            touchPosition.y = transform.position.y;
            transform.position = new Vector3(touchPosition.x, touchPosition.y, transform.position.z);
        }
        else
        {
            // Default to using keyboard controls
            Vector3 newPosition = transform.position + new Vector3(m_horizontal, 0, 0) * speed * Time.deltaTime;

            // Restrict the barrier to move within the screen
            newPosition.x = Mathf.Clamp(newPosition.x, screenBounds.x * -1 + squareWidth, screenBounds.x - squareWidth);
            transform.position = newPosition;
        }
    }


    void AutoMoveToBall()
    {
        float randomOffsetX = Random.Range(-0.5f, 0.5f);

        // Automatically track the ball's position, moving only along the horizontal axis
        Vector3 targetPosition = new Vector3(ballTransform.position.x, transform.position.y, transform.position.z);

        // Interpolate to move to the target position, ensuring smooth movement
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Limit the barrier to move within the screen
        float clampedX = Mathf.Clamp(transform.position.x, screenBounds.x * -1 + squareWidth, screenBounds.x - squareWidth);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("powerUp"))
        {
            GameManager.instance.life += 1;
            // Get the Bricks script associated with the power-up
            Bricks bricks = collider.gameObject.GetComponent<Bricks>();
            if (bricks != null)
            {
                // Call the RemovePowerUp method and pass in the current powerUp object
                bricks.RemovePowerUp(collider.gameObject);
            }

            // Here, you can choose to either directly destroy the power-up or retain management (if there are other logics to handle)
            Destroy(collider.gameObject);
        }
    }
}
