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
        // ������Ϸ�߼������������Ϸ���������õ÷�
    }

    public void ContinueGame ()
    {
        SceneManager.LoadSceneAsync(1);
        isNewGame = false;
    }
    public void PauseGame()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0; // ��ͣ��Ϸ
        SaveGame();
    }

    public void ResumeGame()
    {
        Debug.Log("Game Resumed");
        Time.timeScale = 1; // �ָ���Ϸ
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
       
        // ��ȡ���� Bricks ������������ǵ�״̬
        Bricks[] bricks = FindObjectsOfType<Bricks>();
        foreach (Bricks brick in bricks)
        {
            GameData.WallData wallData = new GameData.WallData
            {
                position = brick.transform.position,
                health = brick.health,
                isDestroyed = !brick.gameObject.activeSelf // ���ש�鱻�ݻ٣��򱣴�Ϊ true
            };

            gameData.walls.Add(wallData);
        }

        // ʹ�� SaveManager ���� gameData
        saveManager.SaveGame(gameData);
    }

    //public void LoadGame()
    //{
    //    // ֱ��ָ�� JSON �ļ���·��
    //    string filePath = "D:\\Github\\BreakOut-Lin\\BreakOut\\Assets\\Script\\SaveData\\savegame.json";

    //    if (File.Exists(filePath))
    //    {
    //        string jsonData = File.ReadAllText(filePath);
    //        GameData gameData = JsonUtility.FromJson<GameData>(jsonData);

    //        if (gameData != null)
    //        {
    //            // ������������͵÷�
    //            life = gameData.playerLives;
    //            MaxScore = gameData.score;

    //            // ɾ�����������е�ש��
    //            Bricks[] existingBricks = FindObjectsOfType<Bricks>();
    //            foreach (Bricks brick in existingBricks)
    //            {
    //                Destroy(brick.gameObject);
    //            }

    //            // ʹ�ñ����������������ש��
    //            foreach (GameData.WallData wallData in gameData.walls)
    //            {
    //                // �����µ�ש��ʵ��
    //                GameObject newBrick = Instantiate(LevelGenerator.instance.brickPrefab, wallData.position, Quaternion.identity);
    //                Bricks brick = newBrick.GetComponent<Bricks>();

    //                if (brick != null)
    //                {
    //                    brick.health = wallData.health;
    //                    brick.UpdateHealthDisplay();
    //                    newBrick.SetActive(!wallData.isDestroyed); // ���ש��֮ǰ���ݻ٣����ڼ���ʱ����Ϊ������
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
