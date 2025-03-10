using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform worldBlockPrefab;

    [SerializeField]
    private NavMeshSurface navMeshSurface;

    [SerializeField]
    private Transform[] enemyPrefabs;

    [SerializeField]
    private EnemyDirector enemyDirector;

    public const int worldSize = 9;
    public const int blockScale = 5;
    private WorldBlock[,] worldBlocks;
    public WorldMap currentMap { get; private set; }

    private List<WorldMap> worldMapList;

    private bool isNavMeshBuilt = false;

    void Start()
    {
        worldBlocks = new WorldBlock[worldSize, worldSize];

        for(int x = 0; x < worldSize;  x++)
        {
            for (int z = 0; z < worldSize; z++)
            {
                worldBlocks[x, z] = Instantiate(worldBlockPrefab, new Vector3(x, 0, z) * blockScale + new Vector3(blockScale * 0.5f, 0, blockScale * 0.5f), Quaternion.identity, transform).GetComponent<WorldBlock>();
            }
        }

        worldMapList = new List<WorldMap>();
        worldMapList.Add(GenerateMap1());
        worldMapList.Add(GenerateMap2());
        worldMapList.Add(GenerateMap3());
        worldMapList.Add(GenerateMap4());

        LoadNextMap();
    }

    public void LoadMap(WorldMap worldMap)
    {
        currentMap = worldMap;

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
        WorldMap newMap = worldMapList[Random.Range(0, worldMapList.Count)];

        if (currentMap != null)
        {
            while (newMap == currentMap)
            {
                newMap = worldMapList[Random.Range(0, worldMapList.Count)];
            }
        }

        LoadMap(newMap);
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

    private IEnumerator WaitForMapLoad()
    {
        while(!WorldIsDormant())
        {
            yield return new WaitForSeconds(0.1f);
        }


        StartCoroutine(nameof(RebuildNavMeshAsync));
        
        while(!isNavMeshBuilt)
        {
            yield return new WaitForSeconds(0.1f);
        }

        enemyDirector.SpawnWave();
    }

    private IEnumerator RebuildNavMeshAsync()
    {
        isNavMeshBuilt = false;

        List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();

        NavMeshBuilder.CollectSources(transform, navMeshSurface.layerMask, navMeshSurface.useGeometry,
                                      navMeshSurface.defaultArea, new List<NavMeshBuildMarkup>(), sources);


        if (navMeshSurface.navMeshData == null)
        {
            //navMeshSurface.navMeshData = NavMeshBuilder.BuildNavMeshData(navMeshSurface.GetBuildSettings(), sources, surfaceBounds, navMeshSurface.center, Quaternion.identity);
            navMeshSurface.BuildNavMesh();
            navMeshSurface.navMeshData.name = "CustomNavMeshData";
            //navMeshSurface.navMeshData = new NavMeshData();
        }

        

        Bounds surfaceBounds = new Bounds(navMeshSurface.center, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));


        AsyncOperation operation = NavMeshBuilder.UpdateNavMeshDataAsync(navMeshSurface.navMeshData, navMeshSurface.GetBuildSettings(), sources, surfaceBounds);

        yield return operation;

        isNavMeshBuilt = true;
    }

    public void LoadNextMap()
    {
        LoadRandomMap();

        StartCoroutine(nameof(WaitForMapLoad));
    }

    private WorldMap GenerateMap1()
    {
        float[,] map = new float[worldSize, worldSize]
            {
            {50, 50, 50, 50, 50, 50, 50, 50, 50 },
            {50, 50, 50, 50, 60, 60, 50, 50, 50 },
            {40, 40, 20, 20, 20, 20, 20, 20, 45 },
            {30, 30, 30, 30, 30, 30, 30, 30, 40 },
            {30, 30, 30, 30, 30, 30, 30, 30, 35 },
            {30, 20, 20, 20, 40, 10, 10, 30, 30 },
            {30, 10, 10, 20, 50, 10, 10, 10, 25 },
            {30, 20, 10, 30, 60, 80, 80, 15, 20 },
            {30, 30, 30, 30, 70, 80, 80, 15, 15 },
        };

        WorldMap worldMap = new WorldMap("Elevated Side", map);

        return worldMap;
    }

    private WorldMap GenerateMap2()
    {
        float[,] map = new float[worldSize, worldSize]
        {
            { 40, 40, 10, 10, 10, 10, 10, 40, 40 },
            { 40, 30, 20, 10, 10, 10, 20, 30, 40 },
            { 10, 20, 10, 10, 20, 10, 10, 20, 10 },
            { 10, 10, 10, 20, 30, 20, 10, 10, 10 },
            { 10, 10, 20, 30, 40, 30, 20, 10, 10 },
            { 10, 10, 10, 20, 30, 20, 10, 10, 10 },
            { 10, 20, 10, 10, 20, 10, 10, 20, 10 },
            { 40, 30, 20, 10, 10, 10, 20, 30, 40 },
            { 40, 40, 10, 10, 10, 10, 10, 40, 40 },
        };

        WorldMap worldMap = new WorldMap("Central Spike", map);

        return worldMap;
    }

    private WorldMap GenerateMap3()
    {
        float[,] map = new float[worldSize, worldSize]
        {
            {40, 10, 10, 10, 10, 10, 10, 40, 10 },
            {10, 10, 10, 10, 30, 10, 10, 10, 10 },
            {10, 30, 12, 10, 30, 10, 32, 10, 10 },
            {10, 40, 14, 10, 30, 10, 30, 10, 10 },
            {10, 50, 16, 10, 10, 10, 28, 10, 10 },
            {10, 60, 18, 10, 10, 10, 26, 10, 10 },
            {10, 10, 10, 10, 10, 10, 10, 10, 10 },
            {40, 10, 18, 20, 22, 24, 26, 40, 10 },
            {40, 10, 18, 20, 22, 24, 26, 40, 10 },
        };

        WorldMap worldMap = new WorldMap("Stairs", map);

        return worldMap;
    }

    private WorldMap GenerateMap4()
    {
        float[,] map = new float[worldSize, worldSize]
            {
            {60, 60, 60, 60, 60, 60, 60, 60, 60 },
            {60, 50, 40, 30, 20, 10, 10, 10, 60 },
            {60, 10, 10, 10, 10, 10, 10, 10, 60 },
            {60, 10, 10, 10, 10, 20, 10, 10, 60 },
            {60, 70, 10, 10, 10, 10, 10, 10, 60 },
            {60, 70, 10, 20, 10, 10, 20, 10, 60 },
            {60, 10, 10, 10, 10, 10, 10, 10, 60 },
            {60, 10, 10, 10, 20, 30, 40, 50, 60 },
            {60, 60, 60, 60, 60, 60, 60, 60, 60 },
        };

        WorldMap worldMap = new WorldMap("Big Stairs", map);

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