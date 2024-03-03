using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public void SetTarget(Damageable target)
    {
        StartCoroutine(GoToTarget(target));
    }

    IEnumerator GoToTarget(Damageable target)
    {
        while (target != null)
        {
            transform.forward = (target.transform.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 175f);
            
            if ((transform.position - target.transform.position).magnitude < 5f)
            {
                target.TakeDamage(3);
                Destroy(gameObject);
            }
            
            yield return null;
        }

        Destroy(gameObject);
    }
}
