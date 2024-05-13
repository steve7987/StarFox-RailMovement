using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTargeter : MonoBehaviour
{
    
    public Selectable target { get; private set; }
    bool findNewTarget = false;

    public void SetSize(float radius)
    {
        GetComponent<SphereCollider>().radius = radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target == null && other.gameObject.layer == LayerMask.NameToLayer("Rake"))
        {
            Debug.Log(other.gameObject);
            target = other.GetComponent<Selectable>();
            findNewTarget = true;
        }
    }

    private void Update()
    {
        if (target != null)
        {
            //target.TakeDamage(12 * Time.deltaTime);
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
            if (h.GetComponent<Selectable>() != null)
            {
                target = h.GetComponent<Selectable>();
            }
        }
    }
}
