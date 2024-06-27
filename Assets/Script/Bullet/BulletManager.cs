using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;
    public List<Transform> bullets = new List<Transform>(); // Khởi tạo danh sách để tránh lỗi null reference
    public List<Transform> explosions = new List<Transform>(); // Khởi tạo danh sách để tránh lỗi null reference

    private void Awake()
    {
        BulletManager.instance = this;
    }

    private void Start()
    {
        this.LoadBullets();
        this.LoadExplosions();
        this.HideAll();
    }

    protected virtual void LoadBullets()
    {
        foreach (Transform bullet in transform)
        {
            this.bullets.Add(bullet);
        }
    }

    protected virtual void LoadExplosions()
    {
        GameObject explosionParent = GameObject.Find("Explosion");
        if (explosionParent != null)
        {
            foreach (Transform explosion in explosionParent.transform)
            {
                this.explosions.Add(explosion);
            }
        }
        else
        {
            Debug.LogWarning("Explosion GameObject not found!");
        }
    }

    protected virtual void HideAll()
    {
        foreach (Transform bullet in this.bullets)
        {
            bullet.gameObject.SetActive(false);
        }

        foreach (Transform explosion in this.explosions)
        {
            explosion.gameObject.SetActive(false);
        }
    }

    public virtual Transform SpawnBullet(string bulletName, Vector3 spawnPosition, Transform player)
    {
        Transform bulletPrefab = this.GetBulletByName(bulletName);
        if (bulletPrefab != null)
        {
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
        else
        {
            Debug.LogWarning("Bullet with name " + bulletName + " not found!");
            return null;
        }
    }

    public virtual Transform GetBulletByName(string bulletName)
    {
        foreach (Transform bullet in this.bullets)
        {
            if (bullet.name == bulletName)
            {
                return bullet;
            }
        }
        return null;
    }

    public virtual Transform GetExplosionByName(string explosionName)
    {
        foreach (Transform explosion in this.explosions)
        {
            if (explosion.name == explosionName)
            {
                return explosion;
            }
        }
        return null;
    }

    public virtual Transform SpawnExplosion(string explosionName, Vector3 spawnPosition)
    {
        Transform explosionPrefab = this.GetExplosionByName(explosionName);
        if (explosionPrefab != null)
        {
            Transform newExplosion = Instantiate(explosionPrefab);
            newExplosion.position = spawnPosition;
            newExplosion.gameObject.SetActive(true);

            StartCoroutine(DestroyAfterDelay(newExplosion.gameObject, 0.5f));

            return newExplosion;
        }
        else
        {
            Debug.LogWarning("Explosion with name " + explosionName + " not found!");
            return null;
        }
    }

    private IEnumerator DestroyAfterDelay(GameObject explosion, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(explosion);
    }
}
