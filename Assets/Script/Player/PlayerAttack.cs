using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool isHoldingSpace = false;
    public string bulletname="PlayerBullet";
    public string skillName;

    void Start()
    {
        this.bulletname = "PlayerBullet";
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
            SpawnBullet(this.bulletname);
            this.bulletname = "PlayerBullet";
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q button pressed");

            // Gọi hàm SpawnSkill khi nhấn phím Q
            SpawnSkill("Heal");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("W button pressed");  

            // Gọi hàm SpawnSkill khi nhấn phím Q
            this.bulletname = "PlayerBulletFlash";
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R button pressed");

            // Gọi hàm SpawnSkill khi nhấn phím Q
            this.bulletname = "PlayerPow";
        }
    }

    void SpawnBullet(string bulletname)
    {
        Debug.Log("ok");
        Vector3 spawnPosition = transform.position;
        Transform newBullet = BulletManager.instance.SpawnBullet(bulletname, spawnPosition, transform.parent.parent);

        GameManager.instance.GetComponent<GameManager>().turnOffComponent(transform.parent.parent);
        newBullet.gameObject.SetActive(true);
        // Đặt lại thời gian chuyển lượt thành 3 giây
        GameManager.instance.OnBulletSpawned();
    }

    void SpawnSkill(string skill)
    {
        // Kiểm tra xem SkillManager có tồn tại hay không
        if (SkillManager.Instance == null)
        {
            Debug.LogError("SkillManager instance not found!");
            return;
        }

        // Gọi phương thức Spawn của SkillManager để tạo kỹ năng
        Transform newSkill = SkillManager.Instance.Spawn(skill, transform.parent.parent);
        if (newSkill != null)
        {
            newSkill.gameObject.SetActive(true);
            Debug.Log("Skill spawned: " + skillName);
        }
    }
}