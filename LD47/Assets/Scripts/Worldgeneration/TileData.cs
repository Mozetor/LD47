using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Worldgeneration {

    [CreateAssetMenu(menuName = "Data/Tile Data")]
    public class TileData : ScriptableObject {
        /// <summary> Definition of the tileset </summary>
        public List<Definition<TileBase>> tileDefinitions;

        /// <summary>
        /// Converts a type map to tile map
        /// </summary>
        /// <param name="layer">  </param>
        /// <returns></returns>
        public TileBase[] ConvertToTiles(string[,] layer) {
            var r = new TileBase[layer.GetLength(0) * layer.GetLength(1)];
            var d = tileDefinitions.ToDictionary(def => def.name, def => def.value);
            for (int x = 0; x < layer.GetLength(0); x++) {
                for (int y = 0; y < layer.GetLength(1); y++) {
                    if (layer[x, y].Length == 0)
                        continue;
                    r[y * layer.GetLength(1) + x] = d[layer[x, y]];
                }
            }
            return r;
        }

    }
}