using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Worldgeneration
{

    [CreateAssetMenu(menuName = "Data/Tile Data")]
    public class TileData : ScriptableObject
    {
        public List<TileDefinition> tileDefinitions;

        public TileBase[] GetTileBases(string[,] layer)
        {
            var r = new TileBase[layer.GetLength(0) * layer.GetLength(1)];
            var d = TileDefinition.GetTileDefDic(tileDefinitions);
            for (int x = 0; x < layer.GetLength(0); x++)
            {
                for (int y = 0; y < layer.GetLength(1); y++)
                {
                    if (layer[x, y].Length == 0)
                        continue;
                    r[y * layer.GetLength(1) + x] = d[layer[x, y]];
                }
            }
            return r;
        }

    }
}