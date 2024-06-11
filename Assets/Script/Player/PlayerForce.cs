using UnityEngine;
using UnityEngine.UI;

public class PlayerForce : MonoBehaviour
{
    public static PlayerForce instance; 
    public Image fillableImage; // Tham chiếu đến Image component
    public float fillSpeed = 10f; // Tốc độ lấp đầy
    private float lastFillAmount = 0f; // Giá trị fill amount cuối cùng trước khi reset
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
        checkSpace();

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
        // Kiểm tra nếu phím Space được giữ
        if (Input.GetKey(KeyCode.Space))
        {
            // Tăng fill amount theo thời gian cho đến khi đạt 1
            fillableImage.fillAmount += Time.deltaTime * fillSpeed;
            if (fillableImage.fillAmount > 1f)
            {
                fillableImage.fillAmount = 1f;
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
