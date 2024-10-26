using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

public class LevelGenerator : MonoBehaviour
{

    public float spacing = 0.1f;
    public float wallWidthRatio = 0.8f;
    public float wallHeightRatio = 0.3f;
    public bool finishChangeLevel = true;
    public static LevelGenerator instance;
    public Gradient gradient;
    public MoveBall moveBall;
    public Bricks bricks;
    public UIManager uimanager;
    public Vector2Int size;
    public GameObject brickPrefab;
    public GameObject nextPanel_PC;
    public GameObject nextPanel_H;
    public GameObject nextPanel_V;

    private void Awake()
    {

        string path = Path.Combine(Application.persistentDataPath, Application.dataPath + "\\Script\\SaveData\\savegame.json");

        //Check is new game or need continue
        if (GameManager.instance.isNewGame)
        {
            //is new game
            NewGame();
        }
        else
        {
            // continue game
            LoadGame(path);
        }

    }


    private void LoadGame(string filePath)
    {
        string json = File.ReadAllText(filePath);
        GameData gameData = JsonUtility.FromJson<GameData>(json);

        // Clear the current bricks (if needed)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize * 2;
        int numRows, numCols;

        // Adjust the number of bricks based on landscape or portrait mode
        numRows = (screenWidth > screenHeight) ? size.y : size.x;
        numCols = (screenWidth > screenHeight) ? size.x : size.y;

        // Calculate the width and height of the wall based on the screen size
        float wallWidthRatio = 0.8f;
        float wallHeightRatio = 0.30f;

        float wallWidth = screenWidth * wallWidthRatio;
        float wallHeight = screenHeight * wallHeightRatio;

        // Calculate the width and height of the bricks, taking spacing into account
        float brickWidth = (wallWidth - (numCols - 1) * spacing) / numCols;
        float brickHeight = (wallHeight - (numRows - 1) * spacing) / numRows;

        // Create bricks based on the saved data
        foreach (GameData.WallData wallData in gameData.walls)
        {
            GameObject newBrick = Instantiate(brickPrefab, transform);
            newBrick.transform.localScale = new Vector3(brickWidth, brickHeight, 1);
            newBrick.transform.position = GameManager.instance.startPosition + new Vector3(wallData.position.x, wallData.position.y, 0); // 使用保存的数据设置位置

            // Get or add the Brick script and set the life value
            Bricks brick = newBrick.GetComponent<Bricks>();
            if (brick == null)
            {
                brick = newBrick.AddComponent<Bricks>();
            }
            brick.health = wallData.health;
            brick.score = wallData.blockScore;
            brick.isDestroyed = wallData.isDestroyed;
            brick.GetComponent<SpriteRenderer>().color = wallData.brickColor;
            brick.brickcolor = wallData.brickColor;
            brick.hasPowerUp = wallData.hasPowerUp;
        }

        // Retrieve the stored information data
        GameManager.instance.life = gameData.playerLives;
        GameManager.instance.MaxScore = gameData.Maxscore;
        GameManager.instance.Score = gameData.score;
        GameManager.instance.bricksDestroyed = gameData.blockisDestroyed;
        GameManager.instance.level = gameData.level;

    }




    private void NewGame()
    {
        Debug.Log("NEWGAME");


        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize * 2;

        // Adjust the number of bricks based on landscape or portrait mode
        int numRows = (screenWidth > screenHeight) ? size.y : size.x;
        int numCols = (screenWidth > screenHeight) ? size.x : size.y;

        // Calculate the width and height of the wall based on the screen size
        float wallWidthRatio = 0.8f;
        float wallHeightRatio = 0.30f;

        float wallWidth = screenWidth * wallWidthRatio;
        float wallHeight = screenHeight * wallHeightRatio;

        // Calculate the width and height of the bricks, taking spacing into account
        float brickWidth = (wallWidth - (numCols - 1) * spacing) / numCols;
        float brickHeight = (wallHeight - (numRows - 1) * spacing) / numRows;

        // Calculate the starting position of the bricks to center the entire wall on the screen
        Vector3 startPosition = transform.position - new Vector3(wallWidth / 2 - brickWidth / 2, wallHeight / 2 - brickHeight / 2, 0);

        // Create bricks
        for (int i = 0; i < numCols; i++)
        {
            for (int j = 0; j < numRows; j++)
            {
                GameObject newBrick = Instantiate(brickPrefab, transform);
                newBrick.transform.localScale = new Vector3(brickWidth, brickHeight, 1); // 
                newBrick.transform.position = startPosition + new Vector3(i * (brickWidth + spacing), j * (brickHeight + spacing), 0);
                newBrick.GetComponent<SpriteRenderer>().color = gradient.Evaluate((float)j / (numRows - 1));

                Debug.Log("Color" + newBrick.GetComponent<SpriteRenderer>().color);

                // Get or add the Brick script and set the initial life value
                Bricks brick = newBrick.GetComponent<Bricks>();
                if (brick == null)
                {
                    brick = newBrick.AddComponent<Bricks>();
                }

                if (GameManager.instance.level == 1)
                {
                    brick.health = j;
                    brick.score = j;
                }
                else
                {
                    brick.health = j * 2;
                    brick.score = j + 2;
                }
                brick.brickcolor = gradient.Evaluate((float)j / (numRows - 1));
                brick.startPosition = startPosition;
                brick.hasPowerUp = Random.value < 0.2f; // A 20% chance to receive a drop power-up
            }
        }

        instance = this;
    }




    void Start()
    {
        moveBall = FindObjectOfType<MoveBall>();
        bricks = FindObjectOfType<Bricks>();
        uimanager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        //Get the number of bricks and compare it with the eliminated bricks; if the numbers match, proceed to the next level
        if ((size.x * size.y) == GameManager.instance.bricksDestroyed)
        {
            if (GameManager.instance.level == 1)
            {
                GameManager.instance.level = 2;
            }
            else
            {
                GameManager.instance.level = 1;
            }
            finishChangeLevel = false;
            GameManager.instance.bricksDestroyed = 0;

            // show nextPanel in diferent mode
            if (Screen.dpi == 96)
            {
                nextPanel_PC.SetActive(true);
                nextPanel_H.SetActive(false);
                nextPanel_V.SetActive(false);
            }
            else
            {
                CheckScreenOrientation();
            }
            GameManager.instance.PauseGame();

        }


        //Check Screen Orientation
        void CheckScreenOrientation()
        {

            if (Screen.width > Screen.height)
            {
                nextPanel_H.SetActive(true);
                nextPanel_V.SetActive(false);
            }
            else
            {

                nextPanel_H.SetActive(false);
                nextPanel_V.SetActive(true);
            }
        }
        //Change to new game and save the game
        if (!finishChangeLevel)
        {
            NewGame();
            GameManager.instance.SaveGame();
            moveBall.ResetBall();
            finishChangeLevel = true;
        }



    }
}
