using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Collider2D attackCollider;
    public int attackDamage = 10;
    public Vector2 knockback=Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision){
        damagable damagable = collision.GetComponent<damagable>();
        if (damagable != null){
            bool gotHit=damagable.Hit(attackDamage,knockback);
            
            
        }
    }
}
