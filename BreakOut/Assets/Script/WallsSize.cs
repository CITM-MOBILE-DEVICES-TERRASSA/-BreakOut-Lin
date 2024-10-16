using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsSize : MonoBehaviour
{
    // Start is called before the first frame update
    private EdgeCollider2D edgePoint;

    private int currentScreenWidth;
    private int currentScreenHeight;
    void Start()
    {
        edgePoint = GetComponent<EdgeCollider2D>();

        currentScreenWidth = Screen.width;
        currentScreenHeight = Screen.height;


        SetColliderPointsBasedOnScreen();
    }

    // Update is called once per frame

    void SetColliderPointsBasedOnScreen()
    {
       
        Camera mainCamera = Camera.main;


        
        float cameraZDistance = Mathf.Abs(mainCamera.transform.position.z);

       
        Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, cameraZDistance));
        Vector3 topLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, cameraZDistance));
        Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cameraZDistance));
        Vector3 bottomRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, cameraZDistance));

       
        Vector2[] screenPoints = new Vector2[4]; 
        screenPoints[0] = new Vector2(bottomLeft.x, bottomLeft.y);  
        screenPoints[1] = new Vector2(topLeft.x, topLeft.y);        
        screenPoints[2] = new Vector2(topRight.x, topRight.y);     
        screenPoints[3] = new Vector2(bottomRight.x, bottomRight.y);

       
        edgePoint.points = screenPoints;
    }
    void Update()
    {
        if (Screen.width != currentScreenWidth || Screen.height != currentScreenHeight)
        {
            
            currentScreenWidth = Screen.width;
            currentScreenHeight = Screen.height;

            
            SetColliderPointsBasedOnScreen();
        }
    }
}
