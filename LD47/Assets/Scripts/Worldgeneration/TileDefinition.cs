using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

namespace Worldgeneration
{

    [System.Serializable]
    public class TileDefinition
    {
        /// <summary> Name </summary>
        public string name;
        /// <summary> Tile </summary>
        public TileBase tile;


        public static Dictionary<string, TileBase> GetTileDefDic(List<TileDefinition> tileDefinitions)
        {
            var dic = new Dictionary<string, TileBase>();
            tileDefinitions.ForEach(def => dic.Add(def.name, def.tile));
            return dic;
        }
    }
}