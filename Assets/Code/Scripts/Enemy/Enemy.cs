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

        void Start()
        {
			gameData = GameObject.Find("GameData").GetComponent<GameData>();
            GetComponent<Health>().SetHealth(data.health);
            player = GameObject.FindWithTag("Player");
        }
        
        // Update is called once per frame
        void Update()
        {
			if (!gameData.started || gameData.paused || gameData.ended)
			{
				return;
			}
            Move();
            FlipEnemy();
        }

        private void FlipEnemy()
        {
			if (!player)
			{
				return;
			}
            if ((player.transform.position.x >= transform.position.x && !facingRight) || (player.transform.position.x < transform.position.x && facingRight))
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                facingRight = !facingRight;
            }
        }

        private void Move()
        {
			if (!player)
			{
				return;
			}
            if (Mathf.Abs(transform.position.x - player.transform.position.x) > data.buffer && player.GetComponent<PlayerMovement>().GetState() != "sneaking")
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y),
                    data.moveSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.CompareTag("Player") && collider.GetComponent<Health>() && canAttack)
            {
                StartCoroutine(Attack(collider));
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