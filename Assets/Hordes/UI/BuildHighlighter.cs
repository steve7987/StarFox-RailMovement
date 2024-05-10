using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildHighlighter : MonoBehaviour
{
    [SerializeField] int maxSize = 5;
    [SerializeField] HighlighterCube cubePrefab;
    [SerializeField] SpriteRenderer spriteRenderer;

    HighlighterCube[,] cubes;

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

        
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        lastPos = Vector2Int.zero;
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
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
                cubes[i, j].SetBuildable(GridManager.instance.CanBuildSquare(lastPos.x + i, lastPos.y + j));
            }
        }
    }

    public void Activate(BuildingData data)
    {
        spriteRenderer.transform.localScale = data.GetSpriteScale();
        spriteRenderer.sprite = data.buildingImage;


        curSize = data.buildingSize;
        for (int i = 0; i < maxSize; i++)
        {
            for (int j = 0; j < maxSize; j++)
            {
                cubes[i, j].gameObject.SetActive(i < curSize.x && j < curSize.y);
            }
        }
    }
}
