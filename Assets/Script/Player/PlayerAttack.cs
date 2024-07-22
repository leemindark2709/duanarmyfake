using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private bool isHoldingSpace = false;
    public string bulletname;
    private string initialBulletName;
    public string skillName;
    public int numofusesq;
    public int numofusese;
    public int numofusesr;
    public float hp;
    public DamageReceiver damageReceiver;
    private SkillInGame skillInGame;
    private Image healImage;
    private Image flashImage;
    private Image powImage;
    private TextMeshProUGUI textNumofuseq;
    private TextMeshProUGUI textNumofusee;
    public float time;
    [SerializeField] private AudioManager audioManager; // Corrected the typo

    private void Awake()
    {

        // Gán AudioSource component
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Start()
    {

        // Lấy các component cần thiết và lưu trữ trong biến
        damageReceiver = transform.parent.parent.GetComponent<DamageReceiver>();
        skillInGame = transform.parent.parent.GetComponent<SkillInGame>();
        skillInGame.numofusese = 5;
        skillInGame.numofusesq = 2;
        skillInGame.numofusesr = 1;
        hp = damageReceiver.getHp();
        numofusese = skillInGame.numofusese;
        numofusesq = skillInGame.numofusesq;
        numofusesr = skillInGame.numofusesr;

        textNumofuseq = damageReceiver.playertable.Find("CanvasUI").Find("numofuseq").GetComponent<TextMeshProUGUI>();
        textNumofusee = damageReceiver.playertable.Find("CanvasUI").Find("numofusee").GetComponent<TextMeshProUGUI>();
        healImage = damageReceiver.playertable.Find("CanvasUI").Find("Heal").Find("Heal").GetComponent<Image>();
        flashImage = damageReceiver.playertable.Find("CanvasUI").Find("Flash").Find("Flash").GetComponent<Image>();
        powImage = damageReceiver.playertable.Find("CanvasUI").Find("Pow").Find("Pow").GetComponent<Image>();

        initialBulletName = bulletname;

        // Kiểm tra trạng thái ban đầu của máu để cập nhật powImage
        CheckHp();
    }

    void Update()
    {
        textNumofuseq.text = numofusesq.ToString();
        textNumofusee.text = numofusese.ToString();
        time = Time.time - transform.parent.parent.GetComponent<PlayerMoving>().time;
        hp = damageReceiver.getHp();

        if (Input.GetKey(KeyCode.Space))
        {
            if (time < 10)
            {
                isHoldingSpace = true;
                Debug.Log("Space button pressed");
            }
            else
            {
                isHoldingSpace = false;
                Debug.Log("Space button released");

                SpawnBullet(bulletname);
                ResetTimers();
                DisablePlayerForce();
                ResetTimerImage();

                bulletname = initialBulletName;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            ResetTimerImage();
            isHoldingSpace = false;
            Debug.Log("Space button released");
            SpawnBullet(bulletname);
            ResetTimers();
            bulletname = initialBulletName;
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
        audioManager.PlaySFX(audioManager.HealSkill);
        Debug.Log("Q button pressed");
        SpawnSkill("Heal");
        numofusesq -= 1;

        skillInGame.numofusesq = numofusesq;
        textNumofuseq.text = numofusesq.ToString();
        if (numofusesq == 0)
        {
            healImage.enabled = false;
        }
    }

    void UseSkillE()
    {
        audioManager.PlaySFX(audioManager.Flash);
        Debug.Log("E button pressed");
        bulletname = "PlayerBulletFlash";
        numofusese -= 1;
        skillInGame.numofusese = numofusese;
        textNumofusee.text = numofusese.ToString();
        if (numofusese == 0)
        {
            flashImage.enabled = false;
        }
    }

    void UseSkillR()
    {
        audioManager.PlaySFX(audioManager.Pow);
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

    void SpawnBullet(string bulletName)
    {
        Debug.Log("ok");
        audioManager.PlaySFX(audioManager.SpawBulletSound);
        Vector3 spawnPosition = transform.position;
        Transform newBullet = BulletManager.instance.SpawnBullet(bulletName, spawnPosition, transform.parent.parent);

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
            Debug.Log("Skill spawned: " + skill);
        }
    }

    void CheckHp()
    {
        if (numofusesr > 0)
        {
            powImage.enabled = hp < 5;
        }
    }

    void ResetTimers()
    {
        var playerForce = damageReceiver.playertable.Find("CanvasUI").Find("Force").Find("PlayerForce").GetComponent<PlayerForce>();
        var timeActive = damageReceiver.playertable.Find("CanvasUI").Find("TimeActive").Find("Time").GetComponent<TimeActive>();

        playerForce.enabled = false;
        timeActive.timerImage.fillAmount = 0f;
        timeActive.elapsedTime = 0;
        timeActive.enabled = false;
    }

    void DisablePlayerForce()
    {
        var playerForce = damageReceiver.playertable.Find("CanvasUI").Find("Force").Find("PlayerForce").GetComponent<PlayerForce>();
        playerForce.enabled = false;
    }

    void ResetTimerImage()
    {
        var timeActive = damageReceiver.playertable.Find("CanvasUI").Find("TimeActive").Find("Time").GetComponent<TimeActive>();
        timeActive.timerImage.fillAmount = 0f;
    }
}
