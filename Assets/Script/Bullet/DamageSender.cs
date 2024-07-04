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

    private void OnTriggerEnter2D(Collider2D other)
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
            TextureEditor textureEditor = other.gameObject.GetComponent<TextureEditor>();
            if (textureEditor != null)
            {
                terrainTexture = textureEditor.texture; // Gán terrainTexture từ TextureEditor
                if (terrainTexture != null)
                {
                    // Lấy vị trí va chạm trong thế giới
                    Vector2 collisionPoint = other.ClosestPoint(transform.position);

                    // Chuyển đổi vị trí va chạm sang tọa độ màn hình (screen position)
                    Vector2 screenPosition = Camera.main.WorldToScreenPoint(collisionPoint);

                    // Gọi phương thức Erase của TextureEditor với tọa độ màn hình
                    textureEditor.Erase(screenPosition);
                }
                else
                {
                    Debug.LogError("Texture từ TextureEditor bị null.");
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy component TextureEditor trên đối tượng map.");
            }
        }

        Despawn();
    }

    protected virtual void Despawn()
    {
        Destroy(gameObject); // Phá hủy đối tượng game hiện tại (viên đạn)
    }

    
}
