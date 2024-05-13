using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Units/UnitData", order = 1)]
public class UnitData : ScriptableObject
{
    [Header("Cosmetic")]
    public string unitName;

    [Header("Combat")]
    public float maxHP = 50f;
    public float damage = 6f;
    public float attackRange = 5f;
    public float moveSpeed = 2f;

}
