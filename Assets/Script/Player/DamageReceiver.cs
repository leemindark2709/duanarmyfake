using UnityEngine;
using static Unity.Collections.AllocatorManager;
using UnityEngine.Timeline;

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
        //playerHP = GetComponent<PlayerHP>();    
        playerHP = playertable.Find("CanvasUI").Find("BloodBar").Find("BloodBar").GetComponent<PlayerHP>();



    }
    private void OnEnable()
    {
        this.ResetHP();
    }

    protected virtual void ResetHP()
    {
        this.Hp = this.maxHp;
    }

    public virtual void Damaged(float damage)
    {
        this.Hp -= damage;
        //playerHP.UpdateHP(hp, maxHp);
       playerHP.UpdateHP(Hp, maxHp); 
        if (this.Hp <= 0) this.Hp = 0;

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
        return this.Hp > 0;
    }

    protected virtual void Despawn()
    {
        // Xóa player khỏi danh sách và hàng đợi trong GameManager
        GameManager.instance.RemovePlayer(transform);
        Destroy(transform.gameObject);
    }
     public float getHp()
    {
        return this.Hp;
    }
}
