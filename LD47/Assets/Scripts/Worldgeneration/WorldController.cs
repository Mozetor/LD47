using System.Collections.Generic;
using System.Diagnostics;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;
using static Utils.ColorUtils;
using Assets.Enemies.Behaviour;
using System.Collections;
using Assets.WaveSpawner;
using Assets.WaveSpawner.Implementation;

namespace Worldgeneration {

    public class WorldController : MonoBehaviour {

        [Tooltip("The prefab of a single tile")]
        public GameObject tilePrefab;
        [Tooltip("The world data to generate world with")]
        public WorldData currentWorld;
        [Tooltip("Path data reference")]
        public Paths paths;
        [Tooltip("Wave data reference")]
        public WaveData waveData;
        [Header("Debug")]
        [Tooltip("Whether to draw the calculated paths")]
        public bool renderPaths;
        [Tooltip("Whether to measure the time needed to generate world")]
        public bool measureWorldGenerationTime;

        /// <summary> Dictionary of pixel definitions of current world. </summary>
        private Dictionary<Color, PixelType> colorTypeDic;
        /// <summary> Dictionary of type to sprites. </summary>
        private Dictionary<PixelType, List<Sprite>> typeSpritesDic;
        /// <summary> List of all layer types. </summary>
        private List<PixelType[,]> pixelTypes;


        private void Start() {
            var stopwatch = Stopwatch.StartNew();
            if (measureWorldGenerationTime) {
                stopwatch.Start();
            }
            GenerateWorld();
            if (measureWorldGenerationTime) {
                stopwatch.Stop();
                Debug.Log($"Time: {stopwatch.ElapsedMilliseconds}ms");
            }

            paths.paths = CalculatePaths();
            if (renderPaths) {
                ShowPaths();
            }

            var spawns = FindObjectOfType<BuildBattleSpawner>().spawnPoints;
            spawns.Clear();
            paths.paths.ForEach((p) => spawns.Add(p.pathPoints[0]));
        }

        /// <summary>
        /// Whether the tile at a given position is a path.
        /// </summary>
        /// <param name="x"> X position of tile </param>
        /// <param name="y"> Y position of tile </param>
        /// <returns> Whether tile is path </returns>
        public bool IsPath(int x, int y) {
            return GetPixelType(x, y) != PixelType.Grass;
        }
        /// <summary>
        /// Returns the pixel type at a given position.
        /// </summary>
        /// <param name="x"> X position of tile </param>
        /// <param name="y"> Y position of tile </param>
        /// <returns> Type of the tile on lowest layer </returns>
        public PixelType GetPixelType(int x, int y) {
            var p = WorldToArrayPosition(x, y);
            return pixelTypes[0][p.x, p.y];
        }
        /// <summary>
        /// Calculate all paths of the current world.
        /// </summary>
        /// <returns> All paths </returns>
        public List<Path> CalculatePaths() {
            var paths = new List<Path>();

            var starts = CalculatePathStarts();
            foreach (var start in starts) {
                paths.Add(CalculatePath(start));
            }

            return paths;
        }
        /// <summary>
        /// Convertes a world position to a array position.
        /// </summary>
        /// <param name="x"> World position X </param>
        /// <param name="y"> World position Y </param>
        /// <returns></returns>
        public (int x, int y) WorldToArrayPosition(int x, int y) {
            return (x + currentWorld.worldSize.x / 2 - 1, y + currentWorld.worldSize.y / 2 - 1);
        }

