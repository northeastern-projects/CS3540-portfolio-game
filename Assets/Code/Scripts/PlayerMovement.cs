using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    public float dashSpeed;
    public Rigidbody2D rb;
    private bool _facingRight = true;

    private float _moveDirection;
    private float _moveVertical;
    private bool _isJumping;

    
    // Awake is called after objects are initialized. Called in a random order
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        ProcessInputs();

        Animate();
    }

    // better for handling physics
    private void FixedUpdate()
    {
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
        rb.velocity = new Vector2(_moveDirection * moveSpeed, rb.velocity.y);
        if (!_isJumping && _moveVertical > 0.1f)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

        if (Input.GetButtonDown("Fire1") && _facingRight)
        {
            rb.AddForce(new Vector2(dashSpeed, 0f), ForceMode2D.Impulse);
        }
        else if (Input.GetButtonDown("Fire1") && !_facingRight)
        {
            rb.AddForce(new Vector2(-dashSpeed, 0f), ForceMode2D.Impulse);
        }
    }
    private void ProcessInputs()
    {
        _moveDirection = Input.GetAxis("Horizontal");
        _moveVertical = Input.GetAxis("Vertical");
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
}