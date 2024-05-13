using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitCommand
{
    Stop,
    AttackMove,
    Move,
    //Patrol?
}

public class SoldierController : Selectable
{
    [SerializeField] WeaponTargeter targeter;
    [SerializeField] Canvas canvas;
    [SerializeField] UnityEngine.UI.Slider hpSlider;

    [SerializeField] UnitData data;

    Animator animator;

    float currentHP;

    Vector3 moveTarget;

    UnitCommand currentAction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        moveTarget = transform.position;  //ie don't move anywhere
        currentAction = UnitCommand.Stop;
        targeter.SetSize(data.attackRange);  //need to account for scale?
        currentHP = data.maxHP;
    }

    private void Update()
    {
        if (currentAction == UnitCommand.Move)
        {
            SimpleMove();
        }
        else if (currentAction == UnitCommand.AttackMove)
        {
            if (targeter.target == null)
            {
                SimpleMove();
            }
            else
            {
                SimpleAttack();
            }
        }
        else
        {
            if (targeter.target != null)
            {
                SimpleAttack();
            }
            else
            {
                animator.SetBool("Attack", false);
            }
        }
    }

    private void SimpleAttack()
    {
        transform.forward = targeter.target.transform.position - transform.position;
        animator.SetBool("Attack", true);
        animator.SetFloat("Speed", 0);
    }

    private void SimpleMove()
    {
        Vector3 dir = moveTarget - transform.position;
        animator.SetBool("Attack", false);
        if (dir.sqrMagnitude < 1)
        {
            animator.SetFloat("Speed", 0);
            currentAction = UnitCommand.Stop;
        }
        else
        {
            transform.forward = dir;
            transform.position += dir.normalized * data.moveSpeed * Time.deltaTime;
            animator.SetFloat("Speed", 5);
        }
    }

    public override string GetText()
    {
        return data.unitName;
    }

    public override void TakeDamage(float amount)
    {
        currentHP -= amount;
        hpSlider.gameObject.SetActive(true);
        hpSlider.value = currentHP / data.maxHP;
        canvas.gameObject.SetActive(true);
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
        currentAction = UnitCommand.AttackMove;
    }

    void Fire()
    {
        if (targeter.target != null)
        {
            targeter.target.TakeDamage(data.damage);
        }
    }
}
