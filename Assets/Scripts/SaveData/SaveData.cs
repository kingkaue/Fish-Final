using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{

    public string selectedClass;
    public Vector3 playerPosition;
    public float currentHealth;
    public float maxHealth;
    public int playerLevel;
    public int xp;
    public int nextLevelXP;
    public string currentScene;
}

[System.Serializable]
public class EnemyData
{
    public string enemyType;
    public Vector3 position;
    public Quaternion rotation;
}

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
    public List<EnemyData> enemyData;
}


public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private string savePath;
    private GameData currentGameData;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        savePath = Application.persistentDataPath + "/gamesave.json";
    }

    public void SaveGame()
    {
        currentGameData = new GameData();

        // Save player data
        currentGameData.playerData = new PlayerData
        {

            selectedClass = GetPlayerClass(),
            playerPosition = GetPlayerPosition(),
            currentHealth = GetPlayerHealth(),
            maxHealth = GetPlayerMaxHealth(),
            playerLevel = GameManager.instance.playerLevel,
            xp = GameManager.instance.xp,
            nextLevelXP = GameManager.instance.nextLevelXP,
            currentScene = SceneManager.GetActiveScene().name
        };

        // Save enemy data
        currentGameData.enemyData = new List<EnemyData>();
        var enemies = FindObjectsOfType<EnemyStats>(); // You'll need an Enemy component on your enemies
        foreach (var enemy in enemies)
        {
            currentGameData.enemyData.Add(new EnemyData
            {
                enemyType = enemy.gameObject.name.Replace("(Clone)", "").Trim(),
                position = enemy.transform.position,
                rotation = enemy.transform.rotation
            });
        }

        // Serialize and save to file
        string json = JsonUtility.ToJson(currentGameData, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Game saved successfully");
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);
            currentGameData = JsonUtility.FromJson<GameData>(jsonData);

            // Load the scene first
            if (currentGameData.playerData.currentScene != SceneManager.GetActiveScene().name)
            {
                SceneManager.LoadScene(currentGameData.playerData.currentScene);
                // Note: You'll need to continue loading after the scene loads (see below)
                return;
            }

            // If we're already in the correct scene, load everything
            CompleteLoading();
        }
        else
        {
            Debug.LogWarning("No save file found at " + savePath);
        }
    }

    // Call this after the scene has finished loading
    public void CompleteLoading()
    {
        if (currentGameData == null) return;

        // Load player data
        var playerManager = FindObjectOfType<PlayerManager>();
        var playerMovement = FindObjectOfType<PlayerMovement>();
        var saveData = GetComponent<SaveData>();


        // Set player class (you'll need to implement this based on your class selection system)
        SetPlayerClass(currentGameData.playerData.selectedClass);

        // Set player position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = currentGameData.playerData.playerPosition;
        }

        // Set player stats
        playerManager.currentHealth = currentGameData.playerData.currentHealth;
        playerManager.currentMaxHealth = currentGameData.playerData.maxHealth;
        GameManager.instance.playerLevel = currentGameData.playerData.playerLevel;
        GameManager.instance.xp = currentGameData.playerData.xp;
        GameManager.instance.nextLevelXP = currentGameData.playerData.nextLevelXP;

        // Clear existing enemies
        var existingEnemies = FindObjectsOfType<EnemyStats>();
        foreach (var enemy in existingEnemies)
        {
            Destroy(enemy.gameObject);
        }

        // Spawn saved enemies
        foreach (var enemyData in currentGameData.enemyData)
        {
            GameObject enemyPrefab = GetEnemyPrefab(enemyData.enemyType);
            if (enemyPrefab != null)
            {
                Instantiate(enemyPrefab, enemyData.position, enemyData.rotation);
            }
        }

        Debug.Log("Game loaded successfully");
    }

    // Helper methods
    private string GetPlayerClass()
    {
        // Implement based on how you track the player's class
        var playerManager = FindObjectOfType<PlayerManager>();
        return playerManager.className;
    }

    private void SetPlayerClass(string className)
    {
        // Implement based on your class selection system
        // This might involve destroying the current class component and adding the correct one
    }

    private Vector3 GetPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.transform.position : Vector3.zero;
    }

    private float GetPlayerHealth()
    {
        var playerManager = FindObjectOfType<PlayerManager>();
        return playerManager != null ? playerManager.currentHealth : 0;
    }

    private float GetPlayerMaxHealth()
    {
        var playerManager = FindObjectOfType<PlayerManager>();
        return playerManager != null ? playerManager.currentMaxHealth : 0;
    }

    private GameObject GetEnemyPrefab(string enemyType)
    {
        // Implement based on how you reference enemy prefabs
        // This might involve a dictionary or switch statement
        return null;
    }

    // For testing - remove in final version
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadGame();
        }
    }
}