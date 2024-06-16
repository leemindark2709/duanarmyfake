using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]private bool isHoldingSpace = false;
    public string bulletname = "PlayerBullet";
    public string skillName;
    public int numofusesq;
    public int numofusese;
    public int numofusesr;
    public float hp;
    private DamageReceiver damageReceiver;
    private SkillInGame skillInGame;
    private Image healImage;
    private Image flashImage;
    private Image powImage;
    public float time;

    void Start()
    {
        // Lấy các component cần thiết và lưu trữ trong biến
        damageReceiver = transform.parent.parent.GetComponent<DamageReceiver>();
        skillInGame = transform.parent.parent.GetComponent<SkillInGame>();

        hp = damageReceiver.getHp();
        numofusese = skillInGame.numofusese;
        numofusesq = skillInGame.numofusesq;
        numofusesr = skillInGame.numofusesr;

        healImage = damageReceiver.playertable.Find("CanvasUI").Find("Heal").Find("Heal").GetComponent<Image>();
        flashImage = damageReceiver.playertable.Find("CanvasUI").Find("Flash").Find("Flash").GetComponent<Image>();
        powImage = damageReceiver.playertable.Find("CanvasUI").Find("Pow").Find("Pow").GetComponent<Image>();

        this.bulletname = "PlayerBullet";

        // Kiểm tra trạng thái ban đầu của máu để cập nhật powImage
        CheckHp();
    }

    void Update()
    {
        time = Time.time - transform.parent.parent.GetComponent<PlayerMoving>().time;
        hp = damageReceiver.getHp();

        if (Input.GetKey(KeyCode.Space) && (Time.time - transform.parent.parent.GetComponent<PlayerMoving>().time) < 10)
        {
            isHoldingSpace = true;
            Debug.Log("Space button pressed");
        }
        if (Input.GetKey(KeyCode.Space) && (Time.time - transform.parent.parent.GetComponent<PlayerMoving>().time) >= 10)
        {
            isHoldingSpace = false;
            Debug.Log("Space button released");
            SpawnBullet(this.bulletname);
            damageReceiver.playertable.Find("CanvasUI").Find("Force").Find("PlayerForce").GetComponent<PlayerForce>().enabled = false;
            this.bulletname = "PlayerBullet";
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingSpace = false;
            Debug.Log("Space button released");
            SpawnBullet(this.bulletname);
            damageReceiver.playertable.Find("CanvasUI").Find("Force").Find("PlayerForce").GetComponent<PlayerForce>().enabled = false;
            this.bulletname = "PlayerBullet";
        }

        if (Input.GetKeyDown(KeyCode.Q) && numofusesq > 0)
        {
            UseSkillQ();
        }

        if (Input.GetKeyDown(KeyCode.E) && numofusese > 0)
        {
            UseSkillE();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            UseSkillR();
        }

        // Gọi hàm kiểm tra máu trong mỗi khung hình
        CheckHp();
    }

    void UseSkillQ()
    {
        Debug.Log("Q button pressed");
        SpawnSkill("Heal");
        numofusesq -= 1;
        skillInGame.numofusesq = numofusesq;

        if (numofusesq == 0)
        {
            healImage.enabled = false;
        }
    }

    void UseSkillE()
    {
        Debug.Log("E button pressed");
        bulletname = "PlayerBulletFlash";
        numofusese -= 1;
        skillInGame.numofusese = numofusese;

        if (numofusese == 0)
        {
            flashImage.enabled = false;
        }
    }

    void UseSkillR()
    {
        Debug.Log("R button pressed");
        Debug.Log("Current HP: " + hp);
        if (hp < 5 && numofusesr > 0)
        {
            Debug.Log("HP is less than 5, setting bulletname to PlayerPow");
            bulletname = "PlayerPow";
            numofusesr -= 1;
            skillInGame.numofusesr = numofusesr;
           
        }
        powImage.enabled = false;
    }

    void SpawnBullet(string bulletname)
    {
        Debug.Log("ok");
        Vector3 spawnPosition = transform.position;
        Transform newBullet = BulletManager.instance.SpawnBullet(bulletname, spawnPosition, transform.parent.parent);

        GameManager.instance.GetComponent<GameManager>().turnOffComponent(transform.parent.parent);
        newBullet.gameObject.SetActive(true);
        GameManager.instance.OnBulletSpawned();
    }

    void SpawnSkill(string skill)
    {
        if (SkillManager.Instance == null)
        {
            Debug.LogError("SkillManager instance not found!");
            return;
        }

        Transform newSkill = SkillManager.Instance.Spawn(skill, transform.parent.parent);
        if (newSkill != null)
        {
            newSkill.gameObject.SetActive(true); 
            Debug.Log("Skill spawned: " + skillName);
        }
    }

    void CheckHp()
    {
        if (numofusesr>0)
        {
            powImage.enabled = hp < 5;
        }
     
    }
}
