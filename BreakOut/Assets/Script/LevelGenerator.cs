using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Vector2Int size; // 横向砖块数量（x）和纵向砖块数量（y）
    public GameObject brickPrefab;
    public float spacing = 0.1f; // 用于设置砖块之间的间距比例
    public float wallWidthRatio = 0.8f; // 墙的宽度占屏幕宽度的比例（如0.8表示80%）
    public float wallHeightRatio = 0.3f; // 墙的高度占屏幕高度的比例（如0.3表示30%）

    private void Awake()
    {
        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize * 2;

        // 根据屏幕大小计算墙的宽高
        float wallWidth = screenWidth * wallWidthRatio;
        float wallHeight = screenHeight * wallHeightRatio;

        // 计算砖块的宽度和高度，使其适应墙的尺寸，同时考虑间距
        float brickWidth = (wallWidth - (size.x - 1) * spacing) / size.x;
        float brickHeight = (wallHeight - (size.y - 1) * spacing) / size.y;

        // 计算砖块的起始位置，使整个墙在屏幕中居中显示
        Vector3 startPosition = transform.position - new Vector3(wallWidth / 2 - brickWidth / 2, wallHeight / 2 - brickHeight / 2, 0);

        // 创建砖块
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                GameObject newBrick = Instantiate(brickPrefab, transform);
                newBrick.transform.localScale = new Vector3(brickWidth, brickHeight, 1); // 调整砖块的尺寸
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
