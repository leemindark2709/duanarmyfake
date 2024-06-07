using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public float hp = 0;
    public float maxHp = 10;

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
        GameManager.instance.RemovePlayer(transform);
        Destroy(transform.gameObject);
       
    }
}
