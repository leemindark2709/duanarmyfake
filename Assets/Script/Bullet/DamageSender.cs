using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSender : MonoBehaviour
{
    public float damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageReceiver damageReceiver = other.GetComponent<DamageReceiver>();
        if (damageReceiver == null) return;  // Thêm kiểm tra null

        damageReceiver.Damaged(this.damage);

        this.Despawn();
    }

    protected virtual void Despawn()
    {
        Destroy(gameObject);  // Chỉ phá hủy game object hiện tại thay vì parent game object
    }
}
