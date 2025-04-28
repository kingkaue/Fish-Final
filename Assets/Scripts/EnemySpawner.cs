using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyPrefab2;
    public float spawnHeightAboveGrid = 1.5f;
    public float spawnRadius = 50f;
    public float spawnDelay = 2f;
    public GameObject BossEnemy;
    private bool bossSpawned = false;
    private GenerateGrid gridGenerator;

    void Start()
    {
        gridGenerator = FindObjectOfType<GenerateGrid>();
        if (gridGenerator == null)
        {
            Debug.LogError("No GenerateGrid found in scene!");
            return;
        }

        StartCoroutine(EnemySpawningLoop()); // Start the infinite spawning loop
    }

    private IEnumerator EnemySpawningLoop()
    {
        // Wait until the grid is ready
        yield return new WaitUntil(() => gridGenerator.blockPositions.Count > 0);

        // Infinite loop to keep spawning enemies
        while (true)
        {
            // Spawn regular enemies based on player level
            if (GameManager.Instance.playerLevel == 1)
            {
                SpawnEnemy(enemyPrefab);
                if (Random.Range(0, 7) == 5) SpawnEnemy(enemyPrefab2);
            }
            else if (GameManager.Instance.playerLevel == 2)
            {
                SpawnEnemy(enemyPrefab);
                if (Random.Range(0, 5) == 5) SpawnEnemy(enemyPrefab2);
            }
            else if (GameManager.Instance.playerLevel >= 4)
            {
                SpawnEnemy(enemyPrefab);
                if (Random.Range(0, 3) == 3) SpawnEnemy(enemyPrefab2);

                // Spawn boss ONCE if not already spawned
                if (!bossSpawned)
                {
                    SpawnEnemy(BossEnemy);
                    bossSpawned = true;
                    Debug.Log("BOSS SPAWNED!");
                }
            }

            // Wait before next spawn
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnEnemy(GameObject enemyType)
    {
        if (gridGenerator.blockPositions.Count > 0)
        {
            // Spawn on grid positions
            int randomIndex = Random.Range(0, gridGenerator.blockPositions.Count);
            Vector3 spawnPos = gridGenerator.blockPositions[randomIndex];
            spawnPos.y += spawnHeightAboveGrid;
            Instantiate(enemyType, spawnPos, Quaternion.identity);
        }
        else
        {
            // Fallback: Spawn randomly in NavMesh area
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPoint.y = 100f;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 200f, NavMesh.AllAreas))
            {
                Instantiate(enemyType, hit.position + Vector3.up * spawnHeightAboveGrid, Quaternion.identity);
            }
        }
    }
}