using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Vector2Int size; // ����ש��������x��������ש��������y��
    public GameObject brickPrefab;
    public float spacing = 0.1f; // ��������ש��֮��ļ�����
    public float wallWidthRatio = 0.8f; // ǽ�Ŀ��ռ��Ļ��ȵı�������0.8��ʾ80%��
    public float wallHeightRatio = 0.3f; // ǽ�ĸ߶�ռ��Ļ�߶ȵı�������0.3��ʾ30%��

    private void Awake()
    {
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
            }
        }
    }






    void Start()
    {
        //for (int i = 0; i < size.x; i++)
        //{

        //    for (int j = 0; j < size.y; j++)
        //    {
        //        GameObject newBrick = Instantiate(brickPrefab, transform);
        //        newBrick.transform.position = transform.position + new Vector3((float)((size.x - 1)*.5-i) * offset.x, j * offset.y, 0);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
