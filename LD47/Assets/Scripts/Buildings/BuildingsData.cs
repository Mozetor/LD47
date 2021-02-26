using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Worldgeneration;

namespace Buildings {

    public class BuildingsData<T> where T : class, IPlaceable<T> {

        private readonly bool[,] terrainPlaceability;
        private readonly T[,] buildings;
        private readonly WorldData worldData;

        public BuildingsData(WorldData worldData, Func<bool[,]> terrainPlacibilitySupplier) {
            this.worldData = worldData;
            terrainPlaceability = terrainPlacibilitySupplier();
            buildings = new T[worldData.worldSize.x, worldData.worldSize.y];
        }

        public void DrawGizmos() {
            for (int x = 0; x < worldData.worldSize.x; x++) {
                for (int y = 0; y < worldData.worldSize.y; y++) {
                    var name = buildings[x, y] is null ? "" : buildings[x, y].GetName();
                    var color = IsPlaceable(x, y) ? Color.green : Color.yellow;
                    color = IsEmpty(x, y) ? color : Color.red;
                    DrawQuad(new Vector3(x - (worldData.worldSize.x / 2), y - (worldData.worldSize.y / 2)), color, name);
                }
            }
        }

        private void DrawQuad(Vector3 pos, Color32 color, string text = "") {
            Debug.DrawLine(pos, pos + new Vector3(0, 1), color);
            Debug.DrawLine(pos, pos + new Vector3(1, 0), color);
            Debug.DrawLine(pos + new Vector3(1, 0), pos + new Vector3(1, 1), color);
            Debug.DrawLine(pos + new Vector3(0, 1), pos + new Vector3(1, 1), color);
            Handles.Label(pos, text);
        }

        public bool InMap(Vector2Int pos) => pos.x >= 0 && pos.y >= 0 && pos.x < worldData.worldSize.x && pos.y < worldData.worldSize.y;

        public T GetBuilding(Vector2Int pos) => buildings[pos.x, pos.y];

        public bool IsEmpty(int x, int y) => buildings[x, y] is null;

        public bool IsEmpty(Vector2Int pos) => buildings[pos.x, pos.y] is null;

        public bool IsEmpty(List<Vector2Int> poss) => poss.TrueForAll(IsEmpty);

        public bool IsEmpty(IPlaceable<T> building, Vector2Int pos) => IsEmpty(CalculatePositionWithOffset(building.GetOffsetPositions(), pos));

        public bool IsPlaceable(int x, int y) => terrainPlaceability[x, y] && IsEmpty(x, y);

        public bool IsPlaceable(Vector2Int pos) => terrainPlaceability[pos.x, pos.y] && IsEmpty(pos);

        public bool IsPlaceable(List<Vector2Int> poss) => poss.TrueForAll(IsPlaceable);

        public bool IsPlaceable(IPlaceable<T> building, Vector2Int pos) => IsPlaceable(CalculatePositionWithOffset(building.GetOffsetPositions(), pos));

        public List<Vector2Int> CalculatePositionWithOffset(List<Vector2Int> poss, Vector2Int pos) {
            for (int i = 0; i < poss.Count; i++) {
                poss[i] = CoordinateConverter.WorldToArray(poss[i] + pos, worldData.worldSize);
            }
            return poss;
        }

        public bool TrySet(IPlaceable<T> placeable, Vector2Int worldPos) {
            List<Vector2Int> positions = CalculatePositionWithOffset(placeable.GetOffsetPositions(), worldPos);
            if (!IsPlaceable(positions)) {
                return false;
            }
            foreach (var position in positions) {
                buildings[position.x, position.y] = placeable.GetT();
            }
            return true;
        }

        public void Remove(IPlaceable<T> placeable) {
            var t = placeable.GetT();
            for (int x = 0; x < buildings.GetLength(0); x++) {
                for (int y = 0; y < buildings.GetLength(1); y++) {
                    if (buildings[x, y] != null && buildings[x, y].Equals(t)) {
                        buildings[x, y] = null;
                    }
                }
            }
        }

    }
}