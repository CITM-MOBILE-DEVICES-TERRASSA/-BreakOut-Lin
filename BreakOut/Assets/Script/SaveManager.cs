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
        string json = JsonUtility.ToJson(gameData, true); // Convert the GameData object to a JSON string
        File.WriteAllText(savePath, json);// Save the JSON string to a file
        Debug.Log("Game saved to " + savePath);
    }

    public GameData LoadGame()
    {
        //si exixt path
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json); // Deserialize the JSON string back into a GameData object
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
