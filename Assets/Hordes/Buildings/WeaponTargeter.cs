using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTargeter : MonoBehaviour
{
    
    public Selectable target { get; private set; }
    bool findNewTarget = false;

    float radius;

    public void SetSize(float radius)
    {
        this.radius = radius;
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
            //target out of range
            if ((target.transform.position - transform.position).sqrMagnitude > radius * radius)
            {
                target = null;
                LookForTarget();
            }
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
        var hits = Physics.OverlapSphere(transform.position, radius, 1 << LayerMask.NameToLayer("Rake"));
        foreach (var h in hits)
        {
            if (h.GetComponent<Selectable>() != null)
            {
                target = h.GetComponent<Selectable>();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target.transform.position + Vector3.up, 0.25f);
            Gizmos.DrawLine(transform.position + Vector3.up, target.transform.position + Vector3.up);
        }
        

    }
}
