using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padding : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 5;
    private Vector2 screenBounds;
    private float squareWidth;
    private float squareHeight;

    float m_horizontal;
    void Start()
    {
        Camera mainCamera = Camera.main;
       
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        squareWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        m_horizontal = Input.GetAxis("Horizontal");

        Vector3 newPosition = transform.position + new Vector3(m_horizontal, 0, 0) * speed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, screenBounds.x * -1 + squareWidth, screenBounds.x - squareWidth);
       
        transform.position = newPosition;

    }
}
