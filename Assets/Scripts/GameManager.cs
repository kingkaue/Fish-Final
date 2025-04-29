using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject playerPrefab, EnemyPrefab;
    GameObject enemyParent;
    public float timer;
    public int xp = 0;
    public int nextLevelXP = 100;
    public int playerLevel = 1;
    public float levelXPMult = 1.5f;
    public GameObject CurrentPlayer { get; private set; } // Make setter private

    public event Action<int> OnLeveledUp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Find the player in Start to ensure the scene is fully loaded
        FindPlayer();
        DisplayXP();
        Debug.Log($"GameManager Start - Player Level: {playerLevel}, XP: {xp}, Next Level XP: {nextLevelXP}"); // Debug log
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void InitializeGame()
    {
        enemyParent = new GameObject("Enemies");
        // Consider instantiating the player here if it's not always loaded
        // if (CurrentPlayer == null && playerPrefab != null && SceneManager.GetActiveScene().name != "Menu")
        // {
        //     CurrentPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        //     CurrentPlayer.tag = "Player"; // Ensure it has the correct tag
        // }
    }

    void FindPlayer()
    {
        if (CurrentPlayer == null)
        {
            CurrentPlayer = GameObject.FindGameObjectWithTag("Player");
            if (CurrentPlayer == null && SceneManager.GetActiveScene().name != "Menu" && playerPrefab != null)
            {
                Debug.LogWarning("Player not found in scene, attempting to instantiate.");
                CurrentPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                CurrentPlayer.tag = "Player";
            }
            else if (CurrentPlayer == null && SceneManager.GetActiveScene().name != "Menu")
            {
                Debug.LogError("Player with tag 'Player' not found in the scene!");
            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log(timer);
    }

    void SpawnEnemy()
    {
        GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        enemy.transform.parent = enemyParent.transform;
        enemy.transform.position = new Vector3(UnityEngine.Random.Range(-20f, 20f), 0.5f, UnityEngine.Random.Range(-20f, 20f));
    }

    void GenerateLevel()
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = new Vector3(5f, 0, 5f);

        GameObject cubeParent = new GameObject("Cubes");
        GameObject[] cubes;
        cubes = new GameObject[5];

        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubes[i].transform.parent = cubeParent.transform;
            cubes[i].transform.position = new Vector3(UnityEngine.Random.Range(-20f, 20f), 0.5f, UnityEngine.Random.Range(-20f, 20f));
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayer(); // Ensure player is found after each scene load
        if (scene.name == "Menu")
        {
            xp = 0;
            playerLevel = 1;
            nextLevelXP = 100;
            Debug.Log("GameManager - Resetting player level and XP for Menu.");
            RunData.Reset(); 
        }
        Debug.Log($"Scene Loaded: {scene.name} - Player Level: {playerLevel}, XP: {xp}, Next Level XP: {nextLevelXP}"); // Debug log
    }

    public void AddXP(int amount)
    {
        xp += amount;
        // RunData.FinalXP = xp;
        DisplayXP();
        CheckLevelUp();
    }

    void LevelUp()
    {
        playerLevel++;
        nextLevelXP += (int)(nextLevelXP * levelXPMult);
        OnLeveledUp?.Invoke(playerLevel);
        CheckLevelUp();
        Debug.Log("The Player has leveled up! New Level: " + playerLevel);
    }

    void CheckLevelUp()
    {
        if (xp >= nextLevelXP) LevelUp();
    }

    void DisplayXP()
    {
        Debug.Log("Current XP: " + xp);
    }

    public int GetXP()
    {
        return xp;
    }

    void AddAugment()
    {
        // TODO
    }

    public class GameState
    {
        public int savedLevel;
        public int savedXP;
        // Add other game state variables
    }

    public GameState GetGameState()
    {
        return new GameState()
        {
            savedLevel = playerLevel,
            savedXP = xp
        };
    }

    public void RestoreGameState(GameState state)
    {
        playerLevel = state.savedLevel;
        xp = state.savedXP;
        // Restore other state variables
    }
}