using System;

namespace Assets.WaveSpawner {
    /// <summary>
    /// This is the class holding the information about a group of the same spawn type.
    /// </summary>
    [Serializable]
    public class SpawnGroup {
        /// <summary> The name of the group used to identify the group easier. </summary>
        public string name;
        /// <summary> The object representing the group. </summary>
        public Spawnable spawnable;
        /// <summary> The number of enemies in the group. </summary>
        public int amount;

        /// <summary>
        /// Creates a new SpawnGroup with the given values.
        /// </summary>
        /// <param name="name"> The name of the group. </param>
        /// <param name="spawnable"> The object representing the group. </param>
        /// <param name="amount"> The amount of spawned objects in the group. </param>
        public SpawnGroup(string name, Spawnable spawnable, int amount) {
            this.name = name;
            this.spawnable = spawnable;
            this.amount = amount;
        }

        /// <summary>
        /// Scales the group by the given factor.
        /// </summary>
        /// <param name="group"> The group to be scaled. </param>
        /// <param name="factor"> The factou by which the group is scaled. </param>
        /// <returns> The scaled group. </returns>
        public static SpawnGroup operator *(SpawnGroup group, int factor) {
            var name = string.Format("{0} (Scale: {1})", group.name, factor);
            return new SpawnGroup(name, group.spawnable, group.amount * factor);
        }

        /// <summary>
        /// Scales the group by the given factor.
        /// </summary>
        /// <param name="factor"> The factou by which the group is scaled. </param>
        /// <param name="group"> The group to be scaled. </param>
        /// <returns> The scaled group. </returns>
        public static SpawnGroup operator *(int factor, SpawnGroup group) => group * factor;
    }
}