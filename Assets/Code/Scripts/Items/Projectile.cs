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
        transform.position = new Vector2(transform.position + 1 * data.speed * Time.DeltaTime, rb.velocity.y - rb.gravityScale * Time.DeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collider.GetComponent<Health>())
        {
            collider.GetComponent<Health>().health -= data.damage;
        }
    }
}
