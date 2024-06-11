using UnityEngine;
using static Unity.Collections.AllocatorManager;
using UnityEngine.Timeline;

public class DamageReceiver : MonoBehaviour
{
    public static DamageReceiver Instance { get; private set; }
    public Transform playertable;
    public PlayerHP playerHP;
    public float hp = 0;
    public float maxHp = 10;
    private void Awake()
    {
            Instance = this;
    }
    private void Start()
    {
        //playerHP = GetComponent<PlayerHP>();    
        playerHP = playertable.Find("CanvasUI").Find("BloodBar").Find("BloodBar").GetComponent<PlayerHP>();


    }
    private void OnEnable()
    {
        this.ResetHP();
    }

    protected virtual void ResetHP()
    {
        this.hp = this.maxHp;
    }

    public virtual void Damaged(float damage)
    {
        this.hp -= damage;
        //playerHP.UpdateHP(hp, maxHp);
       playerHP.UpdateHP(hp, maxHp); 
        if (this.hp <= 0) this.hp = 0;

        this.Dying();
    }

    protected virtual void Dying()
    {
        if (this.IsAlive()) return;
        Debug.Log(transform.name + " Dying");

        this.Despawn();
    }

    protected virtual bool IsAlive()
    {
        return this.hp > 0;
    }

    protected virtual void Despawn()
    {
        // Xóa player khỏi danh sách và hàng đợi trong GameManager
        GameManager.instance.RemovePlayer(transform);
        Destroy(transform.gameObject);
    }
}
