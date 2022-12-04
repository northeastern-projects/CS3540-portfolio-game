using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageInterval = 1.0f;
    private Health _playerHealth;

    private void Awake()
    {
        _playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        InvokeRepeating(nameof(DamagePlayer), 0.0f, damageInterval);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        CancelInvoke();
    }

    private void DamagePlayer()
    {
        _playerHealth.Damage(damage);
    }
}