        /// <summary>
        /// Generate the world from textures.
        /// </summary>
        private void GenerateWorld() {
            typeSpritesDic = currentWorld.GetTypeSpriteDictionary();
            colorTypeDic = currentWorld.GetPixelDefDictionary();
            pixelTypes = new List<PixelType[,]>();

            // Calculate the types for all layers.
            foreach (var tex in currentWorld.textures) {
                GenerateTypeArray(tex);
            }

            // Spawn all tile layers based on type.
            for (int i = 0; i < pixelTypes.Count; i++) {
                SpawnLayer(pixelTypes[i], i);
            }
        }
        /// <summary>
        /// Calculate all pixel types of a layer.
        /// </summary>
        /// <param name="texture"> Texture to generate types from </param>
        private void GenerateTypeArray(Texture2D texture) {
            var t = new PixelType[currentWorld.worldSize.x, currentWorld.worldSize.y];
            for (int x = 0; x < currentWorld.worldSize.x; x++) {
                for (int y = 0; y < currentWorld.worldSize.y; y++) {
                    if (!colorTypeDic.ContainsKey(ToNiceColor(texture.GetPixel(x, y)))) {
                        Debug.LogError("Texture '" + texture.name + "' contains color: " + ToNiceColor(texture.GetPixel(x, y)) + " at pos: " + (x, y));
                        continue;
                    }
                    t[x, y] = colorTypeDic[ToNiceColor(texture.GetPixel(x, y))];
                }
            }
            pixelTypes.Add(t);
        }
        /// <summary>
        /// Spawns a complete layer of tiles.
        /// </summary>
        /// <param name="types"> Tile types of the layer </param>
        /// <param name="layer"> Layer number </param>
        private void SpawnLayer(PixelType[,] types, int layer) {
            for (int x = 0; x < currentWorld.worldSize.x; x++) {
                for (int y = 0; y < currentWorld.worldSize.y; y++) {
                    var p = new Vector3Int(x - currentWorld.worldSize.x / 2 + 1, y - currentWorld.worldSize.y / 2 + 1, 0);
                    SpawnPixel(types[x, y], p, layer);
                }
            }
        }
        /// <summary>
        /// Spawns a new tile.
        /// </summary>
        /// <param name="type"> Type of the tile </param>
        /// <param name="pos"> Position of the tile </param>
        /// <param name="layer"> Layer number </param>
        private void SpawnPixel(PixelType type, Vector3Int pos, int layer) {
            InstantiateRandomTile(typeSpritesDic[type], pos, layer);
        }

        private void InstantiateRandomTile(List<Sprite> sprites, Vector3 pos, int layer) {
            if(sprites.Count == 0) {
                return;
            }
            int i = Random.Range(0, sprites.Count);
            InstantiateTile(sprites[i], pos, layer);
        }

        private void InstantiateTile(Sprite sprite, Vector3 pos, int layer) {
            var rend = Instantiate(tilePrefab, pos, Quaternion.identity, transform).GetComponent<SpriteRenderer>();
            rend.sprite = sprite;
            rend.sortingOrder = layer - 10;
        }

        private List<Vector3Int> CalculatePathStarts() {
            var l = new List<Vector3Int>();
            for (int x = 0; x < currentWorld.worldSize.x; x++) {
                for (int y = 0; y < currentWorld.worldSize.y; y++) {
                    if (x == 0 || x == currentWorld.worldSize.x - 1 || y == 0 || y == currentWorld.worldSize.y - 1) {
                        if (pixelTypes[0][x, y] == PixelType.Path) {
                            l.Add(new Vector3Int(x, y, 0));
                        }
                    }
                }
            }
            return l;
        }

        private Path CalculatePath(Vector3Int pathStart) {
            Path path = new Path();
            path.pathPoints = new List<Vector3>();
            SearchPath(path, pathStart);
            path.pathPoints.Add(new Vector3(.5f, .5f));
            return path;
        }

        private void SearchPath(Path path, Vector3Int p) {
            var pn = new Vector3Int(p.x - currentWorld.worldSize.x / 2 + 1, p.y - currentWorld.worldSize.y / 2 + 1, 0);
            if (p.x < 0 || p.y < 0 || p.x >= currentWorld.worldSize.x || p.y >= currentWorld.worldSize.y
                || path.pathPoints.Contains(pn) || pixelTypes[0][p.x, p.y] != PixelType.Path) {
                return;
            }
            path.pathPoints.Add(pn);
            if (pn.x > -2 && pn.y > -2 && pn.x < 3 && pn.y < 3) {
                return;
            }
            SearchPath(path, new Vector3Int(p.x + 1, p.y, 0));
            SearchPath(path, new Vector3Int(p.x - 1, p.y, 0));
            SearchPath(path, new Vector3Int(p.x, p.y + 1, 0));
            SearchPath(path, new Vector3Int(p.x, p.y - 1, 0));
        }
        private void ShowPaths() {
            if (paths.paths.Count == 0) {
                return;
            }
            foreach (var path in paths.paths) {
                StartCoroutine(PathShower(path));
            }
        }

        IEnumerator PathShower(Path path) {
            for (int i = 0; i < path.pathPoints.Count - 1; i++) {
                Debug.DrawLine(path.pathPoints[i], path.pathPoints[i + 1], Color.black, 600);
                yield return new WaitForSecondsRealtime(.1f);
            }
        }
    }
}