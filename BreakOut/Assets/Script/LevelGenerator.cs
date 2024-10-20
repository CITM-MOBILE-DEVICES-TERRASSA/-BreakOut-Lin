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

    public static LevelGenerator instance; // ����ȫ�ַ���


    private void Awake()
    {

        string path = "D:\\Github\\BreakOut-Lin\\BreakOut\\Assets\\Script\\SaveData\\savegame.json";

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
            brick.isDestroyed = wallData.isDestroyed; // �����Ƿ񱻴ݻ�����״̬
        }

        // �����Ҫ��������ҵ������ͷ���
        GameManager.instance.life = gameData.playerLives;
        GameManager.instance.MaxScore = gameData.score;
    }



    private void NewGame() {
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
                brick.health = j;
                brick.score = j;

            }
        }
        instance = this;
    }


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
