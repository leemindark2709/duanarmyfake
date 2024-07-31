using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSenderFlash : MonoBehaviour
{
    public float damage = 1;      // Số sát thương gây ra
    public Transform player;      // Tham chiếu đến Transform của người chơi

    // Start is called before the first frame update
    void Start()
    {
        player = transform.GetComponent<BulletFly>().player;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Xử lý sự kiện khi có va chạm 2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu đối tượng va chạm có component DamageReceiver
        DamageReceiver damageReceiver = other.GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {
            // Gây sát thương
            damageReceiver.Damaged(this.damage);
        }

        // Dịch chuyển người chơi đến vị trí va chạm
        if (player != null)
        {
            player.position = this.transform.position;
        }

        // Phá hủy game object hiện tại (viên đạn)
        this.Despawn();
    }
    public virtual void movePlayer()
    {

    }
    // Hàm để phá hủy game object
    protected virtual void Despawn()
    {
        Destroy(gameObject); // Phá hủy game object hiện tại (viên đạn)
    }
}
