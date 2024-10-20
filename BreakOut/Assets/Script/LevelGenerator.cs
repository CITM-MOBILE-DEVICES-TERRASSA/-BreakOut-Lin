using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

public class LevelGenerator : MonoBehaviour
{
    public Vector2Int size; // ����ש��������x��������ש��������y��
    public GameObject brickPrefab;
    public float spacing = 0.1f; // ��������ש��֮��ļ�����
    public float wallWidthRatio = 0.8f; // ǽ�Ŀ��ռ��Ļ��ȵı�������0.8��ʾ80%��
    public float wallHeightRatio = 0.3f; // ǽ�ĸ߶�ռ��Ļ�߶ȵı�������0.3��ʾ30%��
    public Gradient gradient;
    public MoveBall moveBall;
    public Bricks bricks;
    public bool finishChangeLevel = true;
    public static LevelGenerator instance; // ����ȫ�ַ���
    public UIManager uimanager;

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

        // ��յ�ǰש�飨�����Ҫ��
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize * 2;

        // ������Ļ��С����ǽ�Ŀ��
        float wallWidth = screenWidth * wallWidthRatio;
        float wallHeight = screenHeight * wallHeightRatio;

        // ����ש��Ŀ�Ⱥ͸߶ȣ�ʹ����Ӧǽ�ĳߴ磬ͬʱ���Ǽ��
        float brickWidth = (wallWidth - (size.x - 1) * spacing) / size.x;
        float brickHeight = (wallHeight - (size.y - 1) * spacing) / size.y;

        // ����ש�����ʼλ�ã�ʹ����ǽ����Ļ�о�����ʾ
        // ����ש�����ʼλ�ã�ʹ����ǽ����Ļ�о�����ʾ
        Vector3 startPosition = transform.position - new Vector3(wallWidth / 12 - brickWidth / 2, wallHeight - brickHeight / 24, 0);


        // ���ݱ�������ݴ���ש��
        foreach (GameData.WallData wallData in gameData.walls)
        {
            GameObject newBrick = Instantiate(brickPrefab, transform);
            newBrick.transform.localScale = new Vector3(brickWidth, brickHeight, 1); // ����ש��ĳߴ�
            newBrick.transform.position = startPosition + new Vector3(wallData.position.x, wallData.position.y, 0); // ʹ�ñ������������λ��

            // ��ȡ����� Brick �ű�����������ֵ
            Bricks brick = newBrick.GetComponent<Bricks>();
            if (brick == null)
            {
                brick = newBrick.AddComponent<Bricks>();
            }
            brick.health = wallData.health;
            brick.score = wallData.blockScore;
            brick.isDestroyed = wallData.isDestroyed; // �����Ƿ񱻴ݻ�����״̬
        }

        // �����Ҫ��������ҵ������ͷ���
        GameManager.instance.life = gameData.playerLives;
        GameManager.instance.MaxScore = gameData.Maxscore;
        GameManager.instance.Score = gameData.score;
        GameManager.instance.bricksDestroyed = gameData.blockisDestroyed;
        GameManager.instance.level = gameData.level;
    }



    private void NewGame() {
        Debug.Log("NEWGAME");
        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize * 2;

        // ������Ļ��С����ǽ�Ŀ��
        float wallWidth = screenWidth * wallWidthRatio;
        float wallHeight = screenHeight * wallHeightRatio;

        // ����ש��Ŀ�Ⱥ͸߶ȣ�ʹ����Ӧǽ�ĳߴ磬ͬʱ���Ǽ��
        float brickWidth = (wallWidth - (size.x - 1) * spacing) / size.x;
        float brickHeight = (wallHeight - (size.y - 1) * spacing) / size.y;

        // ����ש�����ʼλ�ã�ʹ����ǽ����Ļ�о�����ʾ
        Vector3 startPosition = transform.position - new Vector3(wallWidth / 2 - brickWidth / 2, wallHeight / 2 - brickHeight / 2, 0);

        // ����ש��
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                GameObject newBrick = Instantiate(brickPrefab, transform);
                newBrick.transform.localScale = new Vector3(brickWidth, brickHeight, 1); // ����ש��ĳߴ�
                newBrick.transform.position = startPosition + new Vector3(i * (brickWidth + spacing), j * (brickHeight + spacing), 0);
                newBrick.GetComponent<SpriteRenderer>().color = gradient.Evaluate((float)j / (size.y - 1));

                // ��ȡ����� Brick �ű������ó�ʼ����ֵ
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
                else {
                    brick.health = j * 2;
                    brick.score = j * 2;
                }
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
            uimanager.NextLevelPanel.SetActive(true);
            GameManager.instance.PauseGame();
        }

        Debug.Log("bricksDestroyed :" + GameManager.instance.bricksDestroyed);

        if (!finishChangeLevel) {
            NewGame();
            GameManager.instance.SaveGame();
            moveBall.ResetBall();
            finishChangeLevel = true;
        }



    }
}
