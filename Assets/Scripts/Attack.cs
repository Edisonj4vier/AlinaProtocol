using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ver si se puede golpear
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            //aplicar knockback
            Vector2 deliveredknockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            //dar en el blanco
            bool gotHit = damageable.Hit(attackDamage, deliveredknockback);
            if (gotHit)
            {
                Debug.Log(collision.name = "hit for" + attackDamage);
            }
        }
    }
}
