using System;
using System.Collections;
using UnityEngine;

namespace Code.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed;
        public float sprintSpeed;
        public float jumpSpeed;

        [SerializeField] [Tooltip("Insert Animator Controller")]
        private Animator playerAnimator;

		//Attacking
		[SerializeField] private int attackDamage = 20;

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
        private float _rbGravity;

        private CapsuleCollider2D _capsuleCollider;
        private bool _facingRight = true;

        private float _moveDirection;
        private float _moveVertical;
        private bool _startDash;
		private bool _isSneaking;
        private bool _isJumping;
        private bool _isRunning;
        private bool _isOnLadder;
        private bool _isClimbingLadder;
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
            _rbGravity = rb.gravityScale;
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_isDashing)
            {
                return;
            }

            ProcessInputs();
            
            Move();

            Animate();
            
            // Walking and Sprinting
            var speed = _isRunning ? sprintSpeed : _isSneaking ? moveSpeed / 2 : moveSpeed;
            rb.velocity = new Vector2(_moveDirection * speed, rb.velocity.y);

            // Ladder climbing
            if (_isOnLadder && (_isClimbingLadder || _moveVertical != 0.0f) && rb.velocity.y < speed)
            {
                rb.gravityScale = 0.0f;
                rb.velocity = new Vector2(rb.velocity.x, _moveVertical * speed);
                _isClimbingLadder = true;
            }
            else
            {
                rb.gravityScale = _rbGravity;
                _isClimbingLadder = false;
            }
            
            // Fall through platforms when holding down
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"), _moveVertical < 0.0f);

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

            playerAnimator.SetBool(IsJumping, _isJumping);
            playerAnimator.SetBool(IsRunning, _isRunning);
            playerAnimator.SetBool(IsWalking, rb.velocity.x != 0.0f);
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


        }

        private void Move()
        {
            
            // Jumping
            var origin = rb.position;
            var layerMask = LayerMask.GetMask("Ground", "Platform");
            bool grounded = Physics2D.Raycast(origin, Vector2.down, _capsuleCollider.size.y/2, layerMask);
            bool inPlatform = Physics2D.Raycast(origin, Vector2.down, _capsuleCollider.size.y/2 - 0.3f, layerMask);
            //Debug.Log($"Grounded: {grounded}, InPlatform: {inPlatform}");
            
            if (Input.GetKeyDown("w") && grounded && !inPlatform)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }
        }

        private void ProcessInputs()
        {
            _moveDirection = Input.GetAxis("Horizontal");
            _moveVertical = Input.GetAxis("Vertical");
            _startDash = Input.GetKeyDown(KeyCode.LeftControl);
            _isRunning = Input.GetKey(KeyCode.LeftShift);
			_isSneaking = Input.GetKey(KeyCode.RightShift);
            _startAttack = Input.GetMouseButtonDown(0);
        }

        private void FlipCharacter()
        {
            _facingRight = !_facingRight;
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
            else if (collision.gameObject.CompareTag("Ladder"))
            {
                _isOnLadder = true;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                _isJumping = false;
            }
            else if (collision.gameObject.CompareTag("Ladder"))
            {
                _isOnLadder = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                _isJumping = true;
            }
            else if (collision.gameObject.CompareTag("Ladder"))
            {
                _isOnLadder = false;
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
            Collider2D[] hitEnemies =
                Physics2D.OverlapCircleAll(attackPosition.position, attackRange);

            // Damage detected enemies
            foreach (Collider2D enemy in hitEnemies)
            {
				enemy.GetComponent<Health>().Damage(attackDamage);
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

		public string GetState()
		{
			if (_isDashing) { return "dashing"; }
			if (_isRunning) { return "running"; }
			if (_isSneaking) { return "sneaking"; }
			return "walking";
		}
    }
}