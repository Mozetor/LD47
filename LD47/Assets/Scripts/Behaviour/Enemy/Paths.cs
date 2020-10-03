using System.Collections.Generic;
using UnityEngine;

namespace Assets.Enemies.Behaviour {
    [CreateAssetMenu(menuName = "Enemy Spawning/Paths")]
    public class Paths : ScriptableObject {
        public List<Path> paths;
    }
}