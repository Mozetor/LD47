using System.Collections.Generic;
using System.Diagnostics;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;
using static Utils.ColorUtils;


namespace Worldgeneration {

    public class WorldController : MonoBehaviour {

        [Tooltip("The prefab of a single tile")]
        public GameObject tilePrefab;
        [Tooltip("The world data to generate world with")]
        public WorldData currentWorld;

        /// <summary> Dictionary of pixel definitions of current world. </summary>
        private Dictionary<Color, PixelType> dic;
        /// <summary> Dictionary of type to sprites. </summary>
        private Dictionary<PixelType, List<Sprite>> typeSpritesDic;
        /// <summary> List of all layer types. </summary>
        private List<PixelType[,]> pixelTypes;

        // Measure time needed to generate the world in ms.
        private void Start() {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            GenerateWorld();
            stopwatch.Stop();
            //Debug.Log($"Time: {stopwatch.ElapsedMilliseconds}ms");
        }


        /// <summary>
        /// Whether the tile at a given position is a path.
        /// </summary>
        /// <param name="x"> X position of tile </param>
        /// <param name="y"> Y position of tile </param>
        /// <returns> Whether tile is path </returns>
        public bool IsPath(int x, int y) {
            return GetPixelType(x, y) == PixelType.Path;
        }
        /// <summary>
        /// Returns the pixel type at a given position.
        /// </summary>
        /// <param name="x"> X position of tile </param>
        /// <param name="y"> Y position of tile </param>
        /// <returns> Type of the tile on lowest layer </returns>
        public PixelType GetPixelType(int x, int y) {
            return pixelTypes[0][x, y];
        }


        /// <summary>
        /// Generate the world from textures.
        /// </summary>
        private void GenerateWorld() {
            typeSpritesDic = currentWorld.GetTypeSpriteDictionary();
            dic = currentWorld.GetPixelDefDictionary();
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
                    t[x, y] = dic[ToNiceColor(texture.GetPixel(x, y))];
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
                    var p = new Vector3Int(x - currentWorld.worldSize.x / 2+1, y - currentWorld.worldSize.y / 2+1, 0);
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
            switch (type) {
                case PixelType.Empty:
                    return;
                case PixelType.Grass:
                case PixelType.Path:
                    InstantiateRandomTile(typeSpritesDic[type], pos, layer);
                    break;
                case PixelType.GrassDirt0:
                case PixelType.GrassDirt1:
                case PixelType.GrassDirt2:
                case PixelType.GrassDirt3:
                case PixelType.GrassDirt4:
                case PixelType.GrassDirt5:
                case PixelType.GrassDirt6:
                case PixelType.GrassDirt7:
                case PixelType.GrassDirt8:
                case PixelType.GrassDirt9:
                case PixelType.GrassDirt10:
                case PixelType.GrassDirt11:
                    InstantiateTile(typeSpritesDic[type][0], pos, layer);
                    break;
                default:
                    throw new NotImplementedException("Type not implemented!");
            }
        }

        private void InstantiateRandomTile(List<Sprite> sprites, Vector3 pos, int layer) {
            int i = Random.Range(0, sprites.Count);
            InstantiateTile(sprites[i], pos, layer);
        }

        private void InstantiateTile(Sprite sprite, Vector3 pos, int layer) {
            var rend = Instantiate(tilePrefab, pos, Quaternion.identity, transform).GetComponent<SpriteRenderer>();
            rend.sprite = sprite;
            rend.sortingOrder = layer - 10;
        }
    }
}