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
        string json = JsonUtility.ToJson(gameData, true); // ��GameData����ת��ΪJSON�ַ���
        File.WriteAllText(savePath, json); // ��JSON�ַ������浽�ļ���
        Debug.Log("Game saved to " + savePath);
    }

    public GameData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json); // ��JSON�ַ��������л�ΪGameData����
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
