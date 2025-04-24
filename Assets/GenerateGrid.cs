using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Search;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public GameObject blockGameObject;

    public GameObject cactus;

    private int worldsizeX = 40;

    private int worldsizeZ = 40;

    private float gridoffset = 1.1f;

    private int noiseHeight = 5;

    private List<Vector3> blockPositions = new List<Vector3>();




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int x = 0; x < worldsizeX; x++)
        {
            for (int z = 0; z < worldsizeZ; z++)
            {
                Vector3 pos = new Vector3(x * gridoffset, generateNoise(x, z, 8f) * noiseHeight, z * gridoffset);

                GameObject block = Instantiate(blockGameObject, pos, Quaternion.identity) as GameObject;

                blockPositions.Add(block.transform.position);

                block.transform.SetParent(this.transform);
            }
        }
        SpawnObject();
    }

    private void SpawnObject()
    {
        for (int c = 0; c < 20; c++)
        {
            GameObject toPlaceObject = Instantiate(cactus, ObjectSpawnLocation(), Quaternion.identity);
        }
    }

    private Vector3 ObjectSpawnLocation()
    {
        int rndIndex = Random.Range(0, blockPositions.Count);

        Vector3 newPos = new Vector3(blockPositions[rndIndex].x, blockPositions[rndIndex].y + 1.5f, blockPositions[rndIndex].z); //0.5 gets it above ground

        blockPositions.RemoveAt(rndIndex);
        return newPos;
    }



    // Update is called once per frame
    void Update()
    {

    }

    private float generateNoise(int x, int z, float detailScale)
    {
        float xNoise = (x + this.transform.position.x) / detailScale;
        float zNoise = (z + this.transform.position.y) / detailScale;
        return Mathf.PerlinNoise(xNoise, zNoise);
    }
}
