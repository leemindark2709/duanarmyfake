using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
   
    public float hp = 0;
    public float maxHp = 10;

    private void Start()
    {
    
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
        PlayerHP.instance.UpdateHP(hp, maxHp);
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
