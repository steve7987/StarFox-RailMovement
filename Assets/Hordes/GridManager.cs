using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamperTypes
{
    noDamper,
    buildingDamper,
    fullDamper,
}

public class GridManager
{
    public static GridManager instance;

    bool[,] grid;
    DamperTypes[,] dampers;

    //what grid info do we need?
    //damper field?

    public GridManager(int sx, int sy)
    {
        grid = new bool[sx, sy];
        dampers = new DamperTypes[sx, sy];
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

    public void AddDampers(Vector3 pos, BuildingData data, bool inConstruction)
    {
        int sx = Mathf.RoundToInt(pos.x);
        int sy = Mathf.RoundToInt(pos.z);

        for (int i = -(int)data.damperRange - 1; i < data.damperRange + 1; i++)
        {
            for (int j = -(int)data.damperRange - 1; j < data.damperRange + 1; j++)
            {
                if (i * i + j * j <= data.damperRange * data.damperRange)
                {
                    dampers[sx + i, sy + j] = inConstruction ? DamperTypes.buildingDamper : DamperTypes.fullDamper;
                }
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

    public DamperTypes GetDamperType(int sx, int sy)
    {
        return dampers[sx, sy];
    }

}
