using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/Projectile", order = 3)]
public class ProjectileData : ScriptableObject
{
    public int speed;
    public int damage;
    public int cooldown;
}
