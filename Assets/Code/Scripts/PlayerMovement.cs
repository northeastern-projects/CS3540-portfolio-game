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
        private bool _canPowerAttack = true;
        public float powerAttackCooldown;

        public Rigidbody2D rb;
        private float _rbGravity;

        private CapsuleCollider2D _capsuleCollider;
        private bool _facingRight = true;

        // sounds
        [SerializeField] private AudioClip[] playerSounds;
        private AudioSource _playerAs;
        
        private float _moveDirection;
        private float _moveVertical;
        private bool _startDash;
        private bool _isGrounded;
        private bool _isSneaking;
        private bool _isJumping;
        private bool _isRunning;
        private bool _isOnLadder;
        private bool _isClimbingLadder;
        private bool _startAttack;
        private bool _startPowerAttack;
        private float _timeSinceGrounded;
        private int _numJumps;
        private bool _forgivenessJump;
        private static readonly int IsJumping = Animator.StringToHash("isJumping");
        private static readonly int IsDashing = Animator.StringToHash("isDashing");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int Attack1 = Animator.StringToHash("AttackTrigger");
        private static readonly int Attack2 = Animator.StringToHash("PowerAttack");
        private static readonly int IsClimbing = Animator.StringToHash("isClimbing");
        private static int GroundPlatformMask;


        // Awake is called after objects are initialized. Called in a random order
        private void Awake()
        {
            _playerAs = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody2D>();
            _rbGravity = rb.gravityScale;
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
            GroundPlatformMask = LayerMask.GetMask("Ground", "Platform");
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
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"),
                _moveVertical < 0.0f);

            if (_startDash && _canDash)
            {
                StartCoroutine(Dash());
            }

            if (_startAttack && _canAttack && !_isClimbingLadder)
            {
                StartCoroutine(Attack());
            }
            if (_startPowerAttack && _canAttack && _canPowerAttack && !_isClimbingLadder)
            {
                StartCoroutine(PowerAttack());
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
            playerAnimator.SetBool(IsRunning, rb.velocity.x != 0.0f && _isRunning);
            playerAnimator.SetBool(IsClimbing, _isClimbingLadder && rb.velocity.y != 0f);
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
            _isGrounded = Physics2D.Raycast(origin, Vector2.down, _capsuleCollider.size.y / 2, GroundPlatformMask);
            bool inPlatform = Physics2D.Raycast(origin, Vector2.down, _capsuleCollider.size.y / 2 - 0.3f, GroundPlatformMask);
            
            //Debug.Log($"Grounded: {grounded}, InPlatform: {inPlatform}");

            if (_isGrounded)
            {
                _timeSinceGrounded = 0;
                _numJumps = 1;
                _forgivenessJump = true;
            }
            else
            {
                _timeSinceGrounded = _timeSinceGrounded + Time.deltaTime;
            }

            if (Input.GetKeyDown("w") && _isGrounded && !inPlatform) 
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                _forgivenessJump = false;
            }
            else if (Input.GetKeyDown("w") && _timeSinceGrounded < 0.2f && !inPlatform && _forgivenessJump && _numJumps > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                _forgivenessJump = false;
            }
            else if (Input.GetKeyDown("w") && _timeSinceGrounded < 0.2f && !inPlatform && _numJumps > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                _numJumps = _numJumps - 1;
            }
            else if (Input.GetKeyDown("w") && _timeSinceGrounded >= 0.2f && _numJumps > 0)
            {
                _forgivenessJump = false;
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                _numJumps = _numJumps - 1;
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
            _startPowerAttack = Input.GetMouseButtonDown(1);
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
                Physics2D.OverlapCircleAll(attackPosition.position, attackRange, LayerMask.GetMask("Enemy"));

            // Damage detected enemies
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Health>().Damage(attackDamage);
                Debug.Log("Hit " + enemy.name);
            }

            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
        }
        
        private IEnumerator PowerAttack()
        {
            _canAttack = false;
            _canPowerAttack = false;
            playerAnimator.SetTrigger(Attack2);
            // Detect which enemies are in range
            Collider2D[] hitEnemies =
                Physics2D.OverlapCircleAll(attackPosition.position, attackRange, LayerMask.GetMask("Enemy"));

            // Damage detected enemies
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Health>().Damage(attackDamage * 2);
                Debug.Log("Hit " + enemy.name);
            }

            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
            
            yield return new WaitForSeconds(powerAttackCooldown);
            _canPowerAttack = true;

        }

        private void OnDrawGizmosSelected()
        {
            if (attackPosition == null)
            {
                return;
            }

            Gizmos.DrawWireSphere(attackPosition.position, attackRange);
        }

        public void footStep()
        {
            if (!_isGrounded)
                return;
            
            _playerAs.clip = playerSounds[0];
            _playerAs.Play();
        }
        
        public void attackSound()
        {
            _playerAs.clip = playerSounds[1];
            _playerAs.Play();
        }
        
        public void ladderClimb()
        {
            _playerAs.clip = playerSounds[2];
            _playerAs.Play();
        }
        
        public string GetState()
        {
            if (_isDashing)
            {
                return "dashing";
            }

            if (_isRunning)
            {
                return "running";
            }

            if (_isSneaking)
            {
                return "sneaking";
            }

            return "walking";
        }
    }
}