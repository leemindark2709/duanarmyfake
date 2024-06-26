﻿using UnityEngine;

public class BulletFly : MonoBehaviour
{
    public float force = 10f;  // Lực được truyền vào
    public float angle = 10f;   // Góc ném ban đầu
    public bool isFacingRight = false; // Hướng mặc định
    private Rigidbody2D rb;
    public Transform player;
    public float deleteDistance = 50f; // Khoảng cách để xoá đối tượng

    public void SetPlayer(Transform player)
    {
        this.player = player;
        force = this.player.GetComponent<DamageReceiver>().playertable.Find("CanvasUI").Find("Force").Find("PlayerForce").GetComponent<PlayerForce>().GetLastFillAmount() * 100;
        // Lấy góc quay từ component CanonRotation của player
        angle = player.GetComponentInChildren<CanonRotation>().rotation();

        // Lấy hướng của player từ component PlayerMoving
        isFacingRight = player.GetComponent<PlayerMoving>().IsFacingRight();
    }

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Chuyển đổi góc từ độ sang radian
        float angleInRadians = angle * Mathf.Deg2Rad;

        // Tính toán lực theo hướng và góc
        Vector2 forceDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

        // Kiểm tra hướng đối tượng và điều chỉnh hướng x
        if (!isFacingRight)
        {
            angle += 180f; // Đảo góc nếu không hướng sang phải
        }

        // Áp dụng lực ban đầu cho Rigidbody2D
        rb.AddForce(Quaternion.Euler(0, 0, angle) * Vector2.right * force, ForceMode2D.Impulse);

        // Xoá đối tượng nếu đi ra khỏi khoảng cách quy định
        Invoke("CheckDistanceWithCamera", 5f); // Gọi hàm sau 5 giây
    }

    void CheckDistanceWithCamera()
    {
        float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);

        if (distanceToCamera > deleteDistance)
        {
            Destroy(gameObject); // Xoá đối tượng
        }
        else
        {
            Invoke("CheckDistanceWithCamera", 1f); // Gọi lại hàm kiểm tra mỗi giây nếu vẫn còn trong khoảng cách
        }
    }
}
