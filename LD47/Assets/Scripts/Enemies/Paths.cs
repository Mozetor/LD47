using System.Collections.Generic;
using UnityEngine;

namespace Enemies {
    [CreateAssetMenu(menuName = "Enemy Spawning/Paths")]
    public class Paths : ScriptableObject {
        public List<Path> paths;
    }
}