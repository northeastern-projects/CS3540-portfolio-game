using System;
using System.Collections;
using UnityEngine;

namespace Code.Scripts
{
    public class EnemyMovement : MonoBehaviour
    {
        public float moveSpeed;
        public float attackSpeed;
        [SerializeField] private Animator playerAnimator;

        // Damage
        public float health = 200.0f;
        public float damageTakenPerHit = 10f;
        public float damageDealtPerHit = 15f;

        // Dashing
        public float dashForce;
        private const float DashTime = 0.5f;
        private bool _canDash = true;
        private bool _isDashing;
        public float dashingCooldown;
        [SerializeField] private TrailRenderer tr;

        //Attacking
        private bool _canAttack = true;
        public float attackCooldown;
        public Transform attackPosition;
        public float attackRange;

        public Rigidbody2D rb;
        private bool _facingRight = false;

        private float _moveDirection;
        private bool _startDash;
        private bool _isMoving;
        private bool _startAttack;
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private static readonly int IsDashing = Animator.StringToHash("isDashing");
        private static readonly int IsMoving = Animator.StringToHash("isMoving");


        // Awake is called after objects are initialized. Called in a random order
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (health <= 0)
            {
                Destroy(this.gameObject);
            }

                if (_isDashing)
            {
                return;
            }
            
            Animate();

            // Walking and Attacking
            rb.velocity = _isDashing
                ? new Vector2(_moveDirection * attackSpeed, rb.velocity.y)
                : new Vector2(_moveDirection * moveSpeed, rb.velocity.y);

            if (_startDash && _canDash)
            {
                StartCoroutine(Dash());
            }

            if (_startAttack && _canAttack)
            {
                StartCoroutine(Attack());
            }
        }

        // better for handling physics
        private void FixedUpdate()
        {
            if (_isDashing)
            {
                return;
            }
        }

        private void Animate()
        {
            // Animate
            if (_moveDirection > 0 && !_facingRight)
            {
                FlipCharacter();
            }
            else if (_moveDirection < 0 && _facingRight)
            {
                FlipCharacter();
            }
            
            playerAnimator.SetBool(IsMoving, _isMoving);
        }

        private void FlipCharacter()
        {
            _facingRight = !_facingRight;
            transform.Rotate(0f, 180f, 0f);
        }

        private IEnumerator Dash()
        {
            _canDash = false;
            _isDashing = true;
            playerAnimator.SetBool(IsDashing, true);
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;
            rb.velocity = _facingRight ? new Vector2(dashForce, 0f) : new Vector2(-dashForce, 0f);
            tr.emitting = true;
            yield return new WaitForSeconds(DashTime);
            // ReSharper disable once Unity.InefficientPropertyAccess
            tr.emitting = false;
            rb.gravityScale = originalGravity;
            _isDashing = false;
            playerAnimator.SetBool(IsDashing, false);
            yield return new WaitForSeconds(dashingCooldown);
            _canDash = true;
        }

        private IEnumerator Attack()
        {
            _canAttack = false;
            // playerAnimator.SetTrigger(Attack1);
            // Detect which enemies are in range
            Collider2D[] hitEnemies =
                Physics2D.OverlapCircleAll(attackPosition.position, attackRange);
            
            // Damage detected enemies
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.gameObject.GetComponent("PlayerMovement").health -= enemy.gameObject.GetComponent("PlayerMovement").damageTakenPerHit;
                Debug.Log("Hit " + enemy.name);
            }
            
            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPosition == null)
            {
                return;
            }
            Gizmos.DrawWireSphere(attackPosition.position, attackRange);
        }
    }
}