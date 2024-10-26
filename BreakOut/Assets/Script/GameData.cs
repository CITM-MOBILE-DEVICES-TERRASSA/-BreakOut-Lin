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
        public Vector2 position; 
        public int health;       
        public int blockScore;       
        public Color brickColor;       
        public bool isDestroyed;
        public Vector3 startPosition;
        public bool hasPowerUp;
    }
}

