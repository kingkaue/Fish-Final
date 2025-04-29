using System;
using UnityEngine;
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
    public GameObject CurrentPlayer;

    public event Action<int> OnLeveledUp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeGame()
    {
        enemyParent = new GameObject("Enemies");
        SpawnPlayer(new Vector3(0, 1, 0));
        // Any other initialization code
    }


    void Start()
    {
        DisplayXP();
    }

    void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log(timer);
    }

    public void SpawnPlayer(Vector3 position)
    {
        if (playerPrefab != null)
        {
            // Destroy existing player if one exists
            if (CurrentPlayer != null)
            {
                Destroy(CurrentPlayer);
            }

            CurrentPlayer = Instantiate(playerPrefab, position, Quaternion.identity);
            CurrentPlayer.name = "Player";
            CurrentPlayer.tag = "Player";
            Debug.Log($"Player spawned at: {position}");
        }
        else
        {
            Debug.LogError("Player prefab not assigned in GameManager!");
        }

    }

    void SpawnEnemy()
    {
        //GameObject enemy =  Instantiate(playerPrefab, new Vector3(Random.Range(-20f,20f), 1, Random.Range(-20f, 20f)), Quaternion.identity);
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

    public void AddXP(int amount)
    {
        xp += amount;
        DisplayXP();

        CheckLevelUp();
    }

    void LevelUp()
    {
        playerLevel++;

        nextLevelXP += (int)(nextLevelXP * levelXPMult);

        // Fire event:
        OnLeveledUp?.Invoke(playerLevel);

        CheckLevelUp();
        Debug.Log("The Player has leveled up!");
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

    //track character selection

    //save state

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
