using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public bool mageUnlocked, rogueUnlocked;
}
public class SaveData : MonoBehaviour
{
    public bool mageUnlocked, rogueUnlocked;
    private string path;
    void Start()
    {
        path = Application.persistentDataPath + "/playerdata.json";
        //Debug.Log(path);

        Loading();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            Saving();
    }

    public void Saving()
    {
        PlayerData player = new PlayerData
        {
            mageUnlocked = mageUnlocked,
            rogueUnlocked = rogueUnlocked
        };

        string json = JsonUtility.ToJson(player);
        File.WriteAllText(path, json);
        Debug.Log("Data saved Successfully");
    }

    public void Loading()
    {
        if (File.Exists(path))
        {
            //Load JSON
            string jsonData = File.ReadAllText(path);
            PlayerData loadedPlayer = JsonUtility.FromJson<PlayerData>(jsonData);
            Debug.Log($"Mage Unlocked: {loadedPlayer.mageUnlocked}, Rogue Unlocked:{loadedPlayer.rogueUnlocked}");

            //update vars to reflect load
            mageUnlocked = loadedPlayer.mageUnlocked;
            rogueUnlocked = loadedPlayer.rogueUnlocked;
        }
    }
}

