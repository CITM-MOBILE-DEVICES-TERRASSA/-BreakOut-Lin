using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string savePath;

    void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, Application.dataPath+"\\Script\\SaveData\\savegame.json");
    }

    public void SaveGame(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData, true); // 将GameData对象转换为JSON字符串
        File.WriteAllText(savePath, json); // 将JSON字符串保存到文件中
        Debug.Log("Game saved to " + savePath);
    }

    public GameData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json); // 将JSON字符串反序列化为GameData对象
            Debug.Log("Game loaded from " + savePath);
            return gameData;
        }
        else
        {
            Debug.LogWarning("Save file not found");
            return null;
        }
    }
}
