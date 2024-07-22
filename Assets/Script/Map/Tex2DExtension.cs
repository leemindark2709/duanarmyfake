using UnityEngine;

public static class Tex2DExtension
{
    public static void ErasePixelsWithinRadius(this Texture2D tex, int x, int y, int r, Color color)
    {
        int rSquared = r * r;

        int xMin = Mathf.Max(0, x - r);
        int xMax = Mathf.Min(tex.width, x + r);
        int yMin = Mathf.Max(0, y - r);
        int yMax = Mathf.Min(tex.height, y + r);

        for (int u = xMin; u < xMax; u++)
        {
            for (int v = yMin; v < yMax; v++)
            {
                if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
                {
                    tex.SetPixel(u, v, color);
                }
            }
        }

        tex.Apply(false, false);
    }
}
