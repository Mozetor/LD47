using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Worldgeneration
{

    [CreateAssetMenu(menuName = "Data/World Data")]
    public class WorldData : ScriptableObject
    {
        /// <summary> Size of the world </summary>
        public Vector2Int worldSize;
        /// <summary> World Layers </summary>
        public List<WorldLayer> worldLayers = new List<WorldLayer>();
        /// <summary> Tiles used for each pixel type </summary>
        public List<Definition<Color32>> pixelDefinitions = new List<Definition<Color32>>();
        /// <summary> Used for validation on data change. </summary>
        public Action OnValidation;
        /// <summary> Types of each world layer. </summary>
        public List<string[,]> WorldPixelTypes { get; private set; }
        /// <summary> Number of Layers in this world. </summary>
        public int Layers => worldLayers.Count;

        private Dictionary<Color32, string> pixelDefDic;

        private void OnEnable()
        {
            pixelDefDic = pixelDefinitions.ToDictionary(def => def.value, def => def.name);
            WorldPixelTypes = GetPixelTypes();
        }

        private void OnValidate()
        {
            OnValidation?.Invoke();
        }

        /// <summary>
        /// Calculate all sprite name layers.
        /// </summary>
        /// <returns> List of all layers sprite names </returns>
        private List<string[,]> GetPixelTypes()
        {
            var r = new List<string[,]>();
            for (int i = 0; i < Layers; i++)
            {
                r.Add(GetPixelTypes(i));
            }
            return r;
        }
        /// <summary>
        /// Calculate all sprite names for a specific layer.
        /// </summary>
        /// <param name="layer"> Layer of this world data </param>
        /// <returns> All sprite names of this layer </returns>
        private string[,] GetPixelTypes(int layer)
        {
            string[,] pixelTypes = new string[worldSize.x, worldSize.y];

            for (int y = 0; y < worldSize.y; y++)
            {
                for (int x = 0; x < worldSize.x; x++)
                {
                    var pixel = worldLayers[layer].texture.GetPixel(x, y);
                    if (pixel.a == 0)
                    {
                        pixelTypes[x, y] = "";
                        continue;
                    }
#if UNITY_EDITOR
                    if (!pixelDefDic.ContainsKey(pixel))
                        throw new NotSupportedException($"Pixel of color {worldLayers[layer].texture.GetPixel(x, y)} was not found in dictionary!");
#endif
                    pixelTypes[x, y] = pixelDefDic[pixel];
                }
            }

            return pixelTypes;
        }

    }
}