using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

public class LevelGenerator : MonoBehaviour
{
    public Vector2Int size; // 横向砖块数量（x）和纵向砖块数量（y）
    public GameObject brickPrefab;
    public float spacing = 0.1f; // 用于设置砖块之间的间距比例
    public float wallWidthRatio = 0.8f; // 墙的宽度占屏幕宽度的比例（如0.8表示80%）
    public float wallHeightRatio = 0.3f; // 墙的高度占屏幕高度的比例（如0.3表示30%）
    public Gradient gradient;
    public MoveBall moveBall;
    public Bricks bricks;
    public bool finishChangeLevel = true;
    public static LevelGenerator instance; // 用于全局访问
    public UIManager uimanager;
    public GameObject nextPanel_PC;
    public GameObject nextPanel_H;
    public GameObject nextPanel_V;

    private void Awake()
    {

        string path = Path.Combine(Application.persistentDataPath, Application.dataPath + "\\Script\\SaveData\\savegame.json");

        if (GameManager.instance.isNewGame)
        {
            NewGame();
        }
        else {
           
            LoadGame(path);
        }

    }


    private void LoadGame(string filePath)
    {
        string json = File.ReadAllText(filePath);
        GameData gameData = JsonUtility.FromJson<GameData>(json);

        // 清空当前砖块（如果需要）
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize * 2;
        int numRows, numCols;

        // 根据横竖屏模式选择砖块数量
         numRows = (screenWidth > screenHeight) ? size.y : size.x; // 横屏时行数使用size.y，竖屏时使用size.x
         numCols = (screenWidth > screenHeight) ? size.x : size.y; // 横屏时列数使用size.x，竖屏时使用size.y

        // 根据屏幕大小计算墙的宽高
        float wallWidthRatio = 0.8f; // 墙宽度占屏幕80%
        float wallHeightRatio = 0.30f; // 墙高度占屏幕25%（修改为原来的50%的一半）

        float wallWidth = screenWidth * wallWidthRatio;
        float wallHeight = screenHeight * wallHeightRatio;

        // 计算砖块的宽度和高度，同时考虑间距
        float brickWidth = (wallWidth - (numCols - 1) * spacing) / numCols;
        float brickHeight = (wallHeight - (numRows - 1) * spacing) / numRows;

        // 根据保存的数据创建砖块
        foreach (GameData.WallData wallData in gameData.walls)
        {
            GameObject newBrick = Instantiate(brickPrefab, transform);
            newBrick.transform.localScale = new Vector3(brickWidth, brickHeight, 1); // 调整砖块的尺寸
            newBrick.transform.position = GameManager.instance.startPosition + new Vector3(wallData.position.x, wallData.position.y, 0); // 使用保存的数据设置位置

            // 获取或添加 Brick 脚本并设置生命值
            Bricks brick = newBrick.GetComponent<Bricks>();
            if (brick == null)
            {
                brick = newBrick.AddComponent<Bricks>();
            }
            brick.health = wallData.health;
            brick.score = wallData.blockScore;
            brick.isDestroyed = wallData.isDestroyed; // 根据是否被摧毁设置状态
            brick.GetComponent<SpriteRenderer>().color = wallData.brickColor;
            brick.brickcolor = wallData.brickColor;
            brick.hasPowerUp = wallData.hasPowerUp;
        }

        // 如果需要，设置玩家的生命和分数
        GameManager.instance.life = gameData.playerLives;
        GameManager.instance.MaxScore = gameData.Maxscore;
        GameManager.instance.Score = gameData.score;
        GameManager.instance.bricksDestroyed = gameData.blockisDestroyed;
        GameManager.instance.level = gameData.level;

    }




    private void NewGame()
    {
        Debug.Log("NEWGAME");

        // 获取屏幕宽高
        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize * 2;

        // 根据横竖屏模式选择砖块数量
        int numRows = (screenWidth > screenHeight) ? size.y : size.x; // 横屏时行数使用size.y，竖屏时使用size.x
        int numCols = (screenWidth > screenHeight) ? size.x : size.y; // 横屏时列数使用size.x，竖屏时使用size.y

        // 根据屏幕大小计算墙的宽高
        float wallWidthRatio = 0.8f; // 墙宽度占屏幕80%
        float wallHeightRatio = 0.30f; // 墙高度占屏幕25%（修改为原来的50%的一半）

        float wallWidth = screenWidth * wallWidthRatio;
        float wallHeight = screenHeight * wallHeightRatio;

        // 计算砖块的宽度和高度，同时考虑间距
        float brickWidth = (wallWidth - (numCols - 1) * spacing) / numCols;
        float brickHeight = (wallHeight - (numRows - 1) * spacing) / numRows;

        // 计算砖块的起始位置，使整个墙在屏幕中居中显示
        Vector3 startPosition = transform.position - new Vector3(wallWidth / 2 - brickWidth / 2, wallHeight / 2 - brickHeight / 2, 0);

        // 创建砖块
        for (int i = 0; i < numCols; i++)
        {
            for (int j = 0; j < numRows; j++)
            {
                GameObject newBrick = Instantiate(brickPrefab, transform);
                newBrick.transform.localScale = new Vector3(brickWidth, brickHeight, 1); // 调整砖块的尺寸
                newBrick.transform.position = startPosition + new Vector3(i * (brickWidth + spacing), j * (brickHeight + spacing), 0);
                newBrick.GetComponent<SpriteRenderer>().color = gradient.Evaluate((float)j / (numRows - 1));

                Debug.Log("Color" + newBrick.GetComponent<SpriteRenderer>().color);

                // 获取或添加 Brick 脚本并设置初始生命值
                Bricks brick = newBrick.GetComponent<Bricks>();
                if (brick == null)
                {
                    brick = newBrick.AddComponent<Bricks>();
                }

                if (GameManager.instance.level == 1)
                {
                    brick.health = j ;  // 使用 j + 1 确保砖块生命值从1开始
                    brick.score = j ;
                }
                else
                {
                    brick.health = j * 2;
                    brick.score = j +  2;
                }
                brick.brickcolor = gradient.Evaluate((float)j / (numRows - 1));
                brick.startPosition = startPosition;
                brick.hasPowerUp = Random.value < 0.2f; // 10% 概率
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

    // Update is called once per frame
    void Update()
    {
        //if(GameManager.instance.Score == 150) {
        //    level = 2;
        //}

        if ((size.x * size.y) == GameManager.instance.bricksDestroyed) {
            if (GameManager.instance.level == 1)
            {
                GameManager.instance.level = 2;
            }
            else {
                GameManager.instance.level = 1;
            }
            finishChangeLevel = false;
            GameManager.instance.bricksDestroyed = 0;
            //uimanager.OpenNextLevelPanel();

            if (Screen.dpi == 96)
            {
                nextPanel_PC.SetActive(true);
                nextPanel_H.SetActive(false);
                nextPanel_V.SetActive(false);
            }
            else {
                CheckScreenOrientation();
            }
                GameManager.instance.PauseGame();
           
        }



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

        if (!finishChangeLevel) {
            NewGame();
            GameManager.instance.SaveGame();
            moveBall.ResetBall();
            finishChangeLevel = true;
        }



    }
}
