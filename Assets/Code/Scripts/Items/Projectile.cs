using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private ProjectileData data;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x + 1 * data.speed * Time.deltaTime, rb.velocity.y - rb.gravityScale * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Health>())
        {
            collision.gameObject.GetComponent<Health>().Damage(data.damage);
        }
    }
}
