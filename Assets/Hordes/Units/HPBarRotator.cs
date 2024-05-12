using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarRotator : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 45, 0);
    }
}
