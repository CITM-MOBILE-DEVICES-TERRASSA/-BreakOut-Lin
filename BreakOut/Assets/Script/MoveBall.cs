using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 5;
    private Vector2 screenBounds;
    private float squareWidth;
    private float squareHeight;
    void Start()
    {
        Camera mainCamera = Camera.main;

        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        squareWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        squareHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
       

       
        Vector3 newPosition = transform.position + new Vector3(0, 0, 0) * speed * Time.deltaTime;

       
        newPosition.x = Mathf.Clamp(newPosition.x, screenBounds.x * -1 + squareWidth, screenBounds.x - squareWidth);
        if (newPosition.y > screenBounds.y - squareHeight)
        {
            newPosition.y = screenBounds.y - squareHeight;
           
        }

        if (newPosition.y < screenBounds.y * -1 - squareHeight) {
            Debug.Log("Die");
        }


            transform.position = newPosition;
    }
}
