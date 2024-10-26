using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.SocialPlatforms.Impl;


public class GameManager : MonoBehaviour
{
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
        // Singleton
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
    }


    public void StartGame()
    {
        //for save game,json
        string path = Path.Combine(Application.persistentDataPath, Application.dataPath + "\\Script\\SaveData\\savegame.json");
        string json = File.ReadAllText(path);
        GameData gameData = JsonUtility.FromJson<GameData>(json);
        SceneManager.LoadSceneAsync(1);
        //game inici
        life = 3;
        Score = 0;
        MaxScore = gameData.Maxscore;
        isNewGame = true;
        audioSource.Play();
        bricksDestroyed = 0;
    }

    public void ContinueGame ()
    {
        //Continue Game
        SceneManager.LoadSceneAsync(1);
        isNewGame = false;
        audioSource.Play();
    }
    public void PauseGame()
    {
        //PauseGame
        Time.timeScale = 0; 
        SaveGame();
        audioSource.Pause();
    }

    public void ResumeGame()
    {
        //ResumeGame
        Debug.Log("Game Resumed");
        Time.timeScale = 1; 
        audioSource.UnPause();
    }

    public void GameOver()
    {
        //Go scene GameOver
        SceneManager.LoadSceneAsync(2);
        audioSource.Stop();
    }

    public void EndGame()
    {
        //End Game, quit
        Application.Quit();
        audioSource.Pause();
    }

    public void ReturnToMainMenu()
    {
        //GameOver to Main Menu
        SceneManager.LoadSceneAsync(0);
        audioSource.Pause();
    }


    public void SaveGame()
    {
        //Save Game
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


        // Get all Bricks components and save their states.
        Bricks[] bricks = FindObjectsOfType<Bricks>();
        foreach (Bricks brick in bricks)
        {
            GameData.WallData wallData = new GameData.WallData
            {
                //Save brick data
                position = brick.transform.position,
                health = brick.health,
                blockScore = brick.score,
                brickColor = brick.brickcolor,
                isDestroyed = !brick.gameObject.activeSelf, 
                startPosition = brick.startPosition,
                hasPowerUp = brick.hasPowerUp
            };

            gameData.walls.Add(wallData);
        }

        // use SaveManager save data
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
