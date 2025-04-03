using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameObject playerPrefab, EnemyPrefab;
    GameObject enemyParent;
    float timer;
    int xp = 0;
    int nextLevelXP = 100;
    int playerLevel = 1;
    public float levelXPMult = 1.5f;


    private void Awake()
    {
        instance = this;
        
        /*
        enemyParent = new GameObject("Enemies");

        GenerateLevel();

        SpawnPlayer();

        //temp enemy spawner for proof of concept
        for (int i = 0; i < 5; i++)
            SpawnEnemy();
        */
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

    void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        player.name = "Player";
    }

    void SpawnEnemy()
    {
        //GameObject enemy =  Instantiate(playerPrefab, new Vector3(Random.Range(-20f,20f), 1, Random.Range(-20f, 20f)), Quaternion.identity);
        GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        enemy.transform.parent = enemyParent.transform;

        enemy.transform.position = new Vector3(Random.Range(-20f, 20f), 0.5f, Random.Range(-20f, 20f));
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
            cubes[i].transform.position = new Vector3(Random.Range(-20f, 20f), 0.5f, Random.Range(-20f, 20f));
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
        //handle level up actions

        //set next level up conditions
        playerLevel++;

        if (playerLevel % 5 == 0) AddAugment();

        nextLevelXP += (int)(nextLevelXP * levelXPMult);

        //check for multiple level ups
        CheckLevelUp();
    }

    void CheckLevelUp()
    {
        if (xp >= nextLevelXP) LevelUp();
    }

    void DisplayXP()
    {
        Debug.Log("Current XP: " + xp);
    }

    void AddAugment()
    {

    }

    //track character selection

    //save state


}
