using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool isHoldingSpace = false;
    public string bulletname;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isHoldingSpace = true;
            Debug.Log("Space button pressed");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingSpace = false;
            Debug.Log("Space button released");

            // Gọi hàm SpawnBullet khi nút Space được thả ra
            SpawnBullet();
        }
    }

    void SpawnBullet()
    {
        
        Vector3 spawnPosition = transform.position;
        Transform newBullet= BulletManager.instance.SpawnBullet("PlayerBullet", spawnPosition, transform.parent.parent); 
        GameManager.instance.GetComponent<GameManager>().turnOffComponent(transform.parent.parent);
        newBullet.gameObject.SetActive(true);
        // Đặt lại thời gian chuyển lượt thành 3 giây
        GameManager.instance.OnBulletSpawned();
    }
}
