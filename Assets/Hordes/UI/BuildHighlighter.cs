using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildHighlighter : MonoBehaviour
{
    
    [SerializeField] int maxSize = 5;
    [SerializeField] HighlighterCube cubePrefab;
    [SerializeField] SpriteRenderer spriteRenderer;

    HighlighterCube[,] cubes;

    HighlighterCube[,] damperCubes;

    Vector2Int lastPos;
    Vector2Int curSize;

    private void Awake()
    {
        cubes = new HighlighterCube[maxSize, maxSize];

        for (int i = 0; i < maxSize; i++)
        {
            for (int j = 0; j < maxSize; j++)
            {
                cubes[i, j] = Instantiate(cubePrefab, new Vector3(i, 0, j), Quaternion.identity, transform);
            }
        }

        //damper cubes needs to go around the highlighter?

        damperCubes = new HighlighterCube[2 * maxSize, 2 * maxSize];

        for (int i = -maxSize; i < maxSize; i++)
        {
            for (int j = -maxSize; j < maxSize; j++)
            {
                damperCubes[i + maxSize, j + maxSize] = Instantiate(cubePrefab, new Vector3(i, 0, j), Quaternion.identity, transform);
            }
        }

    }

    private void Start()
    {
        DeActivate();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        lastPos = Vector2Int.zero;
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
        GridDrawer.instance.ShowDamperField(false);
    }

    public void SetPosition(Vector3 mouseIntersect)
    {
        Vector3 np = GridDrawer.WorldToGridPosition(mouseIntersect);
        Vector2Int nip = new Vector2Int((int)np.x, (int)np.z);

        //change in position
        //see if we actually moved and play sound if we did
        if (nip != lastPos)
        {
            lastPos = nip;
            transform.position = np + new Vector3(0.5f, 0, 0.5f);

            //check buildable
            CheckBuildable();
        }
    }

    public void CheckBuildable()
    {
        for (int i = 0; i < curSize.x; i++)
        {
            for (int j = 0; j < curSize.y; j++)
            {

                var hl = GridManager.instance.CanBuildSquare(lastPos.x + i, lastPos.y + j) ? HighlightTypes.buildable : HighlightTypes.unbuildable;

                cubes[i, j].SetDisplayType(hl);
            }
        }
    }

    public void Activate(BuildingData data)
    {
        gameObject.SetActive(true);

        spriteRenderer.transform.localScale = data.GetSpriteScale();
        spriteRenderer.sprite = data.buildingImage;

        if (data.showDamperField)
        {
            GridDrawer.instance.ShowDamperField(true);
        }

        curSize = data.buildingSize;
        for (int i = 0; i < maxSize; i++)
        {
            for (int j = 0; j < maxSize; j++)
            {            
                cubes[i, j].gameObject.SetActive(i < curSize.x && j < curSize.y);
            }
        }

        for (int i = -maxSize; i < maxSize; i++)
        {
            for (int j = -maxSize; j < maxSize; j++)
            {
                if (data.showDamperField && i * i + j * j <= data.damperRange * data.damperRange)
                {
                    damperCubes[i + maxSize, j + maxSize].SetDisplayType(HighlightTypes.damper);
                }
                else
                {
                    damperCubes[i + maxSize, j + maxSize].SetDisplayType(HighlightTypes.off);
                }
            }
        }
    }
}
