using System;
using System.Linq;
using UnityEngine;

namespace Assets.WaveSpawner {
    /// <summary>
    /// This is the data class holding the information for a wave to be spawned.
    /// </summary>
    [Serializable]
    public struct Wave {
        /// <summary> The name of the wave used to identify the wave easier. </summary>
        [Tooltip("The name of the wave used to identify the wave easier.")]
        public string name;
        /// <summary> The groups of entities spawned in the wave. </summary>
        [Tooltip("The groups of entities spawned in the wave.")]
        public SpawnGroup[] spawnGroups;
        /// <summary> The minimum of simultaneous entities spawned. (Only used if more enities are to be spawned.) </summary>
        [Tooltip("The minimum of simultaneous enities spawned. (Only used if more enities are to be spawned.)")]
        public int minSimultaneousSpawns;
        /// <summary> The maximum of simultaneous entities spawned.</summary>
        [Tooltip("The maximum of simultaneous enities spawned.")]
        public int maxSimultaneousSpawns;
        /// <summary> The time between the different spawns. </summary>
        [Tooltip("The time between the different spawns.")]
        public float timeBetweenSpawns;
        /// <summary> The time to the first spawn of the wave. </summary>
        [Tooltip("The time to the first spawn of the wave.")]
        public float spawnDelay;

        /// <summary>
        /// Creates a new Wave with the given values.
        /// </summary>
        /// <param name="name"> The name of the wave. </param>
        /// <param name="spawnGroups"> The groups of enemies to be spawned. </param>
        /// <param name="minSimultaneousSpawns"> The minimum of simultaneous spawns. </param>
        /// <param name="maxSimultaneousSpawns"> The maximum of simultaneous spawns. </param>
        /// <param name="timeBetweenSpawns"> The time between spawns. </param>
        /// <param name="spawnDelay"> The time to the first spawn of the wave. </param>
        public Wave(string name, SpawnGroup[] spawnGroups, int minSimultaneousSpawns, int maxSimultaneousSpawns, float timeBetweenSpawns, float spawnDelay) {
            this.name = name;
            this.spawnGroups = spawnGroups;
            this.minSimultaneousSpawns = minSimultaneousSpawns;
            this.maxSimultaneousSpawns = maxSimultaneousSpawns;
            this.timeBetweenSpawns = timeBetweenSpawns;
            this.spawnDelay = spawnDelay;
        }

        /// <summary>
        /// Scales the wave by the given factor.
        /// </summary>
        /// <param name="wave"> The wave to be scaled. </param>
        /// <param name="factor"> The factor by which the wave is scaled. </param>
        /// <returns> The scaled wave. </returns>
        public static Wave operator *(Wave wave, int factor) {
            var name = string.Format("{0} (Scale: {1})", wave.name, factor);
            var groups = wave.spawnGroups.Select(group => group * factor).ToArray();
            int newMin = wave.minSimultaneousSpawns * factor;
            int newMax = wave.maxSimultaneousSpawns * factor;
            return new Wave(name, groups, newMin, newMax, wave.timeBetweenSpawns, wave.spawnDelay);
        }

        /// <summary>
        /// Scales the wave by the given factor.
        /// </summary>
        /// <param name="factor"> The factor by which the wave is scaled. </param>
        /// <param name="wave"> The wave to be scaled. </param>
        /// <returns> The scaled wave. </returns>
        public static Wave operator *(int factor, Wave wave) => wave * factor;
    }
}