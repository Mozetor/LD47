using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Worldgeneration {

    public class WorldController : MonoBehaviour {

        [Tooltip("The prefab of a single tile")]
        public GameObject tilePrefab;
        [Tooltip("The world data to generate world with")]
        public WorldData currentWorld;
        /// <summary> Dictionary of pixel definitions of current world. </summary>
        private Dictionary<Color, (List<Sprite> sprites, PixelType type)> dic;

        private void Start() {
            GenerateWorld();
        }

        /// <summary>
        /// Whether the tile at a given position is a path.
        /// </summary>
        /// <param name="x"> X position of tile </param>
        /// <param name="y"> Y position of tile </param>
        /// <returns></returns>
        public bool IsPath(int x, int y) {
            return GetPixelType(x,y) == PixelType.Path;
        }
        /// <summary>
        /// Returns the pixel type at a given position.
        /// </summary>
        /// <param name="x"> X position of tile </param>
        /// <param name="y"> Y position of tile </param>
        /// <returns></returns>
        public PixelType GetPixelType(int x, int y) {
            return dic[currentWorld.textures[0].GetPixel(x, y)].type;
        }


        /// <summary>
        /// Generate the world from textures.
        /// </summary>
        private void GenerateWorld() {
            dic = currentWorld.GetPixelDefDictionary();
            int layer = -10;
            foreach (var tex in currentWorld.textures) {
                GenerateSlice(tex, layer);
                layer++;
            }
        }
        /// <summary>
        /// Generates all sprites for a single texture.
        /// </summary>
        /// <param name="texture"> Texture of the map </param>
        /// <param name="layer"> Order in layer </param>
        private void GenerateSlice(Texture2D texture, int layer) {
            for (int x = 0; x < texture.width; x++) {
                for (int y = 0; y < texture.height; y++) {
                    SpawnPixel(texture.GetPixel(x, y), new Vector3(x, y), layer);
                }
            }
        }
        /// <summary>
        /// Spawns a sprite for a pixel.
        /// </summary>
        /// <param name="color"> Color of pixel </param>
        /// <param name="pos"> Position of pixel </param>
        /// <param name="layer"> Order in layer </param>
        private void SpawnPixel(Color color, Vector3 pos, int layer) {
            var rend = Instantiate(tilePrefab, pos, Quaternion.identity, transform).GetComponent<SpriteRenderer>();
            if (!dic.ContainsKey(color)) {
                Debug.LogWarning($"Doesn't contain pixel definition to color {color}");
                return;
            }
            var sprites = dic[color].sprites;
            if (sprites.Count == 0) {
                throw new ArgumentException("There exists no sprite with existing color!");
            }
            int i = Random.Range(0, sprites.Count);
            rend.sprite = sprites[i];
            rend.sortingOrder = layer;
        }
    }
}