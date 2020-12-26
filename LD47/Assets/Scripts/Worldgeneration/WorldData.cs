using System;
using System.Collections.Generic;
using UnityEngine;

namespace Worldgeneration
{

    [CreateAssetMenu(menuName = "Data/World Data")]
    public class WorldData : ScriptableObject
    {

        [Tooltip("Width and height of the world")]
        public Vector2Int worldSize;
        public List<WorldLayer> worldLayers = new List<WorldLayer>();
        [Tooltip("The sprites that make the world.")]
        public List<PixelDefinition> pixelDefinitions = new List<PixelDefinition>();

        public Action OnValidation;

        private List<string[,]> worldPixelTypes;
        public List<string[,]> WorldPixelTypes => worldPixelTypes;

        public int Layers => worldLayers.Count;

        private Dictionary<Color32, string> pixelDefDic;

        private void OnEnable()
        {
            pixelDefDic = PixelDefinition.GetDictionary(pixelDefinitions);
            worldPixelTypes = GetPixelTypes();
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
                    if (!pixelDefDic.ContainsKey(pixel))
                        throw new NotSupportedException($"Pixel of color {worldLayers[layer].texture.GetPixel(x, y)} was not found in dictionary!");
                    pixelTypes[x, y] = pixelDefDic[pixel];
                }
            }

            return pixelTypes;
        }

    }
}