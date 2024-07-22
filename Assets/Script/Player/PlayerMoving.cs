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

    private AudioManager audioManager;
    private bool isMoving;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        distance = 0;
        lastPosition = transform.position;
        animator = GetComponent<Animator>();
        // Initially, disable the Animator
        animator.enabled = false;
        audioManager = FindObjectOfType<AudioManager>();
        isMoving = false;
    }

    private void OnEnable()
    {
        time = Time.time;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Check if the player is moving
        if (Mathf.Abs(horizontalInput) > 0)
        {
            // Enable Animator if not already enabled
            if (!animator.enabled)
                animator.enabled = true;

            // Play moving sound if not already playing
            if (!isMoving)
            {
                audioManager.MovingSound();
                isMoving = true;
            }
        }
        else
        {
            // Disable Animator if the player stops
            animator.enabled = false;

            // Stop moving sound if playing
            if (isMoving)
            {
                audioManager.StopBackgroundMusic();
                isMoving = false;
            }
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

        // Giữ nguyên hướng của phần tử con Status
        Transform status = transform.Find("Status");
        if (status != null)
        {
            Vector3 statusScale = status.localScale;
            statusScale.x = -statusScale.x; // Đảm bảo x luôn dương
            status.localScale = statusScale;
        }
    }

    // Hàm để lấy chiều hiện tại của nhân vật
    public bool IsFacingRight()
    {
        return facingRight;
    }
}
