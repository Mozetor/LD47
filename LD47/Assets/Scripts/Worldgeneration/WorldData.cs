using System.Collections.Generic;
using UnityEngine;

namespace Worldgeneration {

    [CreateAssetMenu(menuName = "Data/World Data")]
    public class WorldData : ScriptableObject {

        [Tooltip("The textures from that the world is generated.")]
        public List<Texture2D> textures = new List<Texture2D>();
        [Tooltip("The sprites that make the world.")]
        public List<PixelDefinition> pixelDefinitions = new List<PixelDefinition>();

        public Dictionary<Color, (List<Sprite> sprites, PixelType type)> GetPixelDefDictionary() {
            var dic = new Dictionary<Color, (List<Sprite> sprites, PixelType type)>();

            foreach (var pixelDef in pixelDefinitions) {
                dic.Add(pixelDef.color, (pixelDef.sprites, pixelDef.type));
            }

            return dic;
        }

    }
}