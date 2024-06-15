using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DamageSender : MonoBehaviour
{
    public float damage = 1;

    private  void OnTriggerEnter2D(Collider2D other)
    {
        DamageReceiver damageReceiver = other.GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {
            damageReceiver.Damaged(this.damage);

          

        }

        // Kiểm tra xem đối tượng va chạm có phải là Tilemap hay không
        //Tilemap tilemap = other.GetComponent<Tilemap>();
        //if (tilemap != null)
        //{
        //    // Xử lý va chạm với Tilemap
        //    Vector3 hitPosition = other.transform.position;
        //    Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);
        //    if (tilemap.HasTile(tilePosition))
        //    {
        //        Debug.Log("Tile collided at: " + tilePosition);
        //        tilemap.SetTile(tilePosition, null);
        //    }
        //}

        this.Despawn();
    }

    protected virtual void Despawn()
    {
        Destroy(gameObject);  // Phá hủy game object hiện tại (viên đạn)
    }
}
