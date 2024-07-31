using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSenderDie : MonoBehaviour
{
    [SerializeField] private AudioManager audioManger; // Biến để lưu trữ AudioSource
    public float damage = 10000;
    private void Awake()
    {
        damage = 10000;
    audioManger = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageReceiver damageReceiver = other.GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {
            audioManger.PlaySFX(audioManger.DameSender);
            damageReceiver.Damaged(this.damage);
            BulletManager.instance.SpawnExplosion("ExplosionPow", other.transform.position);
        }

       
    }

  
}
