//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class TilemapCollisionHandler : MonoBehaviour
//{
//    public Tilemap tilemap;

//    void Start()
//    {
//         Nếu tilemap chưa được gán trong inspector, lấy thành phần Tilemap được gắn vào GameObject hiện tại
//        if (tilemap == null)
//        {
//            tilemap = GetComponent<Tilemap>();
//        }
//    }

//     Phương thức để xử lý va chạm cho các đối tượng không kích hoạt trigger
//    void OnCollisionEnter2D(Collision2D collision)
//    {
//         Lặp qua từng điểm tiếp xúc trong va chạm
//        foreach (ContactPoint2D contact in collision.contacts)
//        {
//            if (collision.transform.name.Equals("Player"))
//            {
//               return ;
//            }
//             Xác định vị trí va chạm trong không gian thế giới
//            Vector3 hitPosition = Vector3.zero;
//            hitPosition.x = contact.point.x - 0.01f * contact.normal.x;
//            hitPosition.y = contact.point.y - 0.01f * contact.normal.y;

//             Chuyển đổi vị trí va chạm từ không gian thế giới sang tọa độ ô tile
//            Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);
//            if (tilemap.HasTile(tilePosition))
//            {
//                Debug.Log("Tile va chạm tại: " + tilePosition);
//                 Xóa ô tile tại vị trí va chạm
//                tilemap.SetTile(tilePosition, null);
//            }
//        }
//    }

//     Phương thức để xử lý va chạm cho các đối tượng kích hoạt trigger
//    void OnTriggerEnter2D(Collider2D other)
//    {
//         Lấy vị trí va chạm của collider khác trong không gian thế giới
//        Vector3 hitPosition = other.transform.position;

//         Chuyển đổi vị trí va chạm từ không gian thế giới sang tọa độ ô tile
//        Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);
//        if (tilemap.HasTile(tilePosition))
//        {
//            Debug.Log("Tile va chạm tại: " + tilePosition);
//             Xóa ô tile tại vị trí va chạm
//            tilemap.SetTile(tilePosition, null);
//        }
//    }
//}
