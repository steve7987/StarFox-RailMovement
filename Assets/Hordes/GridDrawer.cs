using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HighlightTypes
{
    off,
    buildable,
    unbuildable,
    damper,
    buildingDamper,
}

public class GridDrawer : MonoBehaviour
{
    public static GridDrawer instance;

    [SerializeField] HighlighterCube gridCubePrefab;


    HighlighterCube[,] gridDisplayCubes;

    bool damperFieldActive;


    private void Awake()
    {
        Debug.Assert(instance == null);
        instance = this;
        gridDisplayCubes = new HighlighterCube[100, 100];
        damperFieldActive = false;
    }

    public void ShowDamperField(bool show)
    {
        damperFieldActive = show;
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                if (show)
                {
                    if (GridManager.instance.GetDamperType(i, j) == DamperTypes.noDamper)
                    {
                        HighlightCell(i, j, HighlightTypes.off);
                    }
                    else if (GridManager.instance.GetDamperType(i, j) == DamperTypes.buildingDamper)
                    {
                        HighlightCell(i, j, HighlightTypes.buildingDamper);
                    }
                    else
                    {
                        HighlightCell(i, j, HighlightTypes.damper);
                    }
                    
                }
                else
                {
                    HighlightCell(i, j, HighlightTypes.off);
                }
            }
        }
    }

    public void RefreshDamperField()
    {
        if (damperFieldActive)
        {
            ShowDamperField(true);
        }
    }

    public void HighlightCell(int sx, int sy, HighlightTypes highlight)
    {
        if (gridDisplayCubes[sx, sy] == null && highlight == HighlightTypes.off)
        {
            return;
        }

        if (gridDisplayCubes[sx, sy] == null)
        {
            gridDisplayCubes[sx, sy] = Instantiate(gridCubePrefab, new Vector3(sx + 0.5f, 0, sy + 0.5f), Quaternion.identity);
        }

        gridDisplayCubes[sx, sy].SetDisplayType(highlight);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int i = 0; i < 100; i++)
        {
            Gizmos.DrawLine(new Vector3(i, 0, 0), new Vector3(i, 0, 100));
        }
        for (int i = 0; i < 100; i++)
        {
            Gizmos.DrawLine(new Vector3(0, 0, i), new Vector3(100, 0, i));
        }
    }

    public static Vector3 WorldToGridPosition(Vector3 worldPosition)
    {
        //round to nearest 0.5?
        float xpos = ((int)worldPosition.x);
        float zpos = ((int)worldPosition.z);

        return new Vector3(xpos, 0, zpos);

        //see if we actually moved and play sound if we did
    }
}
