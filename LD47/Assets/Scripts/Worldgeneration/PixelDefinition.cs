using System.Collections.Generic;
using UnityEngine;

namespace Worldgeneration
{

    /// <summary>
    /// Definition of a color and possible sprites.
    /// </summary>
    [System.Serializable]
    public class PixelDefinition
    {
        /// <summary> Name of the pixel definition. </summary>
        public string name;
        /// <summary> Color </summary>
        public Color32 color;

        public PixelDefinition(string name, Color32 color)
        {
            this.name = name;
            this.color = color;
        }

        public static Dictionary<Color32, string> GetDictionary(List<PixelDefinition> pixelDefinitions)
        {
            var dic = new Dictionary<Color32, string>();
            pixelDefinitions.ForEach(def =>
            {
                if (!dic.ContainsKey(def.color))
                    dic.Add(def.color, def.name);
            });
            return dic;
        }

    }
}
