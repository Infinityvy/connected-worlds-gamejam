using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    public static EnemyDirector Instance;

    [SerializeField]
    private WorldGenerator worldGenerator;

    [SerializeField]
    private Transform eyeguyPrefab;

    [SerializeField]
    private Transform zonerPrefab;

    private List<EnemyEntity> enemies;

    private int waveNumber = 1;
    private int worldSize;
    private int blockScale;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        enemies = new List<EnemyEntity>();
        worldSize = WorldGenerator.worldSize;
        blockScale = WorldGenerator.blockScale;
    }

    public void SpawnWave()
    {
        StartCoroutine(SpawnWaveCoroutine());
    }

    private IEnumerator SpawnWaveCoroutine()
    {
        int waveCredits = 8 + waveNumber * 2; 

        bool blueDim = true;

        while (waveCredits > 0)
        {
            int coinflip;
            if (waveCredits > 1) coinflip = Random.Range(1, 3);
            else coinflip = 1;

            Transform prefab = (coinflip == 1 ? eyeguyPrefab : zonerPrefab);

            waveCredits -= coinflip;

            int x = Random.Range(0, worldSize);
            int z = Random.Range(0, worldSize);

            float y = worldGenerator.currentMap.getMap()[x, z] * 0.5f;

            Vector3 pos = new Vector3((x + 0.5f) * blockScale, y, (z + 0.5f) * blockScale);

            DimensionBound dimBound = Instantiate(prefab, pos, Quaternion.identity).GetComponent<DimensionBound>();

            if (blueDim) dimBound.SetBoundDimension(Dimension.Blue);
            else dimBound.SetBoundDimension(Dimension.Red);

            blueDim = !blueDim;

            yield return new WaitForSeconds(0);
        }

        waveNumber++;
    }

    public void RegisterEnemy(EnemyEntity enemy)
    {
        enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyEntity enemy)
    {
        enemies.Remove(enemy);
        if(enemies.Count == 0) worldGenerator.LoadNextMap();
    }
}