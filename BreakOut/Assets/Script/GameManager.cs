using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.SocialPlatforms.Impl;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int Score;
    public int MaxScore;
    public int life = 3;
    public int bricksDestroyed = 0;
    public int level = 1;
    public bool hasPowerUp;
    public bool isNewGame = true;
    public Color brickColor;
    public static GameManager instance;

    private AudioSource audioSource;
    private SaveManager saveManager;
    public Vector3 startPosition;
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

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        //audioSource.Pause();
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
        audioSource.Play();
        bricksDestroyed = 0;
        // ������Ϸ�߼������������Ϸ���������õ÷�
    }

    public void ContinueGame ()
    {
        SceneManager.LoadSceneAsync(1);
        isNewGame = false;
        audioSource.Play();
    }
    public void PauseGame()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0; // ��ͣ��Ϸ
        SaveGame();
        audioSource.Pause();
    }

    public void ResumeGame()
    {
        Debug.Log("Game Resumed");
        Time.timeScale = 1; // �ָ���Ϸ
        audioSource.UnPause();
    }

    public void GameOver()
    {
        SceneManager.LoadSceneAsync(2);
        audioSource.Stop();
    }

    public void EndGame()
    {
        Application.Quit();
        audioSource.Pause();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
        audioSource.Pause();
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

       
        // ��ȡ���� Bricks ������������ǵ�״̬
        Bricks[] bricks = FindObjectsOfType<Bricks>();
        foreach (Bricks brick in bricks)
        {
            GameData.WallData wallData = new GameData.WallData
            {
                position = brick.transform.position,
                health = brick.health,
                blockScore = brick.score,
                brickColor = brick.brickcolor,
                isDestroyed = !brick.gameObject.activeSelf, // ���ש�鱻�ݻ٣��򱣴�Ϊ true
                startPosition = brick.startPosition,
                hasPowerUp = brick.hasPowerUp
            };

            gameData.walls.Add(wallData);
        }

        // ʹ�� SaveManager ���� gameData
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
