using System;
using System.Collections;
using UnityEngine;

namespace Code.Scripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyData data;
        private bool canAttack = true;

        // The player
        private GameObject player;

        void Awake()
        {
            Debug.Log("Awake");
            GetComponent<Health>().SetHealth(data.health);
            player = GameObject.FindWithTag("Player");
        }
        
        // Update is called once per frame
        void Update()
        {
            Move();
        }

        private void Move()
        {
            if (Mathf.Abs(transform.position.x - player.transform.position.x) > data.buffer)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, data.height),
                    data.moveSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.CompareTag("Player"))
            {
                if (collider.GetComponent<Health>())
                {
                    if (canAttack)
                    {
                        StartCoroutine(Attack(collider));
                    }
                }
            }
        }

        private IEnumerator Attack(Collider2D collider)
        {
            canAttack = false;
            
            //TODO: Attack animation
            collider.GetComponent<Health>().Damage(data.damage);
            yield return new WaitForSeconds(data.attackCooldown);
            
            canAttack = true;
        }
    }
}