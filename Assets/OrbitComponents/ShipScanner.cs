using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScanner : MonoBehaviour
{
    [SerializeField] float scanTime = 1f;

    public void PerformScan(Scanable target)
    {
        StartCoroutine(ScanCoroutine(target));
    }

    IEnumerator ScanCoroutine(Scanable target)
    {
        yield return new WaitForSeconds(scanTime);
        
        if (target != null)
        {
            //perform scan
            target.ScanTarget(this);
        }
    }
}
