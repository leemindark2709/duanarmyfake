using UnityEngine;
using UnityEngine.UI;

public class TimeActive : MonoBehaviour
{
    public static TimeActive instance;
    public Image timerImage; // Tham chiếu đến Image component của thanh thời gian
    public float totalTime = 10f; // Tổng thời gian từ 1 đến 0
    public float elapsedTime = 0f; // Thời gian đã trôi qua

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (timerImage == null)
        {
            Debug.LogError("Timer Image is not assigned!");
            return;
        }

        // Khởi tạo fill amount của timerImage là 1 (tương ứng với 10 giây)
       
       
    }

    void Update()
    {
        
        if (GameManager.instance.ActivePlayer == null)
            return;
        if (GameManager.instance.ActivePlayer.GetComponent<DamageReceiver>().playertable == transform.parent.parent.parent)
        {
            
            elapsedTime += Time.deltaTime;
            timerImage.fillAmount = Mathf.Clamp(1f - (elapsedTime / totalTime), 0f, 1f);

            if (elapsedTime >= totalTime)
            {
                // Reset elapsedTime khi hết 10 giây
                elapsedTime = 0f;
                timerImage.fillAmount = 0f;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                this.elapsedTime = 0f;
                timerImage.fillAmount = 0f;
            }
        }
    }
}
