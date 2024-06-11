using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool isHoldingSpace = false;
    public string bulletname;
    public string skillName;

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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q button pressed");

            // Gọi hàm SpawnSkill khi nhấn phím Q
            SpawnSkill();
        }
    }

    void SpawnBullet()
    {
        Vector3 spawnPosition = transform.position;
        Transform newBullet = BulletManager.instance.SpawnBullet("PlayerBullet", spawnPosition, transform.parent.parent);
        GameManager.instance.GetComponent<GameManager>().turnOffComponent(transform.parent.parent);
        newBullet.gameObject.SetActive(true);
        // Đặt lại thời gian chuyển lượt thành 3 giây
        GameManager.instance.OnBulletSpawned();
    }

    void SpawnSkill()
    {
        // Kiểm tra xem SkillManager có tồn tại hay không
        if (SkillManager.Instance == null)
        {
            Debug.LogError("SkillManager instance not found!");
            return;
        }

        // Gọi phương thức Spawn của SkillManager để tạo kỹ năng
        Transform newSkill = SkillManager.Instance.Spawn("Heal", transform.parent.parent);
        if (newSkill != null)
        {
            newSkill.gameObject.SetActive(true);
            Debug.Log("Skill spawned: " + skillName);
        }
    }
}