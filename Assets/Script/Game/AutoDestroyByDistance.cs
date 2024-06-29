using UnityEngine;

public class AutoDestroyByDistance : MonoBehaviour
{
    public float deleteDistance = 50f; // Khoảng cách cần để xoá đối tượng

    void Update()
    {
        // Kiểm tra khoảng cách với camera và xoá đối tượng nếu cần
        CheckDistanceWithCamera();
    }

    void CheckDistanceWithCamera()
    {
        float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);

        if (distanceToCamera > deleteDistance)
        {
            Destroy(gameObject);
        }
    }
}
