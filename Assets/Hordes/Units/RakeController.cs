using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RakeController : Selectable
{
    [SerializeField] Vector3 target;
    
    [SerializeField] UnityEngine.UI.Slider hpSlider;

    [SerializeField] UnitData data;

    Animator animator;

    Selectable attackTarget;

    float currentHP;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHP = data.maxHP;
        GetComponent<SphereCollider>().radius = data.attackRange / transform.localScale.x;
    }

    private void Update()
    {
        //move towards target
        //if somethings in the way, attack it?
        if (attackTarget != null)
        {
            animator.SetBool("Attack", true);
            animator.SetFloat("Speed", 0);
            transform.forward = attackTarget.transform.position - transform.position;
        }
        else
        {
            animator.SetBool("Attack", false);
            //move towards target
            Vector3 dir = target - transform.position;
            if (dir.sqrMagnitude < 1)
            {
                animator.SetFloat("Speed", 0);
            }
            else
            {
                transform.forward = dir;
                transform.position += dir.normalized * data.moveSpeed * Time.deltaTime;
                animator.SetFloat("Speed", 5);
            }
            
        }

    }

    public override void TakeDamage(float amount)
    {
        currentHP -= amount;
        hpSlider.gameObject.SetActive(true);
        hpSlider.value = currentHP / data.maxHP;
        if (currentHP <= 0)
        {
            animator.SetTrigger("Death");
            hpSlider.gameObject.SetActive(false);
            Destroy(this);
            Destroy(gameObject, 5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            attackTarget = other.GetComponent<Selectable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Selectable>() == attackTarget)
        {
            attackTarget = null;
        }
    }

    //triggered in animation
    void Claw()
    {
        if (attackTarget != null)
        {
            attackTarget.TakeDamage(10);
        }
    }

    public override string GetText()
    {
        return data.unitName;
    }
}
