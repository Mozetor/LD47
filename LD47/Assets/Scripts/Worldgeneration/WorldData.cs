using System.Collections.Generic;
using UnityEngine;
using static Utils.ColorUtils;

namespace Worldgeneration {

    [CreateAssetMenu(menuName = "Data/World Data")]
    public class WorldData : ScriptableObject {

        [Tooltip("Width and height of the world")]
        public Vector2Int worldSize;
        [Tooltip("The textures from that the world is generated.")]
        public List<Texture2D> textures = new List<Texture2D>();
        [Tooltip("The sprites that make the world.")]
        public List<PixelDefinition> pixelDefinitions = new List<PixelDefinition>();

        public Dictionary<Color, PixelType> GetPixelDefDictionary() {
            var dic = new Dictionary<Color, PixelType>();

            foreach (var pixelDef in pixelDefinitions) {
                dic.Add(ToNiceColor(pixelDef.color), pixelDef.type);
            }

            return dic;
        }

        public Dictionary<PixelType, List<Sprite>> GetTypeSpriteDictionary() {
            var dic = new Dictionary<PixelType, List<Sprite>>();

            foreach (var pixelDef in pixelDefinitions) {
                dic.Add(pixelDef.type, pixelDef.sprites);
            }

            return dic;
        }

    }
}