using System.Collections.Generic;
using UnityEngine;

namespace Worldgeneration {

    /// <summary>
    /// Definition of a color and possible sprites.
    /// </summary>
    [System.Serializable]
    public class PixelDefinition {
        /// <summary> Color </summary>
        public Color color;
        /// <summary> Pixel type </summary>
        public PixelType type;
        /// <summary> List of possible sprites. </summary>
        public List<Sprite> sprites = new List<Sprite>();
    }
}