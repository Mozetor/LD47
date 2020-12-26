using UnityEngine;

namespace Worldgeneration
{
    [System.Serializable]
    public class WorldLayer
    {
        public Texture2D texture;
        [NavMeshSelector]
        public int NavMeshSelector;
    }
}