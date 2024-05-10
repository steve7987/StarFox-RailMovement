using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDrawer : MonoBehaviour
{
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
