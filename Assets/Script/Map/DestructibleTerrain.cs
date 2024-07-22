using UnityEngine;
using System.Collections.Generic;

public class DestructibleTerrain : MonoBehaviour
{
    public Texture2D terrainTexture;
    public PolygonCollider2D terrainCollider;
    public float holeRadius = 0.5f; // Bán kính của vòng tròn trong đơn vị world

    void Start()
    {
        // Chuyển đổi Texture2D thành Sprite
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = Sprite.Create(terrainTexture, new Rect(0, 0, terrainTexture.width, terrainTexture.height), Vector2.one * 0.5f);
    }


    public void DestroyTerrain(Vector2 position)
    {
        Vector2Int pixelPos = WorldToTextureCoords(position);

        // Tạo lỗ tại vị trí va chạm
        CreateHole(pixelPos);

        // Cập nhật lại collider
        UpdateCollider();
    }

    Vector2Int WorldToTextureCoords(Vector2 worldPos)
    {
        Vector3 localPos = transform.InverseTransformPoint(worldPos);
        float px = localPos.x * terrainTexture.width / transform.localScale.x;
        float py = localPos.y * terrainTexture.height / transform.localScale.y;
        return new Vector2Int(Mathf.RoundToInt(px), Mathf.RoundToInt(py));
    }

    void CreateHole(Vector2Int position)
    {
        int radius = Mathf.RoundToInt(holeRadius * terrainTexture.width); // Chuyển đổi bán kính từ đơn vị world sang pixel

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (x * x + y * y <= radius * radius)
                {
                    int pixelX = position.x + x;
                    int pixelY = position.y + y;

                    if (pixelX >= 0 && pixelX < terrainTexture.width && pixelY >= 0 && pixelY < terrainTexture.height)
                    {
                        terrainTexture.SetPixel(pixelX, pixelY,Color.black);
                        Debug.LogWarning("ok");
                    }
                }
            }
        }
        terrainTexture.Apply();
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = Sprite.Create(terrainTexture, new Rect(0, 0, terrainTexture.width, terrainTexture.height), Vector2.one * 0.5f);
       
    }

    void UpdateCollider()
    {
        // Lấy dữ liệu alpha của texture
        bool[,] alphaData = new bool[terrainTexture.width, terrainTexture.height];
        for (int x = 0; x < terrainTexture.width; x++)
        {
            for (int y = 0; y < terrainTexture.height; y++)
            {
                alphaData[x, y] = terrainTexture.GetPixel(x, y).a > 0;
            }
        }

        // Tạo lại PolygonCollider2D từ dữ liệu alpha
        List<Vector2> colliderPath = new List<Vector2>();
        for (int x = 0; x < terrainTexture.width; x++)
        {
            for (int y = 0; y < terrainTexture.height; y++)
            {
                if (alphaData[x, y])
                {
                    colliderPath.Add(new Vector2(x / (float)terrainTexture.width, y / (float)terrainTexture.height));
                }
            }
        }
        terrainCollider.SetPath(0, colliderPath.ToArray());
    }
}
