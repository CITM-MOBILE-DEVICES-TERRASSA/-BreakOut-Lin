using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager instance;


    private SaveManager saveManager;
    public HUD hud;


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
        Debug.Log("Game Started");
        // ������Ϸ�߼������������Ϸ���������õ÷�
    }

    public void ContinueGame ()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0; // ��ͣ��Ϸ
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
        GameData gameData = new GameData();
        gameData.playerLives = hud.life;
        gameData.score = hud.MaxScore;

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

    public void LoadGame()
    {
        GameData gameData = saveManager.LoadGame();
        if (gameData != null)
        {
                //hud.life = gameData.playerLives;
                //hud.MaxScore = gameData.score;

            // ɾ�����������е�ש��
            Bricks[] existingBricks = FindObjectsOfType<Bricks>();
            foreach (Bricks brick in existingBricks)
            {
                Destroy(brick.gameObject);
            }

            // ʹ�ñ����������������ש��
            foreach (GameData.WallData wallData in gameData.walls)
            {
                // �����µ�ש��ʵ��
                GameObject newBrick = Instantiate(LevelGenerator.instance.brickPrefab, wallData.position, Quaternion.identity);
                Bricks brick = newBrick.GetComponent<Bricks>();
                brick.health = wallData.health;
                brick.UpdateHealthDisplay();
                newBrick.SetActive(!wallData.isDestroyed); // ���ש��֮ǰ���ݻ٣����ڼ���ʱ����Ϊ������
            }
        }
    }


    void Start()
    {
        saveManager = GetComponent<SaveManager>();
        hud = FindObjectOfType<HUD>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
