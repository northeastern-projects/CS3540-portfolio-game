using System;
using System.Collections;
using UnityEngine;

namespace Code.Scripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyData data;

        // The player
        private GameObject player;

        void Start()
        {
            GetComponent<Health>().SetHealth(data.health);
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        // Update is called once per frame
        void Update()
        {
            Swarm();
        }

        private void Swarm()
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position,
                data.moveSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Player"))
            {
                if (collider.GetComponent<Health>())
                {
                    collider.GetComponent<Health>().Damage(data.damage);
                }
            }
        }
    }
}