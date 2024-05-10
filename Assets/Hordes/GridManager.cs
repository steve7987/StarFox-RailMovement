using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    public static GridManager instance;

    bool[,] grid;

    public GridManager(int sx, int sy)
    {
        grid = new bool[sx, sy];
        Debug.Assert(instance == null);

        instance = this;
    }

    public void AddBuilding(Vector3 pos, BuildingData data)
    {
        int sx = Mathf.RoundToInt(pos.x);
        int sy = Mathf.RoundToInt(pos.z);

        for (int i = 0; i < data.buildingSize.x; i++)
        {
            for (int j = 0; j < data.buildingSize.y; j++)
            {
                grid[sx + i, sy + j] = true;
            }
        }
    }

    public bool CanAddBuilding(Vector3 pos, BuildingData data)
    {
        int sx = Mathf.RoundToInt(pos.x);
        int sy = Mathf.RoundToInt(pos.z);

        for (int i = 0; i < data.buildingSize.x; i++)
        {
            for (int j = 0; j < data.buildingSize.y; j++)
            {
                if (!CanBuildSquare(sx + i, sy + j))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool CanBuildSquare(int sx, int sy)
    {
        return !grid[sx, sy];
    }



}
