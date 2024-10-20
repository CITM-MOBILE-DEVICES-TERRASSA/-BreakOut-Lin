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
        public Vector2 position; // ǽ������
        public int health;       // ǽ������ֵ
        public bool isDestroyed; // ǽ�Ƿ񱻴ݻ�
    }
}

