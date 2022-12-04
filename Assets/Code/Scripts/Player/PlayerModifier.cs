using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Boost_Or_Curse", order = 2)]
public class PlayerModifier : ScriptableObject
{
    //all possible characteristics that can be modified by boosts/curses
    public float moveSpeed;
    public float sprintSpeed;
    public float jumpSpeed;
    public float dashForce;
    public float dashingCooldown;
    public float attackCooldown;
    public float attackRange;
    public int attackDamage;
    public float powerAttackCooldown;

    public void AddModifier(PlayerModifier modifier)
    {
        moveSpeed += modifier.moveSpeed;
        sprintSpeed += modifier.sprintSpeed;
        jumpSpeed += modifier.jumpSpeed;
        dashForce += modifier.dashForce;
        dashingCooldown += modifier.dashingCooldown;
        attackCooldown += modifier.attackCooldown;
        attackRange += modifier.attackRange;
        attackDamage += modifier.attackDamage;
        powerAttackCooldown += modifier.powerAttackCooldown;
    }
}