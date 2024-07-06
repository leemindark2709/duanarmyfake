using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TextureEditor : MonoBehaviour
{
    public Image image;
    public Texture2D texture;
    public Color eraseColor = new Color(0, 0, 0, 0); // Transparent color for erasing
    public int brushSize = 10;

    public Texture2D copyTexture;
    private RectTransform imageRectTransform;
    private Vector2 localPoint;
    private Vector3 lastMousePos;
    private Camera mainCamera;
    private List<PolygonCollider2D> polygonColliders = new List<PolygonCollider2D>();

    void Start()
    {
        // Create a new Texture2D and copy data from the original texture
        copyTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(texture, copyTexture);

        // Create a sprite from the copied texture
        image.sprite = Sprite.Create(copyTexture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        imageRectTransform = image.rectTransform;

        // Get the main camera
        mainCamera = Camera.main;

        // Add the initial PolygonCollider2D
        PolygonCollider2D initialCollider = gameObject.AddComponent<PolygonCollider2D>();
        polygonColliders.Add(initialCollider);

        // Initial update
        UpdateColliders();
    }

    private void Update()
    {
        // Check if the left mouse button is pressed and the mouse position has changed
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
        // Convert screen coordinates to local coordinates of RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(imageRectTransform, touchPosWithinRect, mainCamera, out localPoint);

        // Convert local coordinates to texture coordinates
        int px = Mathf.Clamp((int)((localPoint.x - imageRectTransform.rect.xMin) * copyTexture.width / imageRectTransform.rect.width), 0, copyTexture.width - 1);
        int py = Mathf.Clamp((int)((localPoint.y - imageRectTransform.rect.yMin) * copyTexture.height / imageRectTransform.rect.height), 0, copyTexture.height - 1);

        // Erase pixels within the brush radius
        ErasePixelsWithinRadius(px, py, brushSize, eraseColor);

        // Update colliders after erasing pixels
        UpdateColliders();
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

    private void UpdateColliders()
    {
        // Convert texture to bitmask to identify erased regions
        bool[,] bitmask = new bool[copyTexture.width, copyTexture.height];
        for (int y = 0; y < copyTexture.height; y++)
        {
            for (int x = 0; x < copyTexture.width; x++)
            {
                bitmask[x, y] = copyTexture.GetPixel(x, y).a > 0;
            }
        }

        // Generate clusters from the bitmask
        List<List<Vector2>> clusters = GenerateClustersFromBitmask(bitmask);

        // Clear existing colliders
        foreach (var collider in polygonColliders)
        {
            Destroy(collider);
        }
        polygonColliders.Clear();

        // Create new colliders for each cluster
        foreach (var cluster in clusters)
        {
            PolygonCollider2D newCollider = gameObject.AddComponent<PolygonCollider2D>();
            polygonColliders.Add(newCollider);
            newCollider.pathCount = 1;
            newCollider.SetPath(0, cluster.ToArray());
        }
    }

    private List<List<Vector2>> GenerateClustersFromBitmask(bool[,] bitmask)
    {
        int width = bitmask.GetLength(0);
        int height = bitmask.GetLength(1);
        bool[,] visited = new bool[width, height];
        List<List<Vector2>> clusters = new List<List<Vector2>>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (bitmask[x, y] && !visited[x, y])
                {
                    List<Vector2> cluster = new List<Vector2>();
                    FloodFill(bitmask, visited, x, y, cluster);
                    clusters.Add(GenerateOutlineFromCluster(cluster));
                }
            }
        }

        return clusters;
    }

    private void FloodFill(bool[,] bitmask, bool[,] visited, int x, int y, List<Vector2> cluster)
    {
        int width = bitmask.GetLength(0);
        int height = bitmask.GetLength(1);
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(x, y));
        visited[x, y] = true;

        while (queue.Count > 0)
        {
            Vector2Int point = queue.Dequeue();
            cluster.Add(new Vector2(point.x, point.y));

            foreach (var dir in new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) })
            {
                int nx = point.x + dir.x;
                int ny = point.y + dir.y;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height && bitmask[nx, ny] && !visited[nx, ny])
                {
                    queue.Enqueue(new Vector2Int(nx, ny));
                    visited[nx, ny] = true;
                }
            }
        }
    }

    private List<Vector2> GenerateOutlineFromCluster(List<Vector2> cluster)
    {
        // Placeholder: Convert cluster points to outline
        List<Vector2> outline = new List<Vector2>();

        foreach (var point in cluster)
        {
            // Convert point from pixel coordinates to local coordinates of RectTransform
            float localX = (point.x / (float)copyTexture.width) * imageRectTransform.rect.width - (imageRectTransform.rect.width / 2);
            float localY = (point.y / (float)copyTexture.height) * imageRectTransform.rect.height - (imageRectTransform.rect.height / 2);
            outline.Add(new Vector2(localX, localY));
        }

        // Simplify outline
        outline = SimplifyOutline(outline);

        return outline;
    }

    private List<Vector2> SimplifyOutline(List<Vector2> outlinePoints)
    {
        List<Vector2> simplifiedOutline = new List<Vector2>();
        Vector2 lastPoint = outlinePoints[0];
        simplifiedOutline.Add(lastPoint);

        for (int i = 1; i < outlinePoints.Count; i++)
        {
            if (Vector2.Distance(lastPoint, outlinePoints[i]) > 0.1f)
            {
                lastPoint = outlinePoints[i];
                simplifiedOutline.Add(lastPoint);
            }
        }

        return simplifiedOutline;
    }
}
