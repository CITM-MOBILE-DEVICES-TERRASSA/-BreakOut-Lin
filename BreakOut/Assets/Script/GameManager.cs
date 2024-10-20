using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager instance;
    private SaveManager saveManager;
    public bool isNewGame = true;

    public int Score;
    public int MaxScore;
    public int life = 3;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
        life = 3;
        Score = 0;
        isNewGame = true;
        Debug.Log("Game Started");
        // 启动游戏逻辑，比如加载游戏场景或重置得分
    }

    public void ContinueGame ()
    {
        SceneManager.LoadSceneAsync(1);
        isNewGame = false;
    }
    public void PauseGame()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0; // 暂停游戏
        SaveGame();
    }

    public void ResumeGame()
    {
        Debug.Log("Game Resumed");
        Time.timeScale = 1; // 恢复游戏
    }

    public void GameOver()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }


    public void SaveGame()
    {
        Debug.Log("Life:" + life);
        GameData gameData = new GameData();
        gameData.playerLives = life;
        gameData.score = MaxScore;
       
        // 获取所有 Bricks 组件并保存它们的状态
        Bricks[] bricks = FindObjectsOfType<Bricks>();
        foreach (Bricks brick in bricks)
        {
            GameData.WallData wallData = new GameData.WallData
            {
                position = brick.transform.position,
                health = brick.health,
                isDestroyed = !brick.gameObject.activeSelf // 如果砖块被摧毁，则保存为 true
            };

            gameData.walls.Add(wallData);
        }

        // 使用 SaveManager 保存 gameData
        saveManager.SaveGame(gameData);
    }

    //public void LoadGame()
    //{
    //    // 直接指定 JSON 文件的路径
    //    string filePath = "D:\\Github\\BreakOut-Lin\\BreakOut\\Assets\\Script\\SaveData\\savegame.json";

    //    if (File.Exists(filePath))
    //    {
    //        string jsonData = File.ReadAllText(filePath);
    //        GameData gameData = JsonUtility.FromJson<GameData>(jsonData);

    //        if (gameData != null)
    //        {
    //            // 更新玩家生命和得分
    //            life = gameData.playerLives;
    //            MaxScore = gameData.score;

    //            // 删除场景中现有的砖块
    //            Bricks[] existingBricks = FindObjectsOfType<Bricks>();
    //            foreach (Bricks brick in existingBricks)
    //            {
    //                Destroy(brick.gameObject);
    //            }

    //            // 使用保存的数据重新生成砖块
    //            foreach (GameData.WallData wallData in gameData.walls)
    //            {
    //                // 创建新的砖块实例
    //                GameObject newBrick = Instantiate(LevelGenerator.instance.brickPrefab, wallData.position, Quaternion.identity);
    //                Bricks brick = newBrick.GetComponent<Bricks>();

    //                if (brick != null)
    //                {
    //                    brick.health = wallData.health;
    //                    brick.UpdateHealthDisplay();
    //                    newBrick.SetActive(!wallData.isDestroyed); // 如果砖块之前被摧毁，则在加载时设置为不激活
    //                }
    //                else
    //                {
    //                    Debug.LogError("Bricks component not found on the instantiated object.");
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogError("Failed to parse game data from JSON.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("Game data JSON file not found: " + filePath);
    //    }
    //}


    void Start()
    {
        saveManager = GetComponent<SaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
