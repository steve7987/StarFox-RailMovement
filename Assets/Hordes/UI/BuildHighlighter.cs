using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildHighlighter : MonoBehaviour
{
    [SerializeField] int maxSize = 5;
    [SerializeField] GameObject cubePrefab;

    GameObject[,] cubes;


    private void Awake()
    {
        cubes = new GameObject[maxSize, maxSize];

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
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 mouseIntersect)
    {

        transform.position = GridDrawer.WorldToGridPosition(mouseIntersect) + new Vector3(0.5f, 0, 0.5f);

        //see if we actually moved and play sound if we did
    }
    
    public void SetSize(Vector2Int s)
    {
        for (int i = 0; i < maxSize; i++)
        {
            for (int j = 0; j < maxSize; j++)
            {
                
                cubes[i, j].SetActive(i < s.x && j < s.y);
                
            }
        }
    }
}
