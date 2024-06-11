using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHeal : MonoBehaviour
{
    public float cooldownTime = 5f; // Thời gian hồi chiêu
    public float bloodAmount = 20f; // Lượng máu hồi
    private float nextHealTime = 0f; // Thời điểm có thể hồi máu tiếp the
    // Start is called before the first frame update
    public Transform player;
   
    // Update is called once per frame
    void Start()
    {
       
            HealPlayer(this.player);
            nextHealTime = Time.time + cooldownTime; 
        
    }

    void HealPlayer(Transform player)
    {
        float maxHp = player.GetComponent<DamageReceiver>().maxHp;
        player.GetComponent<DamageReceiver>().hp = player.GetComponent<DamageReceiver>().hp + 1;
        
        float hp = player.GetComponent<DamageReceiver>().hp;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
       
        player.GetComponent<DamageReceiver>().playertable.Find("CanvasUI").Find("BloodBar").Find("BloodBar").GetComponent<PlayerHP>().UpdateHP(hp,maxHp);
    }
}
