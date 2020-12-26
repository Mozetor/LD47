using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

namespace Worldgeneration
{

    public class TileMapGenerator : MonoBehaviour
    {
        [Tooltip("Grid of the scene, can be empty.")]
        public Grid grid;
        public GameObject tilemapPrefab;

        [Header("Data")]

        public TileData tileData;
        public WorldData worldData;

        private List<Tilemap> tilemaps;
        private Vector3Int[] coords;


        private void Awake()
        {
            tilemaps = new List<Tilemap>();
            grid = FindObjectOfType<Grid>();
            if (grid == null)
            {
                grid = new GameObject("World Grid").AddComponent<Grid>();
            }
            coords = new Vector3Int[worldData.worldSize.x * worldData.worldSize.y];
            var offset = new Vector3Int(worldData.worldSize.x / 2, worldData.worldSize.y / 2, 0);
            for (int x = 0; x < worldData.worldSize.x; x++)
            {
                for (int y = 0; y < worldData.worldSize.y; y++)
                {
                    coords[y * worldData.worldSize.y + x] = new Vector3Int(x, y, 0) - offset;
                }
            }
        }

        private void Start()
        {
            if(worldData.worldLayers.Count == 0)
                throw new System.ArgumentOutOfRangeException("World data must have at least one layer!");
            
            GenerateTilemaps();
            GetComponent<NavMeshSurface2d>().BuildNavMesh();
        }

        private void GenerateTilemaps()
        {
            List<string[,]> worldPixelTypes = worldData.WorldPixelTypes;
            for (int i = 0; i < worldPixelTypes.Count; i++)
            {
                var tilemap = Instantiate(tilemapPrefab, grid.transform).GetComponent<Tilemap>();
                tilemap.GetComponent<NavMeshModifier>().area = worldData.worldLayers[i].NavMeshSelector;
                tilemap.GetComponent<TilemapRenderer>().sortingOrder = i;
                tilemaps.Add(tilemap);
                tilemap.SetTiles(coords, tileData.GetTileBases(worldPixelTypes[i]));
            }
        }
    }
}