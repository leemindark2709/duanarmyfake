using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSenderDie : MonoBehaviour
{
    public float damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageReceiver damageReceiver = other.GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {
            damageReceiver.Damaged(this.damage);
            BulletManager.instance.SpawnExplosion("ExplosionPow", other.transform.position);
        }

       
    }

  
}
