using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    
    // Dashing
    public float dashForce;
    private float _dashTime = 0.1f;
    private bool _canDash = true;
    private bool _isDashing;
    public float _dashingCooldown;
    [SerializeField] private TrailRenderer tr;

    public Rigidbody2D rb;
    private bool _facingRight = true;

    private float _moveDirection;
    private float _moveVertical;
    private bool _startDash;
    private bool _isJumping;

    
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
        
        if (_startDash && _canDash)
        {
            StartCoroutine(Dash());
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
    }

    private void Move()
    {
        // Walking
        rb.velocity = new Vector2(_moveDirection * moveSpeed, rb.velocity.y);
        
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
        
    }
    private void FlipCharacter()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
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
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = _facingRight ? new Vector2(dashForce, 0f) : new Vector2(-dashForce, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(_dashTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        _isDashing = false;
        yield return new WaitForSeconds(_dashingCooldown);
        _canDash = true;
    }
}