﻿
using UnityEngine;
using UnityEngine.UI;

public class PlayerForce : MonoBehaviour
{
    public static PlayerForce instance;
    public Image fillableImage; // Tham chiếu đến Image component
    public float fillSpeed = 10f; // Tốc độ lấp đầy
    public float lastFillAmount = 0f; // Giá trị fill amount cuối cùng trước khi reset
    public float time;
    public float fillamount;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (fillableImage == null)
        {
            Debug.LogError("Fillable Image is not assigned!");
            return;
        }

        // Khởi tạo fill amount là 0
        fillableImage.fillAmount = 0f;
    }

    void Update()
    {
        fillamount = fillableImage.fillAmount;
        if (GameManager.instance.ActivePlayer == null)
            return;

        if (GameManager.instance.ActivePlayer.GetComponent<DamageReceiver>().playertable == transform.parent.parent.parent)
        {
            time = Time.time - GameManager.instance.ActivePlayer.GetComponent<PlayerMoving>().time;
            // Kiểm tra nếu phím Space được giữ
            if (Input.GetKey(KeyCode.Space))
            {
                if (time < 10)
                {
                    fillableImage.fillAmount += Time.deltaTime * fillSpeed;
                    if (fillableImage.fillAmount > 1f)
                    {
                        fillableImage.fillAmount = 1f;
                    }

                }
                else if (time >= 10)
                {
                    // Lưu giá trị fill amount hiện tại trước khi reset
                    lastFillAmount = fillableImage.fillAmount;
                    // Đặt lại fill amount về 0 khi nhả phím Space 
                    fillableImage.fillAmount = 0f;

                }
            }

            else if (Input.GetKeyUp(KeyCode.Space))
            {
                // Lưu giá trị fill amount hiện tại trước khi reset
                lastFillAmount = fillableImage.fillAmount;
                // Đặt lại fill amount về 0 khi nhả phím Space
                fillableImage.fillAmount = 0f;
            }


        }


    }

    // Phương thức để đặt trực tiếp fill amount
    public void SetFillAmount(float amount)
    {
        fillableImage.fillAmount = Mathf.Clamp(amount, 0f, 1f); // Đảm bảo giá trị nằm trong khoảng từ 0 đến 1
    }

    // Phương thức để trả về giá trị fill amount hiện tại
    public float GetFillAmount()
    {
        return fillableImage.fillAmount;
    }

    // Phương thức để trả về giá trị fill amount cuối cùng trước khi reset
    public float GetLastFillAmount()
    {
        return lastFillAmount;
    }
    public void checkSpace()
    {

    }
}
