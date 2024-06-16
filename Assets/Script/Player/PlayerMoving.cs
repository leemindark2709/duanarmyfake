using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public static PlayerMoving Instance;
    public Animator animator;
    public float moveSpeed = 5f;
    public bool facingRight = true;
    public float distance = 0f;
    private Vector3 lastPosition;
    public float time;

    public object Tỉme { get; internal set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        
        distance = 0;
        lastPosition = transform.position;
        animator = GetComponent<Animator>();
        // Ban đầu, tắt Animator
        animator.enabled = false;
    }
    private void OnEnable()
    {
        time = Time.time;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Kiểm tra xem nhân vật có đang di chuyển không
        if (Mathf.Abs(horizontalInput) > 0)
        {
            // Bật Animator nếu chưa được bật
            if (!animator.enabled)
                animator.enabled = true;
        }
        else
        {
            // Tắt Animator nếu nhân vật dừng lại
            animator.enabled = false;
        }

        if (horizontalInput < 0 && facingRight || horizontalInput > 0 && !facingRight)
        {
            Flip();
        }

        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * moveSpeed * Time.deltaTime / 10;
        transform.Translate(movement);

        distance += Mathf.Abs(transform.position.x - lastPosition.x);
        lastPosition = transform.position;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Hàm để lấy chiều hiện tại của nhân vật
    public bool IsFacingRight()
    {
        return facingRight;
    }
}
