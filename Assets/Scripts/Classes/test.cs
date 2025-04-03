using UnityEngine;
using System.IO;
[System.Serializable]
public class JsonPlayerData
{
    public bool mageUnlocked, rogueUnlocked;
}
public class test : MonoBehaviour
{
    public bool mageUnlocked, rogueUnlocked;
    private string path;
    void Start()
    {
        path = Application.persistentDataPath + "/playerdata.json";

        // Save to JSON
        JsonPlayerData player = new JsonPlayerData
        {
            mageUnlocked = mageUnlocked,
            rogueUnlocked = rogueUnlocked
        };

        string json = JsonUtility.ToJson(player);
        File.WriteAllText(path, json);

        // Load from JSON
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            JsonPlayerData loadedPlayer =
            JsonUtility.FromJson<JsonPlayerData>(jsonData);
            Debug.Log($"Mage Unlocked: {loadedPlayer.mageUnlocked}, Rogue Unlocked:{ loadedPlayer.rogueUnlocked}");
        }
    }
}

