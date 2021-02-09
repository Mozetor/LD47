using UnityEngine;

namespace Worldgeneration {

    public static class CoordinateConverter {

        public static Vector2Int WorldToArray(Vector2 pos, Vector2Int worldSize) => new Vector2Int((int)pos.x + worldSize.x / 2, (int)pos.y + worldSize.y / 2);

        public static Vector2Int WorldToArray(Vector3 pos, Vector2Int worldSize) => new Vector2Int((int)pos.x + worldSize.x / 2, (int)pos.y + worldSize.y / 2);

        public static Vector3 ArrayToWorld(Vector2Int pos, Vector2Int worldSize) => new Vector3(pos.x - worldSize.x / 2, pos.y - worldSize.y / 2, 0);

        public static Vector3 ArrayToWorld(Vector3 pos, Vector2Int worldSize) => new Vector3(pos.x - worldSize.x / 2, pos.y - worldSize.y / 2, 0);

        public static Vector2Int Round(Vector3 pos) => new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

        public static Vector3Int ToVector3Int(Vector2Int pos) => new Vector3Int(pos.x, pos.y, 0);

        public static Vector2Int GetCurrentMouseTilePosition(Camera cam) => Round(cam.ScreenToWorldPoint(Input.mousePosition));
    }
}