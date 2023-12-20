using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public Vector2 moveSpeed = new Vector2(3f, 0);
    public Vector2 knockback = new Vector2(0, 0);
    Rigidbody2D _rigidbody2D;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        // si el personaje esta mirando a la izquierda, la bala se mueve a la izquierda
        _rigidbody2D.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            Vector2 deliveredknockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            //dar en el blanco
            bool gotHit = damageable.Hit(damage, deliveredknockback);
            if (gotHit)
            {
                Debug.Log(collision.name = "hit for" + damage);
                Destroy(gameObject);
            }
            
        }
    }
}
