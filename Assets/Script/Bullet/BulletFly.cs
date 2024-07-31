using UnityEngine;

public class BulletFly : MonoBehaviour
{
    public float force = 10f;
    public float angle = 10f;
    public bool isFacingRight = false;
    private Rigidbody2D rb;
    public Transform player;
    public float deleteDistance = 50f;

    public void SetPlayer(Transform player)
    {
        this.player = player;
        force = this.player.GetComponent<DamageReceiver>().playertable.Find("CanvasUI").Find("Force").Find("PlayerForce").GetComponent<PlayerForce>().GetLastFillAmount() * 20;
        angle = player.GetComponentInChildren<CanonRotation>().rotation();
        isFacingRight = player.GetComponent<PlayerMoving>().IsFacingRight();
    }

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float angleInRadians = angle * Mathf.Deg2Rad;
        Vector2 forceDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

        if (!isFacingRight)
        {
            angle += 180f;
        }

        rb.AddForce(Quaternion.Euler(0, 0, angle) * Vector2.right * force, ForceMode2D.Impulse);
        Invoke("CheckDistanceWithCamera", 5f);
    }

    void CheckDistanceWithCamera()
    {
        float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);

        if (distanceToCamera > deleteDistance)
        {
            Destroy(gameObject);
        }
        else
        {
            Invoke("CheckDistanceWithCamera", 1f);
        }
    }
}
