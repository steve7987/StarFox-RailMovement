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


    HashSet<BuildingController> buildings;

    GridBuilder gridBuilder;

    //what grid info do we need?
    //damper field?

    public GridManager(int sx, int sy, GridBuilder gridBuilder)
    {
        grid = new bool[sx, sy];
        dampers = new DamperTypes[sx, sy];

        this.gridBuilder = gridBuilder;
        
        
        Debug.Assert(instance == null);



        instance = this;


        buildings = new HashSet<BuildingController>();
    }

    //some method to save the "game" to a file (or at least save buildings, so we can use them to load initial level)
    public void SaveGame()
    {
        GridSave gs = new GridSave(buildings);
        gs.SaveData("tst");
    }

    public void LoadGame(BuildingData bd)
    {
        GridSave gs = GridSave.LoadData("tst");

        var buildings = Resources.FindObjectsOfTypeAll<BuildingData>();

        for (int i = 0; i < gs.buildingNames.Count; i++)
        {
             gridBuilder.CreateBuilding(GetDataFromName(gs.buildingNames[i], buildings) , gs.buildingLocs[i], false);
        }
    }

    BuildingData GetDataFromName(string name, BuildingData[] buildings)
    {
        foreach (var b in buildings)
        {
            if (b.name == name)
            {
                return b;
            }
        }

        Debug.LogError("Unknown building: " + name);
        return null;
    }


    public void AddBuilding(Vector3 pos, BuildingController bc)
    {
        int sx = Mathf.RoundToInt(pos.x);
        int sy = Mathf.RoundToInt(pos.z);

        for (int i = 0; i < bc.data.buildingSize.x; i++)
        {
            for (int j = 0; j < bc.data.buildingSize.y; j++)
            {
                grid[sx + i, sy + j] = true;
            }
        }

        //could store more info here
        buildings.Add(bc);
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
                    if (dampers[sx + i, sy + j] != DamperTypes.fullDamper)
                    {
                        dampers[sx + i, sy + j] = inConstruction ? DamperTypes.buildingDamper : DamperTypes.fullDamper;
                    }
                }
            }
        }
    }

    void RefreshDampers()
    {
        for (int i = 0; i < dampers.GetLength(0); i++)
        {
            for (int j = 0; j < dampers.GetLength(1); j++)
            {
                dampers[i, j] = DamperTypes.noDamper;
            }
        }

        foreach (var building in buildings)
        {
            if (building.data.damperRange > 0)
            {
                AddDampers(building.transform.position, building.data, building.underConstruction);
            }
        }


        GridDrawer.instance.RefreshDamperField();
    }

    public void DestroyBuilding(BuildingController building)
    {
        Debug.Log("Destroy " + building.data.buildingName + " at: " + building.transform.position);
        Debug.Assert(buildings.Contains(building));



        buildings.Remove(building);

        int sx = Mathf.RoundToInt(building.transform.position.x);
        int sy = Mathf.RoundToInt(building.transform.position.z);

        for (int i = 0; i < building.data.buildingSize.x; i++)
        {
            for (int j = 0; j < building.data.buildingSize.y; j++)
            {
                grid[sx + i, sy + j] = false;
            }
        }

        if (building.data.damperRange > 0)
        {
            RefreshDampers();
        }
    }

    //when we remove dampers, how to know what's still covered?
    //option 1, for each square, keep a damper count, then we know how much coverage there is
    //or if something with dampers is removed, recount all dampers

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
