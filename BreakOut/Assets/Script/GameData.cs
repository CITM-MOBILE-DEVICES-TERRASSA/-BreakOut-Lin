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
    public int level;
    public int blockisDestroyed;


    [System.Serializable]
    public class WallData
    {
        public Vector2 position; // ǽ������
        public int health;       // ǽ������ֵ
        public int blockScore;       // ǽ������ֵ
        public Color brickColor;       // ǽ������ֵ
        public bool isDestroyed; // ǽ�Ƿ񱻴ݻ�
        public Vector3 startPosition;
        public bool hasPowerUp;
    }
}

