using UnityEngine;
using System.IO;
using System.Collections.Generic;
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
    public float currentHealth;
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

    [Header("Enemy Prefabs")]
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    private string savePath;
    private GameData currentGameData;
    private bool isLoading = false;
    private bool loadingComplete = false; // Flag to track if loading is done

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("savesystem Awake - Initial instance created and marked DontDestroyOnLoad.");

            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            savePath = Path.Combine(Application.persistentDataPath, "gamesave.json");
        }
        else
        {
            Debug.Log("savesystem Awake - Duplicate instance detected, destroying self.");

            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isLoading && currentGameData != null && scene.name == currentGameData.playerData.currentScene)
        {
            // Wait one frame to ensure all objects in the scene are initialized
            StartCoroutine(DelayedCompleteLoading());
            isLoading = false;
        }
        else if (!isLoading && loadingComplete)
        {
            // This might be needed if you load a new scene without loading a save
            loadingComplete = false;
        }
    }

    private System.Collections.IEnumerator DelayedCompleteLoading()
    {
        yield return null; // Wait one frame
        CompleteLoading();
    }

    public void SaveGame()
    {
        if (GameManager.Instance == null || GameManager.Instance.CurrentPlayer == null)
        {
            Debug.LogWarning("Cannot save - game not properly initialized");
            return;
        }

        currentGameData = new GameData
        {
            playerData = new PlayerData
            {
                selectedClass = GetPlayerClass(),
                playerPosition = GetPlayerPosition(),
                currentHealth = GetPlayerHealth(),
                maxHealth = GetPlayerMaxHealth(),
                playerLevel = GameManager.Instance.playerLevel,
                xp = GameManager.Instance.xp,
                nextLevelXP = GameManager.Instance.nextLevelXP,
                currentScene = SceneManager.GetActiveScene().name
            },
            enemyData = new List<EnemyData>()
        };

        SaveEnemies();
        SaveToFile();
    }

    private void SaveEnemies()
    {
        foreach (var enemy in FindObjectsOfType<EnemyStats>())
        {
            currentGameData.enemyData.Add(new EnemyData
            {
                enemyType = enemy.EnemyType,
                position = enemy.transform.position,
                rotation = enemy.transform.rotation,
                currentHealth = enemy.CurrentHealth
            });
        }
    }

    private void SaveToFile()
    {
        try
        {
            string json = JsonUtility.ToJson(currentGameData, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"Game saved successfully to {savePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found");
            return;
        }

        try
        {
            string jsonData = File.ReadAllText(savePath);
            currentGameData = JsonUtility.FromJson<GameData>(jsonData);

            if (currentGameData.playerData.currentScene != SceneManager.GetActiveScene().name)
            {
                isLoading = true;
                SceneManager.LoadScene(currentGameData.playerData.currentScene);
            }
            else
            {
                StartCoroutine(DelayedCompleteLoading());
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Load failed: {e.Message}");
        }
    }

    private void CompleteLoading()
    {
        if (currentGameData == null) return;

        // Load player
        LoadPlayerData();

        // Load enemies
        ClearExistingEnemies();
        SpawnSavedEnemies();

        loadingComplete = true;
        Debug.Log("Game loaded successfully");
    }

    private void LoadPlayerData()
    {
        var player = GameManager.Instance.CurrentPlayer;
        if (player == null)
        {
            Debug.LogError("Player not found in the scene during LoadPlayerData!");
            return;
        }

        Vector3 loadedPosition = currentGameData.playerData.playerPosition;
        loadedPosition.y += 1f;
        player.transform.position = loadedPosition;

        var playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.currentHealth = currentGameData.playerData.currentHealth;
            playerManager.currentMaxHealth = currentGameData.playerData.maxHealth;
            SetPlayerClass(currentGameData.playerData.selectedClass);
            playerManager.isLoaded = true;
            Debug.Log($"Loaded Player Health: {playerManager.currentHealth}");
            Debug.Log($"Loaded Player Position: {player.transform.position}");
        }
        else
        {
            Debug.LogError("PlayerManager component not found on the player!");
        }

        // **Crucially, set the GameManager's level and XP here**
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerLevel = currentGameData.playerData.playerLevel;
            GameManager.Instance.xp = currentGameData.playerData.xp;
            GameManager.Instance.nextLevelXP = currentGameData.playerData.nextLevelXP;
            Debug.Log($"Loaded Player Level: {GameManager.Instance.playerLevel}, XP: {GameManager.Instance.xp}, Next Level XP: {GameManager.Instance.nextLevelXP}"); // Debug log
        }
        else
        {
            Debug.LogError("GameManager Instance is null during LoadPlayerData!");
        }
    }

    private void ClearExistingEnemies()
    {
        foreach (var enemy in FindObjectsOfType<EnemyStats>())
        {
            Destroy(enemy.gameObject);
        }
    }

    private void SpawnSavedEnemies()
    {
        foreach (var enemyData in currentGameData.enemyData)
        {
            var prefab = GetEnemyPrefab(enemyData.enemyType);
            if (prefab == null) continue;

            var enemy = Instantiate(prefab, enemyData.position, enemyData.rotation);
            if (enemy.TryGetComponent<EnemyStats>(out var stats))
            {
                stats.CurrentHealth = enemyData.currentHealth;
            }
        }
    }

    private GameObject GetEnemyPrefab(string enemyType)
    {
        return enemyPrefabs.Find(x => x.name == enemyType);
    }

    private string GetPlayerClass()
    {
        var player = GameManager.Instance.CurrentPlayer;
        return player != null ? player.GetComponent<PlayerManager>()?.className : "Unknown";
    }

    private Vector3 GetPlayerPosition()
    {
        var player = GameManager.Instance.CurrentPlayer;
        return player != null ? player.transform.position : Vector3.zero;
    }

    private float GetPlayerHealth()
    {
        var player = GameManager.Instance.CurrentPlayer;
        return player != null ? player.GetComponent<PlayerManager>()?.currentHealth ?? 0 : 0;
    }

    private float GetPlayerMaxHealth()
    {
        var player = GameManager.Instance.CurrentPlayer;
        return player != null ? player.GetComponent<PlayerManager>()?.currentMaxHealth ?? 0 : 0;
    }

    private void SetPlayerClass(string className)
    {
        var player = GameManager.Instance.CurrentPlayer;
        if (player == null) return;

        // Remove existing class components
        var classComponents = player.GetComponents<CharacterClass>();
        foreach (var component in classComponents)
        {
            Destroy(component);
        }

        // Add and initialize new class
        switch (className)
        {
            case "Mage":
                player.AddComponent<CharacterClass_Mage>();
                break;
            case "Rogue":
                player.AddComponent<CharacterClass_Rogue>();
                break;
            case "Fighter":
                player.AddComponent<CharacterClass_FIghter>();
                break;
        }

        // Update player manager
        var playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.className = className;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log($"GameManager Instance: {GameManager.Instance}");
            Debug.Log($"GameManager CurrentPlayer: {GameManager.Instance?.CurrentPlayer}");
            SaveSystem.Instance.SaveGame();
            SaveGame();
            Debug.Log("Saved Game");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            LoadGame();
            Debug.Log("Loaded Game");
        }
    }
}