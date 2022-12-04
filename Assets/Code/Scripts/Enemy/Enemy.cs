using System;
using System.Collections;
using UnityEngine;

namespace Code.Scripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyData data;
		private GameData gameData;
        private bool canAttack = true;
        private bool facingRight = false;

        // The player
        private GameObject player;

        [SerializeField] private Animator animator;
        
        Rect rect = new Rect(0, 0, 300, 100);
        Vector3 offset = new Vector3(0f, 0f, 0.5f);
        private int stackedEnemies = 0;
        private bool labelDisplayed = false;

        void Start()
        {
			gameData = GameObject.Find("GameData").GetComponent<GameData>();
            GetComponent<Health>().SetHealth(data.health);
            player = GameObject.FindWithTag("Player");
        }
        
        // Update is called once per frame
        void Update()
        {
			if (!player || !gameData.started || gameData.paused || gameData.ended)
			{
				return;
			}
            if (Mathf.Abs(transform.position.y - player.transform.position.y) > 5)
            {
                Walk();
                
                if (transform.position.x > 57)
                {
                    FlipEnemy();
                    transform.position = new Vector2(57, transform.position.y);
                }

                if (transform.position.x < 3)
                {
                    FlipEnemy();
                    transform.position = new Vector2(3, transform.position.y);
                }
            }
            else
            {
                Attack();
                if ((player.transform.position.x >= transform.position.x && !facingRight) || (player.transform.position.x < transform.position.x && facingRight))
                {
                    FlipEnemy();
                }
            }
        }
        
        private void FlipEnemy()
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            facingRight = !facingRight;
        }

        private void Walk()
        {
            if (facingRight)
            {
                transform.position = new Vector2(transform.position.x + data.moveSpeed * Time.deltaTime, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(transform.position.x - data.moveSpeed * Time.deltaTime, transform.position.y);
            }
        }
        
        private void Attack()
        {
            if (Mathf.Abs(transform.position.x - player.transform.position.x) > data.buffer && player.GetComponent<PlayerMovement>().GetState() != "sneaking")
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y),
                    data.attackSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.CompareTag("Player") && collider.GetComponent<Health>() && canAttack)
            {
                StartCoroutine(Attack(collider));
            }
            else
            {
                //enemies are stacking
                stackedEnemies++;
            }
        }

        private void OnTriggerExit(Collider2D collider)
        {
            if (collider.CompareTag("Player"))
            {
                stackedEnemies--;
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
        
        void DisplayLabel(int number)
        {
            Vector3 point = Camera.main.WorldToScreenPoint(transform.position + offset);
            rect.x = point.x;
            rect.y = Screen.height - point.y - rect.height; // bottom left corner set to the 3D point
            GUI.Label(rect, number.ToString()); // display its name, or other string
        }
    }
}