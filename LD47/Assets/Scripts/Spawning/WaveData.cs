using UnityEngine;

namespace Assets.WaveSpawner {
    /// <summary>
    /// This is the scriptable object containing the waves.
    /// </summary>
    [CreateAssetMenu(menuName = "Enemy Spawning/Wave Data")]
    public class WaveData : ScriptableObject {
        /// <summary> The waves held in this data object. </summary>
        [Tooltip("The waves spawned by the Wave Spawner.")]
        public Wave[] waves;
    }
}