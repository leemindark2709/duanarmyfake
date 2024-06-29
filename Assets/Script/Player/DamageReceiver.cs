using UnityEngine;
using UnityEngine.UI;
public class DamageReceiver : MonoBehaviour
{
    public static DamageReceiver Instance { get; private set; }
    public Transform playertable;
    public PlayerHP playerHP;
    public float Hp = 0;
    public float maxHp = 10;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Lấy tham chiếu tới PlayerHP
        playerHP = playertable.Find("CanvasUI").Find("BloodBar").Find("BloodBar").GetComponent<PlayerHP>();
        setImage();
        // Reset HP khi bắt đầu
        ResetHP();
    }

    private void Update()
    {
        // Kiểm tra và cập nhật trạng thái của Pow image
        CheckHp();

        // Kiểm tra khoảng cách với camera và xoá đối tượng nếu cần
        CheckDistanceWithCamera();
    }

    private void OnEnable()
    {
        ResetHP();
    }

    protected virtual void ResetHP()
    {
        Hp = maxHp;
    }

    public virtual void Damaged(float damage)
    {
        Hp -= damage;
        playerHP.UpdateHP(Hp, maxHp);

        if (Hp <= 0)
        {
            Hp = 0;
            Dying();
        }
    }

    protected virtual void Dying()
    {
        if (IsAlive()) return;
        Debug.Log(transform.name + " Dying");

        Despawn();
    }

    protected virtual bool IsAlive()
    {
        return Hp > 0;
    }

    protected virtual void Despawn()
    {
        // Xóa player khỏi danh sách và hàng đợi trong GameManager
        GameManager.instance.RemovePlayer(transform);
        GameManager.instance.GetRemainingTeam();
        Destroy(gameObject);
    }

    public float getHp()
    {
        return Hp;
    }

    void CheckHp()
    {
        var playerAttack = transform.Find("Canon").Find("PlayerAttack").GetComponent<PlayerAttack>();
        if (playerAttack.numofusesr > 0)
        {
            playertable.Find("CanvasUI").Find("Pow").Find("Pow").GetComponent<Image>().enabled = Hp < 5;
        }
    }

    void setImage()
    {
        this.playertable.Find("CanvasUI").Find("Image").GetComponent<Image>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
    }

   void CheckDistanceWithCamera()
{
    float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
    float deleteDistance = 50f; // Khoảng cách cần để xoá đối tượng

    if (distanceToCamera > deleteDistance)
    {
        Hp = 0; // Đặt HP về 0
        playerHP.UpdateHP(Hp, maxHp); // Cập nhật UI nếu cần
        Dying(); // Gọi hàm Dying để xoá đối tượng
    }
}

}
