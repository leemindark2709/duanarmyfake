using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;
    public List<Transform> bullets;

    private void Start()
    {
        this.LoadBullets();
    }

    private void Awake()
    {
        BulletManager.instance = this;
    }

    protected virtual void LoadBullets()
    {
        foreach (Transform bullet in transform)
        {
            this.bullets.Add(bullet);
        }
    }

    public virtual Transform SpawnBullet(string bulletName, Vector3 spawnPosition, Transform player)
    {
        Transform bulletPrefab = this.GetBulletByName(bulletName);
        Transform newBullet = Instantiate(bulletPrefab);

        newBullet.position = spawnPosition;

        // Gán thông tin player cho viên đạn
        BulletFly bulletFly = newBullet.GetComponent<BulletFly>();
        if (bulletFly != null)
        {
            bulletFly.SetPlayer(player);
        }

        return newBullet;
    }

    public virtual Transform GetBulletByName(string bulletName)
    {
        foreach (Transform bullet in this.bullets)
        {
            if (bullet.name == bulletName) return bullet;
        }
        return null;
    }
}
