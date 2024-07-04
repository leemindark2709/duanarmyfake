using UnityEngine;
using UnityEngine.UI;

public class TextureEditor : MonoBehaviour
{
    public Image image;
    public Texture2D texture;
    public Color eraseColor = new Color(0, 0, 0, 0); // Màu để xóa (màu trong suốt)
    public int brushSize = 10;

    Texture2D copyTexture;
    private RectTransform imageRectTransform;
    private Vector2 localPoint;
    private Vector3 lastMousePos;
    private Camera mainCamera;

    void Start()
    {
        // Tạo một Texture2D mới và sao chép dữ liệu từ Texture gốc
        copyTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(texture, copyTexture);

        // Tạo một Sprite từ Texture đã sao chép
        image.sprite = Sprite.Create(copyTexture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        imageRectTransform = image.rectTransform;

        // Lấy camera chính
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Kiểm tra nếu nút chuột trái được nhấn và vị trí chuột đã thay đổi
        if (Input.GetMouseButton(0) && lastMousePos != Input.mousePosition && brushSize > 0)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(imageRectTransform, Input.mousePosition, mainCamera))
            {
                lastMousePos = Input.mousePosition;

                Erase(Input.mousePosition);

            }
        }
    }

    public void Erase(Vector2 touchPosWithinRect)
    {
        Debug.LogWarning("Hole created at position: " + touchPosWithinRect);
        // Chuyển đổi tọa độ màn hình sang tọa độ cục bộ của RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(imageRectTransform, touchPosWithinRect, mainCamera, out localPoint);

        // Tính toán vị trí pixel trên Texture2D
        int px = Mathf.Clamp((int)((localPoint.x - imageRectTransform.rect.x) * copyTexture.width / imageRectTransform.rect.width), 0, copyTexture.width - 1);
        int py = Mathf.Clamp((int)((localPoint.y - imageRectTransform.rect.y) * copyTexture.height / imageRectTransform.rect.height), 0, copyTexture.height - 1);

        // Xóa các pixel trong bán kính brush


        Debug.LogWarning("Hole created at position: " + px + py);
        ErasePixelsWithinRadius(px, py, brushSize, eraseColor);
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
}