using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.SocialPlatforms.Impl;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager instance;
    private SaveManager saveManager;
    public bool isNewGame = true;

    public int Score;
    public int MaxScore;
    public int life = 3;
    public int bricksDestroyed = 0;
    public int level = 1;

    public Color brickColor;
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
        string path = Path.Combine(Application.persistentDataPath, Application.dataPath + "\\Script\\SaveData\\savegame.json");
        string json = File.ReadAllText(path);
        GameData gameData = JsonUtility.FromJson<GameData>(json);
        SceneManager.LoadSceneAsync(1);
        
        life = 3;
        Score = 0;
        MaxScore = gameData.Maxscore;
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
        gameData.score = Score;
        gameData.blockisDestroyed = bricksDestroyed;
        gameData.level = level;
 
        if (Score > MaxScore)
        {
            gameData.Maxscore = Score;
        }
        else {
            gameData.Maxscore = MaxScore;
        }

       
        // 获取所有 Bricks 组件并保存它们的状态
        Bricks[] bricks = FindObjectsOfType<Bricks>();
        foreach (Bricks brick in bricks)
        {
            GameData.WallData wallData = new GameData.WallData
            {
                position = brick.transform.position,
                health = brick.health,
                blockScore = brick.score,
                brickColor = brick.brickcolor,
                isDestroyed = !brick.gameObject.activeSelf // 如果砖块被摧毁，则保存为 true

            };

            gameData.walls.Add(wallData);
        }

        // 使用 SaveManager 保存 gameData
        saveManager.SaveGame(gameData);
    }

    void Start()
    {
        saveManager = GetComponent<SaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
