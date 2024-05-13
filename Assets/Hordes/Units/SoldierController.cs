using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : Selectable
{
    [SerializeField] WeaponTargeter targeter;
    [SerializeField] UnityEngine.UI.Slider hpSlider;

    Animator animator;

    float currentHP = 50;

    Vector3 moveTarget;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        moveTarget = transform.position;
    }

    private void Update()
    {
        if (targeter.target != null)
        {
            transform.forward = targeter.target.transform.position - transform.position;
        }
        animator.SetBool("Attack", targeter.target != null);
    }

    public override string GetText()
    {
        return "Soldier";
    }

    public override void TakeDamage(float amount)
    {
        currentHP -= amount;
        hpSlider.gameObject.SetActive(true);
        hpSlider.value = currentHP / 50f;
        if (currentHP <= 0)
        {
            animator.SetTrigger("Death");
            hpSlider.gameObject.SetActive(false);
            Destroy(this);
            Destroy(gameObject, 5f);
        }
    }

    public override void SmartAction(Vector3 target)
    {
        moveTarget = target;
    }

    void Fire()
    {
        if (targeter.target != null)
        {
            targeter.target.TakeDamage(1);
        }
    }
}
