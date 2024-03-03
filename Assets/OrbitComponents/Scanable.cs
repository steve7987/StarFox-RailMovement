using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanable : MonoBehaviour
{
    [SerializeField] string baseInfo;
    [SerializeField] string scanRes;

    bool hasBeenScanned = false;  //later on should allow mouse over to show scan results?

    public void ScanTarget(ShipScanner scanner)
    {
        hasBeenScanned = true;
    }

    //we may want this to be different depending on what is being scanned?
    public string GetScanKnowledge()
    {
        if (hasBeenScanned)
        {
            return scanRes;
        }
        else
        {
            return baseInfo;
        }
    }
}
