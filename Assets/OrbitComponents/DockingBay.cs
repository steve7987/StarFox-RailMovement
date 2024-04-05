using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingBay : MonoBehaviour
{

    [SerializeField] int maxFighters = 10;
    [SerializeField] int maxShuttles = 4;

    [SerializeField] GameObject FighterPrefab;
    [SerializeField] GameObject ShuttlePrefab;

    int currentFighters;
    int currentShuttles;

    private void Awake()
    {
        currentFighters = maxFighters;
        currentShuttles = maxShuttles;
    }

    public void LaunchFighters(Damageable target, int numFighters)
    {
        StartCoroutine(LFCoroutine(target, numFighters));
    }

    IEnumerator LFCoroutine(Damageable target, int numFighters)
    {
        while (currentFighters > 0 && numFighters > 0 && target != null && target.IsAlive())
        {
            SubShipController fighter = Instantiate(FighterPrefab, transform.position, Quaternion.identity).GetComponent<SubShipController>();
            fighter.SetInitialTarget(target, this);
            currentFighters -= 1;
            numFighters -= 1;

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void ReturnFighter(SubShipController fighter)
    {
        currentFighters += 1;
        Destroy(fighter.gameObject);
    }

    public void LaunchShuttles(Landable target, int numShuttles)
    {
        StartCoroutine(LSCoroutine(target, numShuttles));
    }

    IEnumerator LSCoroutine(Landable target, int numShuttles)
    {
        while (currentShuttles > 0 && numShuttles > 0 && target != null)
        {
            ShuttleController shuttle = Instantiate(ShuttlePrefab, transform.position, Quaternion.identity).GetComponent<ShuttleController>();
            shuttle.SetInitialTarget(target, this);
            currentShuttles -= 1;
            numShuttles -= 1;

            yield return new WaitForSeconds(0.35f);
        }
    }

    public void ReturnShuttle(ShuttleController shuttle)
    {
        currentShuttles += 1;
        Destroy(shuttle.gameObject);
    }
}
