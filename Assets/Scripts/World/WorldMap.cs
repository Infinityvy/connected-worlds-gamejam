using System.Collections;
using UnityEngine;

public class WorldMap
{
    private string name;
    private float[,] map;

    public WorldMap(string name, float[,] map)
    {
        this.name = name;
        this.map = map;
    }

    public string getName()
    {
        return name;
    }

    public float[,] getMap()
    {
        return map;
    }
}