using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsSystem : MonoBehaviour
{
    //many weapons with diff stats eventually
    //also pass damage to proj?
    [SerializeField] int damage = 3;
    [SerializeField] float rearmTime = 3f;

    [SerializeField] GameObject projectilePrefab;

    Damageable currentTarget;

    Coroutine fireCO;

    public void OpenFire(Damageable target)
    {
        currentTarget = target;
        if (fireCO == null)
        {
            fireCO = StartCoroutine(FireCoroutine());
        }
    }

    IEnumerator FireCoroutine()
    {
        while (currentTarget != null && currentTarget.IsAlive())
        {
            HomingProjectile proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<HomingProjectile>();
            proj.SetTarget(currentTarget);
            //currentTarget.TakeDamage(damage);
            yield return new WaitForSeconds(rearmTime);
        }
        fireCO = null;
    }

    public void CeaseFire()
    {
        currentTarget = null;
    }
}
