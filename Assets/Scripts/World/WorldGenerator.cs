using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform worldBlockPrefab;

    private const int worldSize = 9;
    private int blockScale = 5;
    private WorldBlock[,] worldBlocks;

    private List<WorldMap> worldMapList;

    void Start()
    {
        worldBlocks = new WorldBlock[worldSize, worldSize];

        for(int x = 0; x < worldSize;  x++)
        {
            for (int z = 0; z < worldSize; z++)
            {
                worldBlocks[x, z] = Instantiate(worldBlockPrefab, new Vector3(x, 0, z) * blockScale + new Vector3(blockScale * 0.5f, 0, blockScale * 0.5f), Quaternion.identity).GetComponent<WorldBlock>();
            }
        }

        worldMapList = new List<WorldMap>();
        worldMapList.Add(GenerateMap1());
        worldMapList.Add(GenerateMap2());
        worldMapList.Add(GenerateMap3());
        worldMapList.Add(GenerateMap4());

        LoadRandomMap();
    }

    public void LoadMap(WorldMap worldMap)
    {
        float[,] map = worldMap.getMap();

        for(int x = 0; x < worldSize; x++)
        {
            for(int z = 0; z < worldSize; z++)
            {
                worldBlocks[x, z].SetHeight(map[x, z]);
            }
        }
    }

    public void LoadRandomMap()
    {
        LoadMap(worldMapList[Random.Range(0, worldMapList.Count)]);
    }

    public bool WorldIsDormant()
    {
        for(int x = 0;x < worldSize; x++)
        {
            for (int z = 0;z < worldSize; z++)
            {
                if (!worldBlocks[x, z].IsDormant()) return false;
            }
        }

        return true;
    }

    private WorldMap GenerateMap1()
    {
        float[,] map = new float[worldSize, worldSize]
            {
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
            {40, 10, 10, 10, 30, 10, 10, 10, 40 },
            {10, 30, 10, 10, 20, 10, 10, 30, 10 },
            {15, 15, 20, 10, 20, 10, 20, 15, 15 },
            {10, 30, 10, 10, 20, 10, 10, 30, 10 },
            {40, 10, 10, 10, 20, 10, 10, 10, 40 },
            {10, 10, 10, 30, 10, 30, 10, 10, 10 },
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
        };

        WorldMap worldMap = new WorldMap("Central Platform", map);

        return worldMap;
    }

    private WorldMap GenerateMap2()
    {
        float[,] map = new float[worldSize, worldSize]
        {
            { 10, 10, 10, 10, 10, 10, 10, 10, 10 },
            { 10, 70, 70, 10, 10, 10, 70, 70, 10 },
            { 10, 70, 10, 10, 30, 10, 10, 70, 10 },
            { 10, 10, 10, 40, 50, 40, 10, 10, 10 },
            { 10, 10, 30, 50, 60, 50, 30, 10, 10 },
            { 10, 10, 10, 40, 50, 40, 10, 10, 10 },
            { 10, 70, 10, 10, 30, 10, 10, 70, 10 },
            { 10, 70, 70, 10, 10, 10, 70, 70, 10 },
            { 10, 10, 10, 10, 10, 10, 10, 10, 10 },
        };

        WorldMap worldMap = new WorldMap("Staggered", map);

        return worldMap;
    }

    private WorldMap GenerateMap3()
    {
        float[,] map = new float[worldSize, worldSize]
        {
            {40, 10, 10, 10, 10, 10, 10, 40, 10 },
            {10, 10, 10, 10, 30, 10, 10, 10, 10 },
            {10, 30, 12, 10, 10, 10, 32, 10, 10 },
            {10, 40, 14, 30, 10, 10, 30, 10, 10 },
            {10, 50, 16, 10, 10, 10, 28, 10, 10 },
            {10, 60, 18, 10, 10, 10, 26, 10, 10 },
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
            {40, 10, 18, 20, 22, 24, 26, 40, 10 },
            {40, 10, 18, 20, 22, 24, 26, 40, 10 },
        };

        WorldMap worldMap = new WorldMap("Pillars", map);

        return worldMap;
    }

    private WorldMap GenerateMap4()
    {
        float[,] map = new float[worldSize, worldSize]
            {
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
            {10, 10, 30, 10, 10, 10,310, 10, 10 },
            {10, 30, 10, 30, 10, 30, 10, 30, 10 },
            {10, 30, 10, 10, 30, 10, 10, 30, 10 },
            {10, 30, 10, 10, 10, 10, 10, 30, 10 },
            {10, 10, 30, 10, 50, 10, 30, 10, 10 },
            {40, 10, 30, 10, 10, 10, 30, 10, 40 },
            {10, 10, 10, 30, 10, 30, 10, 10, 10 },
            {10, 20, 10, 10, 30, 10, 10, 20, 10 },
        };

        WorldMap worldMap = new WorldMap("Pillars", map);

        return worldMap;
    }
}

/*
private WorldMap GenerateMap4()
    {
        float[,] map = new float[worldSize, worldSize]
            {
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
        };

        WorldMap worldMap = new WorldMap("Pillars", map);

        return worldMap;
    }
*/