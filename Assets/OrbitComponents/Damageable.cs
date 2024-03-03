using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] int hitPoints = 10;  //later comes from data class?

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        
        if (hitPoints <= 0)
        {
            //what should happen here? Replace with debris?
            gameObject.SetActive(false);
        }
    }

    public bool IsAlive()
    {
        return hitPoints > 0;
    }

}
