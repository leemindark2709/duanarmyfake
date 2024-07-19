using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextureEditor : MonoBehaviour
{
    public Image image;
    public Texture2D texture;
    public Color eraseColor = new Color(0, 0, 0, 0); // Màu trong suốt để xóa
    public int brushSize = 10;

    private Texture2D copyTexture;
    private RectTransform imageRectTransform;
    private Camera mainCamera;
    private PolygonCollider2D polygonCollider;
    private bool isUpdatingCollider = false;

    void Start()
    {
        // Tạo một Texture2D mới và sao chép dữ liệu từ texture gốc
        copyTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(texture, copyTexture);

        // Tạo một sprite từ texture sao chép
        image.sprite = Sprite.Create(copyTexture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        imageRectTransform = image.rectTransform;

        // Lấy camera chính
        mainCamera = Camera.main;

        // Thêm PolygonCollider2D ban đầu
        polygonCollider = gameObject.AddComponent<PolygonCollider2D>();

        // Cập nhật ban đầu
        StartCoroutine(UpdateColliderCoroutine());
    }

    private void Update()
    {
        // Đoạn mã này có thể để kiểm tra va chạm với viên đạn và gọi hàm Erase nếu cần
    }

    public void Erase(Vector2 touchPosWithinRect)
    {
        Debug.LogWarning("Tạo lỗ tại vị trí: " + touchPosWithinRect);
        // Chuyển đổi tọa độ màn hình sang tọa độ cục bộ của RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(imageRectTransform, touchPosWithinRect, mainCamera, out Vector2 localPoint);

        // Chuyển đổi tọa độ cục bộ sang tọa độ texture
        int px = Mathf.Clamp((int)((localPoint.x - imageRectTransform.rect.xMin) * copyTexture.width / imageRectTransform.rect.width), 0, copyTexture.width - 1);
        int py = Mathf.Clamp((int)((localPoint.y - imageRectTransform.rect.yMin) * copyTexture.height / imageRectTransform.rect.height), 0, copyTexture.height - 1);

        // Xóa các pixel trong bán kính cọ
        ErasePixelsWithinRadius(px, py, brushSize, eraseColor);

        // Cập nhật collider sau khi xóa pixel
        if (!isUpdatingCollider)
        {
            StartCoroutine(UpdateColliderCoroutine());
        }
    }

    public void ErasePixelsWithinRadius(int cx, int cy, int radius, Color color)
    {
        int rSquared = radius * radius;
        for (int x = cx - radius; x <= cx + radius; x++)
        {
            for (int y = cy - radius; y <= cy + radius; y++)
            {
                int dx = x - cx;
                int dy = y - cy;
                if ((dx * dx + dy * dy) <= rSquared)
                {
                    if (x >= 0 && x < copyTexture.width && y >= 0 && y < copyTexture.height)
                    {
                        copyTexture.SetPixel(x, y, color);
                    }
                }
            }
        }
        copyTexture.Apply();
    }

    private IEnumerator UpdateColliderCoroutine()
    {
        isUpdatingCollider = true;

        // Xác định các điểm viền của vùng có màu sắc
        List<Vector2> outline = GetTextureOutline();

        // Cập nhật PolygonCollider2D
        polygonCollider.pathCount = 0;
        if (outline.Count > 0)
        {
            polygonCollider.pathCount = 1;
            polygonCollider.SetPath(0, outline.ToArray());
        }

        isUpdatingCollider = false;
        yield return null;
    }

    private List<Vector2> GetTextureOutline()
    {
        List<Vector2> outline = new List<Vector2>();

        // Tạo một bitmask từ texture để xác định vùng có màu sắc
        bool[,] bitmask = new bool[copyTexture.width, copyTexture.height];
        for (int y = 0; y < copyTexture.height; y++)
        {
            for (int x = 0; x < copyTexture.width; x++)
            {
                bitmask[x, y] = copyTexture.GetPixel(x, y).a > 0;
            }
        }

        // Xác định các điểm viền từ bitmask
        for (int y = 0; y < copyTexture.height; y++)
        {
            for (int x = 0; x < copyTexture.width; x++)
            {
                if (bitmask[x, y] && IsBorderPixel(bitmask, x, y))
                {
                    float localX = (x / (float)copyTexture.width) * imageRectTransform.rect.width - (imageRectTransform.rect.width / 2);
                    float localY = (y / (float)copyTexture.height) * imageRectTransform.rect.height - (imageRectTransform.rect.height / 2);
                    outline.Add(new Vector2(localX, localY));
                }
            }
        }

        return SimplifyOutline(outline);
    }

    private bool IsBorderPixel(bool[,] bitmask, int x, int y)
    {
        int width = bitmask.GetLength(0);
        int height = bitmask.GetLength(1);

        // Kiểm tra nếu pixel hiện tại có ít nhất một pixel xung quanh không có màu sắc
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int nx = x + i;
                int ny = y + j;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height && !(i == 0 && j == 0))
                {
                    if (!bitmask[nx, ny])
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private List<Vector2> SimplifyOutline(List<Vector2> outlinePoints)
    {
        List<Vector2> simplifiedOutline = new List<Vector2>();
        if (outlinePoints.Count == 0)
            return simplifiedOutline;

        simplifiedOutline.Add(outlinePoints[0]);

        for (int i = 1; i < outlinePoints.Count; i++)
        {
            if (Vector2.Distance(simplifiedOutline[simplifiedOutline.Count - 1], outlinePoints[i]) > 0.1f)
            {
                simplifiedOutline.Add(outlinePoints[i]);
            }
        }

        return simplifiedOutline;
    }
}
