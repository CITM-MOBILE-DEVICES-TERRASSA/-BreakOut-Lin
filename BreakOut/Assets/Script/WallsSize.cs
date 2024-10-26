using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsSize : MonoBehaviour
{
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


    void SetColliderPointsBasedOnScreen()
    {
        // Get the main camera reference
        Camera mainCamera = Camera.main;


        // Calculate the absolute distance of the camera from the scene along the Z-axis
        float cameraZDistance = Mathf.Abs(mainCamera.transform.position.z);

        // Get the world positions of the screen corners
        Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, cameraZDistance));
        Vector3 topLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, cameraZDistance));
        Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cameraZDistance));
        Vector3 bottomRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, cameraZDistance));

        // Create an array to hold the screen points in 2D space
        Vector2[] screenPoints = new Vector2[4]; 
        screenPoints[0] = new Vector2(bottomLeft.x, bottomLeft.y);  
        screenPoints[1] = new Vector2(topLeft.x, topLeft.y);        
        screenPoints[2] = new Vector2(topRight.x, topRight.y);     
        screenPoints[3] = new Vector2(bottomRight.x, bottomRight.y);

        // Update the edgePoint's collider points with the calculated screen points
        edgePoint.points = screenPoints;
    }
    void Update()
    {
        // Check if the screen dimensions have changed
        if (Screen.width != currentScreenWidth || Screen.height != currentScreenHeight)
        {
            // Update the current screen dimensions
            currentScreenWidth = Screen.width;
            currentScreenHeight = Screen.height;

            // Recalculate the collider points based on the new screen dimensions
            SetColliderPointsBasedOnScreen();
        }
    }
}
