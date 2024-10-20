using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<WallData> walls = new List<WallData>();
    public int playerLives;
    public int score;
    public int Maxscore;

    [System.Serializable]
    public class WallData
    {
        public Vector2 position; // 墙的坐标
        public int health;       // 墙的生命值
        public bool isDestroyed; // 墙是否被摧毁
    }
}

