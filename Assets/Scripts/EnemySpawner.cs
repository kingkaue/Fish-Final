using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public float maxEnemies = 1000;
    private float playerLevell = 1;
    public GameObject enemyPrefab;
    public GameObject enemyPrefab2;
    public float spawnHeightAboveGrid = 1.5f;
    public float spawnRadius = 50f;
    public float spawnDelay = 2f;
    private GameManager gameManager;
    private GenerateGrid gridGenerator; // Reference to your grid generator
    public GameObject BossEnemy;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        gridGenerator = FindObjectOfType<GenerateGrid>();
        if (gridGenerator == null)
        {
            Debug.LogError("No GenerateGrid found in scene!");
            return;
        }
        
        StartCoroutine(EnemySpawnerRoutine());
    }

    private IEnumerator EnemySpawnerRoutine()
    {
        yield return new WaitUntil(() => gridGenerator.blockPositions.Count > 0); // Wait for grid generation

        if (GameManager.instance.playerLevel == 1)
        {
            for (int j = 0; j <= maxEnemies; j++)
            {
                SpawnEnemy(enemyPrefab);
                yield return new WaitForSeconds(spawnDelay);

                if (Random.Range(0, 7) == 5) // 1 in 7 chance
                {
                    SpawnEnemy(enemyPrefab2);
                    yield return new WaitForSeconds(spawnDelay);
                }
            }
        }

        if (GameManager.instance.playerLevel == 2)
        {
            for (int j = 0; j <= maxEnemies; j++)
            {
                SpawnEnemy(enemyPrefab);
                yield return new WaitForSeconds(spawnDelay);

                if (Random.Range(0, 5) == 5) // 1 in 5 chance
                {
                    SpawnEnemy(enemyPrefab2);
                    yield return new WaitForSeconds(spawnDelay - 1f); //spawn delay of 1 seconds as opposed to 2
                }
            }
        }

        if (GameManager.instance.playerLevel == 3)
        {
            for (int j = 0; j <= maxEnemies; j++)
            {
                SpawnEnemy(enemyPrefab);
                yield return new WaitForSeconds(spawnDelay);

                if (Random.Range(0, 3) == 3) // 1 in 5 chance
                {
                    SpawnEnemy(enemyPrefab2);
                    yield return new WaitForSeconds(0.5f); //spawn delay of 0.5 seconds as opposed to 2
                }
                if(Random.Range(0, 15) == 15)
                {
                    SpawnEnemy(BossEnemy);
                }
            }
        }
    }

    private void SpawnEnemy(GameObject enemyType)
    {
        // Method 1: Spawn on random grid position (from your existing blockPositions)
        if (gridGenerator.blockPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, gridGenerator.blockPositions.Count);
            Vector3 spawnPos = gridGenerator.blockPositions[randomIndex];
            spawnPos.y += spawnHeightAboveGrid;

            GameObject enemy = Instantiate(enemyType, spawnPos, Quaternion.identity);
            SetupEnemyNavMesh(enemy);
        }
        // Method 2: Spawn at random NavMesh location (alternative approach)
        else
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPoint.y = 100f; // Start above terrain

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 200f, NavMesh.AllAreas))
            {
                GameObject enemy = Instantiate(enemyType, hit.position + Vector3.up * spawnHeightAboveGrid, Quaternion.identity);
                SetupEnemyNavMesh(enemy);
            }
        }
    }

    private void SetupEnemyNavMesh(GameObject enemy)
    {
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = enemy.AddComponent<NavMeshAgent>();
            agent.baseOffset = spawnHeightAboveGrid;
        }

        // Add any additional NavMesh setup here
        agent.speed = Random.Range(3f, 6f);
        agent.angularSpeed = 120f;
    }
}