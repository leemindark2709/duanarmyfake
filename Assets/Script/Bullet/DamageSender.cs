using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DamageSender : MonoBehaviour
{
    public float damage = 1;
    [SerializeField] private AudioManager audioManager; // Biến để lưu trữ AudioSource

    private void Awake()
    {
        // Gán AudioSource component
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageReceiver damageReceiver = other.GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {
            audioManager.PlaySFX(audioManager.DameSender);
            damageReceiver.Damaged(this.damage);
            BulletManager.instance.SpawnExplosion("ExplosionBullet", other.transform.position);
        }

        // Kiểm tra xem đối tượng va chạm có phải là Tilemap hay không
        Tilemap tilemap = other.GetComponent<Tilemap>();
        if (tilemap != null)
        {
            // Xử lý va chạm với Tilemap
            Vector3 hitPosition = other.ClosestPoint(transform.position);
            Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);
            if (tilemap.HasTile(tilePosition))
            {
                Debug.Log("Tile collided at: " + tilePosition);
                tilemap.SetTile(tilePosition, null);
            }
        }

        // Kiểm tra xem đối tượng va chạm có tên là "map" hay không
        if (other.gameObject.name == "map")
        {
            Debug.Log("Collided with map object: " + other.gameObject.name);
            Destroy(other.gameObject); // Xóa đối tượng map
        }

        this.Despawn();
    }

    protected virtual void Despawn()
    {
        Destroy(gameObject);  // Phá hủy game object hiện tại (viên đạn)
    }
}
