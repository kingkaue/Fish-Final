using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.AI.Navigation;

public class GenerateGrid : MonoBehaviour
{
    public GameObject blockGameObject; // Your cube prefab
    public GameObject cactus;         // Your cactus prefab
    public NavMeshSurface navMeshSurface; // Assign in Inspector

    public int worldsizeX = 100;
    public int worldsizeZ = 100;
    public float gridoffset = 1.1f;
    public int noiseHeight = 5;
    public List<Vector3> blockPositions = new List<Vector3>();

    void Start()
    {
        GenerateTerrain();
        SpawnObjects();
        CombineMeshesAndBakeNavMesh();
    }

    public void GenerateTerrain()
    {
        for (int x = 0; x < worldsizeX; x++)
        {
            for (int z = 0; z < worldsizeZ; z++)
            {
                Vector3 pos = new Vector3(
                    x * gridoffset,
                    generateNoise(x, z, 8f) * noiseHeight,
                    z * gridoffset
                );

                GameObject block = Instantiate(blockGameObject, pos, Quaternion.identity, transform);
                blockPositions.Add(pos);
                block.SetActive(false); // Hide after storing position
            }
        }
    }

    public void SpawnObjects()
    {
        for (int c = 0; c < 20; c++)
        {
            Vector3 spawnPos = ObjectSpawnLocation();

            // Raycast down to find exact terrain height
            RaycastHit hit;
            float terrainHeight = spawnPos.y; // Fallback to original height if raycast fails
            if (Physics.Raycast(spawnPos + Vector3.up * 100, Vector3.down, out hit, Mathf.Infinity))
            {
                terrainHeight = hit.point.y;
            }

            Instantiate(cactus, new Vector3(spawnPos.x, terrainHeight + 3.5f, spawnPos.z), Quaternion.identity);
        }
    }

    public Vector3 ObjectSpawnLocation()
    {
        int rndIndex = Random.Range(0, blockPositions.Count);
        Vector3 newPos = blockPositions[rndIndex];
        blockPositions.RemoveAt(rndIndex);
        return newPos;
    }

    public void CombineMeshesAndBakeNavMesh()
    {
        // 1. Combine all child meshes
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(true); // Include inactive
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        // 2. Create final combined mesh
        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // Support for large meshes
        combinedMesh.CombineMeshes(combine);

        // 3. Set up the parent object
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = combinedMesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = blockGameObject.GetComponent<MeshRenderer>().sharedMaterial;

        // 4. Add collider (necessary for NavMesh)
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = combinedMesh;

        // 5. Clean up individual cubes
        foreach (Transform child in transform)
        {
            if (child != transform) Destroy(child.gameObject);
        }

        // 6. Bake NavMesh
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
        else
        {
            Debug.LogWarning("NavMeshSurface not assigned!");
        }
    }

    public float generateNoise(int x, int z, float detailScale)
    {
        float xNoise = (x + transform.position.x) / detailScale;
        float zNoise = (z + transform.position.z) / detailScale;
        return Mathf.PerlinNoise(xNoise, zNoise);
    }
}