using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyData : ScriptableObject
{
    public int health;
    public int damage;
    public int moveSpeed;
    public int attackSpeed;

    //distance between player and enemy when enemy is attacking
    public float buffer;
    public float attackCooldown;
}
