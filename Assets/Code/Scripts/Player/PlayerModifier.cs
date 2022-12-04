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
    public float powerAttackCooldown;
    
}