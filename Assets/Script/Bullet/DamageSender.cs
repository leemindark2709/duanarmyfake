using UnityEngine;

public class DamageSender : MonoBehaviour
{
    public float damage = 1f;
    [SerializeField] private AudioManager audioManager; // Biến để lưu trữ AudioManager
    [SerializeField] private Texture2D terrainTexture; // Thêm biến để lưu trữ texture

    private void Awake()
    {
        // Gán AudioManager component
        audioManager = GameObject.Find("AudioManager")?.GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager không được tìm thấy trong cảnh.");
        }
    }

    private void  OnTriggerEnter2D(Collider2D other)
    {
        DamageReceiver damageReceiver = other.GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {
            audioManager?.PlaySFX(audioManager.DameSender);
            damageReceiver.Damaged(this.damage);
            BulletManager.instance.SpawnExplosion("ExplosionBullet", other.transform.position);
        }

        if (other.CompareTag("map"))
        {

                    Vector2 collisionPoint = other.ClosestPoint(transform.position);

                    Vector2 screenPosition = Camera.main.WorldToScreenPoint(collisionPoint);

                    audioManager?.PlaySFX(audioManager.DameSender);
                    BulletManager.instance.SpawnExplosion("ExplosionBullet", collisionPoint);

            }
        Despawn();
    }

    protected virtual void Despawn()
    {
        Destroy(gameObject); // Phá hủy đối tượng game hiện tại (viên đạn)
    }

    
}
