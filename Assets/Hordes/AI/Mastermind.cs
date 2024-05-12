using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mastermind : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnRate = 12f;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            Instantiate(enemyPrefab, GetSpawnLocation(), Quaternion.identity);
        }
    }

    Vector3 GetSpawnLocation()
    {
        Vector3 ret = Vector3.zero;
        int count = 1;
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                if (GridManager.instance.GetDamperType(i, j) != DamperTypes.fullDamper && Random.value < 1f / count)
                {
                    ret = new Vector3(i, 0, j);
                }
                count += 1;
            }
        }

        return ret;
    }
}
