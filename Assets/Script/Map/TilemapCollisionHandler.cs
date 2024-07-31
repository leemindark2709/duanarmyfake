using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCollisionHandler : MonoBehaviour
{
    public Tilemap tilemap;

    void Start()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //Lấy vị trí va chạm của collider khác trong không gian thế giới
       Vector3 hitPosition = other.transform.position;

        //Chuyển đổi vị trí va chạm từ không gian thế giới sang tọa độ ô tile
        Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);
        if (tilemap.HasTile(tilePosition))
        {
            Debug.Log("Tile va chạm tại: " + tilePosition);
            //Xóa ô tile tại vị trí va chạm
            tilemap.SetTile(tilePosition, null);
        }
    }
}
