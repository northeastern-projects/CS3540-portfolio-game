using System.Collections;
using UnityEngine;

namespace Code.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed;
        public float sprintSpeed;
        public float jumpForce;

        [SerializeField] [Tooltip("Insert Animator Controller")]
        private Animator playerAnimator;

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
        private bool _facingRight = true;

        private float _moveDirection;
        private float _moveVertical;
        private bool _startDash;
        private bool _isJumping;
        private bool _isRunning;
        private bool _startAttack;
        private static readonly int IsJumping = Animator.StringToHash("isJumping");
        private static readonly int IsDashing = Animator.StringToHash("isDashing");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int Attack1 = Animator.StringToHash("AttackTrigger");


        // Awake is called after objects are initialized. Called in a random order
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_isDashing)
            {
                return;
            }

            ProcessInputs();

            Animate();

            // Walking and Sprinting
            rb.velocity = _isRunning
                ? new Vector2(_moveDirection * sprintSpeed, rb.velocity.y)
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

            Move();
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

            playerAnimator.SetBool(IsJumping, _isJumping);
            playerAnimator.SetBool(IsRunning, _isRunning);
            playerAnimator.SetBool(IsWalking, rb.velocity.magnitude > 0);
        }

        private void Move()
        {
            // Jumping
            if (!_isJumping && _moveVertical > 0.1f)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
        }

        private void ProcessInputs()
        {
            _moveDirection = Input.GetAxis("Horizontal");
            _moveVertical = Input.GetAxis("Vertical");
            _startDash = Input.GetKeyDown(KeyCode.LeftControl);
            _isRunning = Input.GetKey(KeyCode.LeftShift);
            _startAttack = Input.GetMouseButtonDown(0);
        }

        private void FlipCharacter()
        {
            _facingRight = !_facingRight;
            //transform.Rotate(0f, 180f, 0f);
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                _isJumping = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                _isJumping = true;
            }
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
            playerAnimator.SetTrigger(Attack1);
            // Detect which enemies are in range
            var hitEnemies =
                Physics2D.OverlapCircleAll(attackPosition.position, attackRange, LayerMask.GetMask("Enemy"));
            
            // Damage detected enemies
            foreach (var enemy in hitEnemies)
            {

                // replace this log with damage once it is added. The output is just to make sure it is working properly

				//enemy.health -= enemy.damageTakenPerHit;

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