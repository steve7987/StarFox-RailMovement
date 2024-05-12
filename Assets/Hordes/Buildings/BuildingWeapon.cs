using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWeapon : MonoBehaviour
{
    RakeController target;
    bool findNewTarget = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Rake"))
        {
            Debug.Log(other.gameObject);
            target = other.GetComponent<RakeController>();
            findNewTarget = true;
        }
    }


    private void Update()
    {
        if (target != null)
        {
            target.TakeDamage(12 * Time.deltaTime);
            findNewTarget = true;
        }
        else if (findNewTarget)
        {
            LookForTarget();
            findNewTarget = false;
        }
    }

    void LookForTarget()
    {
        var hits = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius, 1 << LayerMask.NameToLayer("Rake"));
        foreach (var h in hits)
        {
            if (h.GetComponent<RakeController>() != null)
            {
                target = h.GetComponent<RakeController>();
            }
        }
    }
}
